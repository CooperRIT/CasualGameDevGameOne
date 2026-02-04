using Unity.Netcode;
using UnityEngine;

public class TempPlayerMovement : NetworkBehaviour
{
    [SerializeField] InputDetector iD;
    [SerializeField] float speed = 5f;
    [SerializeField] Transform playerTransform;
    [SerializeField] SpriteRenderer playerSprite;

    PlayerNetworkData networkData;

    Vector3 movementInputVec3 => iD.MovmentInput;

    void Awake()
    {
        networkData = GetComponent<PlayerNetworkData>();
        playerSprite = transform.parent.GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        // Only owner moves
        if (!IsOwner)
        {
            Destroy(this);
            return;
        }
    }

    void Update()
    {
        // Local movement
        playerTransform.position += movementInputVec3 * speed * Time.deltaTime;

        // Visual sync
        if (networkData != null)
        {
            playerSprite.color = networkData.Data.Value.Color;
        }
    }
}
