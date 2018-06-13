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
        Debug.Log("OnStartServer");
        m_isHosting = true;
        StartBroadcasting(SystemInfo.deviceName, networkPort);
        base.OnStartServer();
    }

    public override void OnStopServer()
    {
        Debug.Log("OnStopServer");
        m_isHosting = false;
        StopBroadcasting();
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
