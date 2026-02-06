using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnTriggerWin : NetworkBehaviour
{
    int storedID = 0;
    int playersEntered = 0;

    [SerializeField] PlayerSpawnManager spawnManager;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(IsServer)
        {
            Debug.Log("There is something inside me");
            int playerId = collision.transform.GetChild(0).GetComponent<PlayerNetworkData>().Data.Value.PlayerID;
            if(playerId == storedID)
            {

            }
            else
            {
                storedID = playerId;
                playersEntered++;
            }

            if(playersEntered == 2)
            {
                spawnManager.SetWinGameCondition();
            }
        }
    }
}
