using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnTriggerWin : NetworkBehaviour
{
    [SerializeField] int storedID = 0;
    [SerializeField] int playersEntered = 0;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (IsServer && collision.gameObject.layer == 8)
        {
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
                Debug.Log("You won");
                PlayerSpawnManager.Instance.SetWinGameCondition();
                playersEntered = 0;
                storedID = 0;
            }
        }
    }
}
