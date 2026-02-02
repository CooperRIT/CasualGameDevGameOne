using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class TempPlayerMovement : NetworkBehaviour
{

    [SerializeField] InputDetector iD;

    [SerializeField] float speed;

    [SerializeField] Transform playerTransform;

    Vector3 movementInputVec3 => iD.MovmentInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //this is for testing, will relocate a lot of this network stuff in the future
    private void OnNetworkInstantiate()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform.position += movementInputVec3 * speed * Time.deltaTime;
    }
}
