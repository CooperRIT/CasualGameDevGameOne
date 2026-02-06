using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    public static PlayerSpawnManager Instance;

    [SerializeField] List<PlayerNetworkData> players = new List<PlayerNetworkData>();

    [SerializeField] List<Vector3> spawnPoints = new List<Vector3>();

    [SerializeField] List<GameObject> playerCameras = new List<GameObject>();

    [SerializeField] TextMeshProUGUI objectiveText; // Just a regular serialized field

    [SerializeField] LevelBuilder levelBuilder;

    [SerializeField] Transform winBox;

    public List<Sprite> sprites = new List<Sprite>();

    bool gameStarted;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called by PlayerNetworkData when player spawns
    public void RegisterPlayer(PlayerNetworkData player)
    {
        players.Add(player);

        Color assignedColor = players.Count == 1 ? Color.blue : Color.red;

        // Server directly sets NetworkVariable
        player.Data.Value = new PlayerMultiData(players.Count, players.Count - 1); // YOU DO NOT NEED ALL THIS DATA WILL OPTIMIZE

        if (players.Count == 2)
            StartCoroutine(nameof(StartGame));
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(.5f);

        TeleportPlayers();

        MakeObjectEnabledClientRpc();

        EnablePlayerSpecificCamerasClientRpc();

        EnableLevel();

        winBox.position = Vector3.zero;

        gameStarted = true;
    }

    /// <summary>
    /// Makes it so that the server will gain control over the first camera and the client will gain control over the first
    /// </summary>
    [ClientRpc]
    private void EnablePlayerSpecificCamerasClientRpc()
    {
        Camera.main.gameObject.SetActive(false);

        if(IsServer)
        {
            Debug.Log("I am the server and I am enabling the first player camera");
            playerCameras[0].gameObject.SetActive(true);
        }
        else
        {
            playerCameras[1].gameObject.SetActive(true);
        }
    }

    private void EnableLevel()
    {
        levelBuilder.LoadLevelClientRpc(0);
    }

    void TeleportPlayers()
    {
        // Send the network IDs and positions
        for (int i = 0; i < players.Count; i++)
        {
            players[i].MoveMeClientRpc(spawnPoints[i]);
            Debug.Log("teleported");
        }
    }

    [ClientRpc]
    void MakeObjectEnabledClientRpc()
    {
        objectiveText.gameObject.SetActive(true);
    }


    public void SetWinGameCondition()
    {
        if (IsServer)
        {
            SetObjectiveTextClientRpc("You win");
        }
    }

    [ClientRpc]
    public void SetObjectiveTextClientRpc(string text)
    {
        objectiveText.text = text;
    }

    public void PlayerFellOff(int playerId)
    {
        if (gameStarted)
        {
            players[playerId - 1].MoveMeClientRpc(spawnPoints[playerId - 1]);
        }
    }
}
