using Meshicon.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CustomNetworkDiscovery))]
public class MyDiscoveryHUD : MonoBehaviour {

    public CustomNetworkDiscovery discovery;
    public MyNetworkManager manager;

    private float xpos, ypos;
    private float height = 45;
    private float spacing = 5f;
    private string customAddress, customPort;

    private enum DiscoveryState
    {
        Stopped,
        Listening,
        Broadcasting
    }
    private DiscoveryState state = DiscoveryState.Stopped;

    private void Start()
    {
        customAddress = manager.networkAddress;
        customPort = manager.networkPort.ToString();
    }
    private void OnGUI()
    {
        xpos = 10f;
        ypos = 10f;

        if(manager.IsPureClient)
        {
            ShowDisconnectButton();
        }
        else
        {
            if(!manager.IsHosting)
            {
                ShowHostButton();
            }
            switch (state)
            {
                case DiscoveryState.Stopped:
                    {
                        if(manager.IsHosting && !discovery.Running)
                            StartBroadcasting(SystemInfo.deviceName, manager.networkPort);
                        ShowJoinOtherButton();
                    }
                    break;
                case DiscoveryState.Listening:
                    {
                        ShowAvailableSessions();
                    }
                    break;
                case DiscoveryState.Broadcasting:
                    {
                        ShowJoinOtherButton();
                    }
                    break;
                default:
                    break;
            }
        }
        
    }

    private void ShowHostButton()
    {
        if (GUI.Button(GetNextRect(), "Host"))
        {
            manager.StartHosting();
        };
    }

    private void ShowAvailableSessions()
    {

        if (GUI.Button(GetNextRect(), "Cancel"))
        {
            StartBroadcasting(SystemInfo.deviceName, manager.networkPort);
        };

        var broadcasts = discovery.BroadcastsReceived;
        foreach (var broadcast in broadcasts.Values)
        {
            if (GUI.Button(GetNextRect(), ExtractComputerName(broadcast.broadcastData)))
            {
                manager.JoinGameAt(broadcast.serverAddress, ExtractPort(broadcast.broadcastData));
                StopBroadcasting();
                break;//dictionary gets cleared through JoinGameAt causing foreach to throw exception if loop continues
            };
        }

        ShowCustomSession();
    }

    private void ShowCustomSession()
    {
        customAddress = GUI.TextField(GetNextNarrowRect(), customAddress, 15);
        customPort = GUI.TextField(GetNextNarrowRect(), customPort, 15);
        if (GUI.Button(GetNextNarrowRect(), "Join"))
        {
            manager.JoinGameAt(customAddress, int.Parse(customPort));
        };
    }

    private void ShowJoinOtherButton()
    {
        if (GUI.Button(GetNextRect(), "Join Other Session"))
        {
            StartListening();
        };
    }

    private void ShowDisconnectButton()
    {
        if (GUI.Button(GetNextRect(), "Disconnect"))
        {
            manager.Disconnect();
        };
    }

    private void StartListening()
    {
        StopBroadcasting();
        bool listening = discovery.StartAsClient();
        state = listening ? DiscoveryState.Listening : DiscoveryState.Stopped;
    }

    private void StartBroadcasting(string deviceName, int networkPort)
    {
        StopBroadcasting();
        discovery.BroadcastData = deviceName + ":port:" + networkPort;
        bool broadcasting = discovery.StartAsServer();
        state = broadcasting ? DiscoveryState.Broadcasting: DiscoveryState.Stopped;
    }

    private void StopBroadcasting()
    {
        if (discovery.Running) discovery.StopBroadcast();
        state = DiscoveryState.Stopped;
    }

    private Rect GetNextRect()
    {
        Rect r = new Rect(xpos, ypos, 200, height);
        ypos += height + spacing;
        return r;
    }

    private Rect GetNextNarrowRect()
    {
        Rect r = new Rect(xpos, ypos, 200, height / 2f);
        ypos += height / 2f + spacing;
        return r;
    }


    private static int ExtractPort(string broadcastData)
    {
        return int.Parse(broadcastData.Split(':').Last());
    }

    private static string ExtractComputerName(string broadcastData)
    {
        return broadcastData.Split(':').First();
    }
}
