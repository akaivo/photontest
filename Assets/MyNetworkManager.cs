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
    [SerializeField]
    private bool m_isPureClient;

    public bool IsServer
    {
        get { return m_isServer; }
    }

    public bool IsPureClient
    {
        get { return m_isPureClient; }
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
        Debug.Log("OnStartClient");
        m_isClient = true;
        m_isPureClient = !m_isServer;
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        Debug.Log("OnsStopClient");
        if(m_isPureClient) SwitchToHosting();
        m_isClient = false;
        m_isPureClient = false;
        base.OnStopClient();
    }

    public void JoinGameAt(string ip, int port)
    {
        if(IsPureClient)
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
