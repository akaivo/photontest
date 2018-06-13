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

    public bool IsHosting
    {
        get { return m_isServer && m_isClient; }
    }

    public bool IsPureClient
    {
        get { return m_isPureClient; }
    }

    void Start ()
    {
        StartHosting();
	}
	
    public void StartHosting()
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

    public void Disconnect()
    {
        if(IsPureClient)
        {
            StopClient();
        }
    }

    public override void OnStartServer()
    {
        m_isServer = true;
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        m_isServer = false;
        base.OnStopServer();
    }

    public override void OnStartClient(NetworkClient client)
    {
        m_isClient = true;
        m_isPureClient = !m_isServer;
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        m_isClient = false;
        m_isPureClient = false;
        base.OnStopClient();
    }
}
