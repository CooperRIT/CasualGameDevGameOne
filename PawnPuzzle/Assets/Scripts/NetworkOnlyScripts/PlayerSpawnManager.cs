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

    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;

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

        gameStarted = true;

        yield return new WaitForSeconds(.5f);

        winBox.position = Vector3.zero;
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
        Debug.Log("teleport Players");
        players[0].MoveMeClientRpc(spawnPoints[levelIndex].spawnLocation_PlayerOne);
        players[1].MoveMeClientRpc(spawnPoints[levelIndex].spawnLocation_PlayerTwo);
    }

    [ClientRpc]
    void MakeObjectEnabledClientRpc()
    {
        objectiveText.gameObject.SetActive(true);
    }


    public void SetWinGameCondition()
    {
        if(!gameStarted)
        {
            return;
        }

        gameStarted = false;

        /*if (IsServer)
        {
            SetObjectiveTextClientRpc("You win");
        }*/

        //Play some sort of animation

        //play sound effect
        //SoundManager.instance.PlaySoundClip(winSound, winBox, 1f);

        //Increase level index
        levelIndex++;

        Debug.Log("we starting next level");

        StartCoroutine(nameof(StartGame));
    }

    [ClientRpc]
    public void SetObjectiveTextClientRpc(string text)
    {
        objectiveText.text = text;
    }

    public void PlayerFellOff(int playerId)
    {
        playerId -= 1;

        if(!gameStarted)
        {
            return;
        }

        Debug.Log(playerId);

        switch(playerId)
        {
            case 0:
                players[0].MoveMeClientRpc(spawnPoints[levelIndex].spawnLocation_PlayerOne);
                break;
            case 1:
                players[1].MoveMeClientRpc(spawnPoints[levelIndex].spawnLocation_PlayerTwo);
                break;
        }

    }


    #region LevelTwoFunctions

    /// <summary>
    /// This is much slower then I wanted, but I cannot pass through a gameobject and do not feel like directly referencing the gate here by making a system that-
    /// store indicies and matches them with teh gate, this is simple and clean(YOOOOO KINGDOM HEARTS REFERNCE)
    /// </summary>
    /// <param name="gameObjectName"></param>
    [ClientRpc]
    public void DisableGateClientRpc(string gameObjectName)
    {
        GameObject.Find(gameObjectName).SetActive(false);
    }


    #endregion
}
