using Meshicon.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    public CustomNetworkDiscovery discovery;
    private bool isHosting;

    void Start ()
    {
        SwitchToHosting();
	}
	
    public void SwitchToHosting()
    {
        if (isHosting) return;
        serverBindToIP = true;
        StartHost();
    }

    public override void OnStartServer()
    {
        isHosting = true;
        StartBroadcasting(SystemInfo.deviceName, networkPort);
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        isHosting = false;
        base.OnStopServer();
    }

    private void StartBroadcasting(string deviceName, int networkPort)
    {
        if(discovery.Running) discovery.StopBroadcast();
        discovery.broadcastData = deviceName + ":port:" + networkPort;
        discovery.Initialize();
        discovery.StartAsServer();
    }

}
