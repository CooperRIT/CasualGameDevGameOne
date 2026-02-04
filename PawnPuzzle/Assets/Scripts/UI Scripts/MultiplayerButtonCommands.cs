using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerButtonCommands : MonoBehaviour
{
    //There are many smarter ways that I Could do this, but I am just testing for now
    public void OnClickHost()
    {
        NetworkManagerSingleton.networkManager.StartHost();
    }

    public void OnClickClient()
    {
        NetworkManagerSingleton.networkManager.StartClient();
    }
}
