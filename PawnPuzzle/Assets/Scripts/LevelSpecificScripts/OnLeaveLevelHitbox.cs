using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnLeaveLevelHitbox : NetworkBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsServer)
        {
            Debug.Log("Left my hitbox");
        }
    }
}
