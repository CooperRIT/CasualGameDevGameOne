using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] LayerMask layersToCheck;

    [SerializeField] PlayerNetworkData multiData;

    RaycastHit2D results;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        results = Physics2D.Raycast(transform.position, Vector3.forward, 10f, layersToCheck);

        if(results.collider == null)
        {
            //Debug.Log("teleporting player");
            PlayerSpawnManager.Instance.PlayerFellOff(multiData.Data.Value.PlayerID);
            return;
        }

        GameObject tempOBJ = results.collider.gameObject;

        switch (tempOBJ.layer)
        {
            //button
            case 9:
                Debug.Log("hit the button");
                tempOBJ.GetComponent<ITriggerable>().OnTrigger();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.forward * 10);
    }
}
