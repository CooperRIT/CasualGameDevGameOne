using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    public static PlayerSpawnManager Instance;

    [SerializeField] List<PlayerNetworkData> players = new List<PlayerNetworkData>();

    [SerializeField] List<Vector3> spawnPoints = new List<Vector3>();

    [SerializeField] GameObject objectiveText;

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
        if(IsHost)
        {
            Debug.Log("I am the Host's version");
        }
        if(IsClient)
        {
            Debug.Log("I am the client's version");
        }
        if(IsServer)
        {
            Debug.Log("I am the server");
        }

        players.Add(player);

        Color assignedColor = players.Count == 1 ? Color.blue : Color.red;

        // Server directly sets NetworkVariable
        player.Data.Value = new PlayerMultiData(assignedColor);

        if (players.Count == 2)
            StartGame();
    }

    void StartGame()
    {
        // Teleport on server
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.parent.position = spawnPoints[i];
        }

        TeleportPlayersClientRpc();

        MakeObjectEnabledServerRpc();

        MakeObjectEnabledClientRpc();
    }

    [ClientRpc]
    void TeleportPlayersClientRpc()
    {
        // Send the network IDs and positions
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.parent.position = spawnPoints[i];
        }
    }

    [ServerRpc]
    void MakeObjectEnabledServerRpc()
    {
        objectiveText.SetActive(true);
    }

    [ClientRpc]
    void MakeObjectEnabledClientRpc()
    {
        objectiveText.SetActive(true);

        Debug.Log("Hello");
    }
}
