using Unity.Netcode;
using UnityEngine;
using System;

public struct PlayerMultiData : INetworkSerializable, IEquatable<PlayerMultiData>
{
    public int PlayerID;
    public int SpriteIndex;

    public PlayerMultiData(int playerID, int spriteIndex)
    {
        PlayerID = playerID;
        SpriteIndex = spriteIndex;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerID);
        serializer.SerializeValue(ref SpriteIndex);
    }

    public bool Equals(PlayerMultiData other)
    {
        return PlayerID == other.PlayerID
            && SpriteIndex == other.SpriteIndex;
    }
}

public class PlayerNetworkData : NetworkBehaviour
{
    public NetworkVariable<PlayerMultiData> Data = new NetworkVariable<PlayerMultiData>();
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        // Listen for changes
        Data.OnValueChanged += OnDataChanged;

        if (IsServer)
        {
            // Default
            Data.Value = new PlayerMultiData(0, 0);

            // Register with manager
            PlayerSpawnManager.Instance.RegisterPlayer(this);
            Debug.Log("This is the server");
        }

        // Apply immediately (late join safety)
        ApplyData(Data.Value);
    }

    void OnDataChanged(PlayerMultiData oldData, PlayerMultiData newData)
    {
        ApplyData(newData);
    }

    void ApplyData(PlayerMultiData data)
    {
        spriteRenderer.sprite = PlayerSpawnManager.Instance.sprites[data.SpriteIndex];
    }

    [ClientRpc]
    public void MoveMeClientRpc(Vector3 position)
    {
        transform.parent.position = position;
    }
}