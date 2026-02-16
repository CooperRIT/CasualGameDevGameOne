using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Xml.Serialization;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[System.Serializable]
struct SpawnPointData
{
    public Vector3 spawnLocation_PlayerOne;
    public Vector3 spawnLocation_PlayerTwo;
}

public class PlayerSpawnManager : NetworkBehaviour
{
    public static PlayerSpawnManager Instance;

    [SerializeField] List<PlayerNetworkData> players = new List<PlayerNetworkData>();

    [SerializeField] List<SpawnPointData> spawnPoints = new List<SpawnPointData>();

    [SerializeField] List<GameObject> playerCameras = new List<GameObject>();

    [SerializeField] TextMeshProUGUI objectiveText; // Just a regular serialized field

    [SerializeField] LevelBuilder levelBuilder;

    [SerializeField] Transform winBox;

    public List<Sprite> sprites = new List<Sprite>();

    int levelIndex = 0;

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

        LoadLevel();

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

    private void LoadLevel()
    {
        levelBuilder.LoadLevelClientRpc(levelIndex);

        int previousLevelIndex = levelIndex - 1;

        if (previousLevelIndex <= -1)
        {
            return;
        }
        levelBuilder.UnloadPreviousLevelClientRpc(previousLevelIndex);
    }

    void TeleportPlayers()
    {
        players[0].transform.position = spawnPoints[levelIndex].spawnLocation_PlayerOne;
        players[1].transform.position = spawnPoints[levelIndex].spawnLocation_PlayerTwo;
    }

    [ClientRpc]
    void MakeObjectEnabledClientRpc()
    {
        objectiveText.gameObject.SetActive(true);
    }


    public void SetWinGameCondition()
    {
        gameStarted = false;

        /*if (IsServer)
        {
            SetObjectiveTextClientRpc("You win");
        }*/

        //Play some sort of animation

        //Increase level index
        levelIndex++;

        StartGame();
    }

    [ClientRpc]
    public void SetObjectiveTextClientRpc(string text)
    {
        objectiveText.text = text;
    }

    public void PlayerFellOff(int playerId)
    {
        if(IsServer && gameStarted)
        {
            players[0].MoveMeClientRpc(spawnPoints[levelIndex].spawnLocation_PlayerOne);
            return;
        }

        players[1].MoveMeClientRpc(spawnPoints[levelIndex].spawnLocation_PlayerTwo);
    }
}
