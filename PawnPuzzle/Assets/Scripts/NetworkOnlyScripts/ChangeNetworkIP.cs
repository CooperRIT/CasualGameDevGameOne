using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeNetworkIP : MonoBehaviour
{
    [SerializeField] private UnityTransport unityTransport;


    [SerializeField] private TMP_InputField ipText;

    private void Start()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork &&
                !IPAddress.IsLoopback(ip))
            {
                ipText.text = ip.ToString();

            }
        }

        OnEnterIP(ipText.text);
    }

    public void OnEnterIP(string ip)
    {
        unityTransport.ConnectionData.Address = ip;
    }

    
}
