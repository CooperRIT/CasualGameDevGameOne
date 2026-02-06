using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] LayerMask ground;

    [SerializeField] PlayerNetworkData multiData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.Raycast(transform.position, Vector3.forward, 10, ground))
        {

        }
        else
        {
            Debug.Log("teleporting player");
            PlayerSpawnManager.Instance.PlayerFellOff(multiData.Data.Value.PlayerID);
        }
    }
}
