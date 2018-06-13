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
    private float spacing = 50f;

    private enum DiscoveryState
    {
        Stopped,
        Listening,
        Broadcasting
    }
    private DiscoveryState state = DiscoveryState.Stopped;

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
            int port = ExtractPort(broadcast.broadcastData);
            if (GUI.Button(GetNextRect(), broadcast.broadcastData.ToString()))
            {
                manager.JoinGameAt(broadcast.serverAddress, port);
                StopBroadcasting();
                break;//dictionary gets cleared through JoinGameAt causing foreach to throw exception if loop continues
            };
        }

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
        Rect r = new Rect(xpos, ypos, 200, 45);
        ypos += spacing;
        return r;
    }

    private static int ExtractPort(string broadcastData)
    {
        return int.Parse(broadcastData.Split(':').Last());
    }
}
