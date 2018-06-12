using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutomaticHosting : MonoBehaviour {

    public NetworkManager manager;

	void Start ()
    {
        Debug.LogWarning("BEFORE");
        Debug.Log("Bind: " + manager.serverBindAddress);
        Debug.Log("Address:" + manager.networkAddress);
        Debug.Log("Port: " + manager.networkPort);
        Debug.Log("Is bound: " + manager.serverBindToIP);
        Debug.Log("Player ip: " + Network.player.ipAddress);
        manager.serverBindToIP = true;
        manager.StartHost();

        Debug.LogWarning("AFTER");
        Debug.Log("Bind: " + manager.serverBindAddress);
        Debug.Log("Address:" + manager.networkAddress);
        Debug.Log("Port: " + manager.networkPort);
        Debug.Log("Is bound: " + manager.serverBindToIP);
        Debug.Log("Player ip: " + Network.player.ipAddress);
    }
}
