using Meshicon.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    public CustomNetworkDiscovery discovery;
    [SerializeField]
    private bool m_isServer;
    [SerializeField]
    private bool m_isClient;
    public bool IsServer
    {
        get { return m_isServer; }
    }

    public bool IsPureClient
    {
        get { return m_isClient && !m_isServer; }
    }

    void Start ()
    {
        SwitchToHosting();
	}
	
    public void SwitchToHosting()
    {
        if (m_isServer)
        {
            Debug.LogWarning("Already hosting");
            return;
        }
        networkAddress = "localhost";
        serverBindToIP = true;
        StartHost();
    }

    public void Disconnect()
    {
        if(IsPureClient)
        {
            StopClient();
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("OnStartServer");
        m_isServer = true;
        StartBroadcasting(SystemInfo.deviceName, networkPort);
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        Debug.Log("OnStopServer");
        m_isServer = false;
        StopBroadcasting();
        base.OnStopServer();
    }

    public override void OnStartClient(NetworkClient client)
    {
        m_isClient = true;
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        m_isClient = false;
        base.OnStopClient();
    }

    public void JoinGameAt(string ip, int port)
    {
        if(m_isClient)
        {
            Debug.LogError("Already a client");
            return;
        }

        if (m_isServer)
        {
            StopHost();
        }

        networkAddress = ip;
        networkPort = port;
        serverBindAddress = ip;
        serverBindToIP = true;
        StartClient();
    }

    private void StartBroadcasting(string deviceName, int networkPort)
    {
        if(discovery.Running) discovery.StopBroadcast();
        discovery.BroadcastData = deviceName + ":port:" + networkPort;
        discovery.StartAsServer();
    }

    private void StopBroadcasting()
    {
        if (discovery.Running) discovery.StopBroadcast();
    }

}
