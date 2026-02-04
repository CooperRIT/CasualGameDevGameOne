using Unity.Netcode;
using UnityEngine;
using System;

public struct PlayerMultiData : INetworkSerializable, IEquatable<PlayerMultiData>
{
    public Color Color;

    public PlayerMultiData(Color color)
    {
        Color = color;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        serializer.SerializeValue(ref Color);
    }

    public bool Equals(PlayerMultiData other)
    {
        return Color.Equals(other.Color);
    }
}

public class PlayerNetworkData : NetworkBehaviour
{
    public NetworkVariable<PlayerMultiData> Data =
        new NetworkVariable<PlayerMultiData>();

    private SpriteRenderer spriteRenderer;

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
            Data.Value = new PlayerMultiData(Color.white);

            // Register with manager
            PlayerSpawnManager.Instance.RegisterPlayer(this);

            Debug.Log("This is the server");
        }

        // Apply immediately (late join safety)
        ApplyColor(Data.Value.Color);
    }

    void OnDataChanged(PlayerMultiData oldData, PlayerMultiData newData)
    {
        ApplyColor(newData.Color);
    }

    void ApplyColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
    }
}
