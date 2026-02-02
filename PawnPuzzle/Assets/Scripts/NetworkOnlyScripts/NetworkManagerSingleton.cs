using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerSingleton : NetworkBehaviour
{
    public static NetworkManager networkManager;


    private void Awake()
    {
        if (networkManager == null)
        {
            networkManager = GetComponent<NetworkManager>();
        }
        else
        {
            Destroy(this);
        }
    }
}
