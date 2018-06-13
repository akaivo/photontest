using Meshicon.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    public CustomNetworkDiscovery discovery;
    [SerializeField]
    private bool m_isHosting;
    [SerializeField]
    private bool m_isClient;

    void Start ()
    {
        SwitchToHosting();
	}
	
    public void SwitchToHosting()
    {
        if (m_isHosting) return;
        serverBindToIP = true;
        StartHost();
    }

    public override void OnStartServer()
    {
        m_isHosting = true;
        StartBroadcasting(SystemInfo.deviceName, networkPort);
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        m_isHosting = false;
        base.OnStopServer();
    }

    public void ConnectTo(string ip, int port)
    {
        if (m_isHosting) StopHost();
        networkAddress = ip;
        networkPort = port;
        serverBindAddress = ip;
        serverBindToIP = true;
        StartClient();
    }

    public override void OnStartClient(NetworkClient client)
    {
        if (discovery.Running) discovery.StopBroadcast();
        m_isClient = true;
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        m_isClient = false;
        base.OnStopClient();
    }

    private void StartBroadcasting(string deviceName, int networkPort)
    {
        if(discovery.Running) discovery.StopBroadcast();
        discovery.BroadcastData = deviceName + ":port:" + networkPort;
        discovery.StartAsServer();
    }

}
