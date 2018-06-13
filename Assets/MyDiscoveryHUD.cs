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

    private enum DiscoveryState
    {
        Stopped,
        Listening,
        Broadcasting
    }
    private DiscoveryState state = DiscoveryState.Stopped;

    private void OnGUI()
    {
        if(manager.IsPureClient)
        {
            ShowDisconnectButton();
        }
        else
        {
            if(!manager.IsHosting)
            {
                if (GUI.Button(new Rect(10, 10 + 300, 200, 45), "Host"))
                {
                    manager.StartHosting();
                };
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

    private void ShowAvailableSessions()
    {
        var broadcasts = discovery.BroadcastsReceived;
        int i = 0;
        foreach (var kvpair in broadcasts)
        {
            var r = new Rect(10, 10 + 50 * i, 200, 45);
            string broadcastData = kvpair.Value.broadcastData;
            int port = int.Parse(broadcastData.Split(':').Last());
            string computer = broadcastData.Split(':').First();
            if (GUI.Button(r, kvpair.Value.broadcastData.ToString()))
            {
                manager.JoinGameAt(kvpair.Value.serverAddress, port);
                StopBroadcasting();
                break;//dictionary gets cleared through JoinGameAt causing foreach to throw exception if loop continues
            };
            i++;
        }

        if (GUI.Button(new Rect(10, 10 + 50 * i, 200, 45), "Cancel"))
        {
            StartBroadcasting(SystemInfo.deviceName, manager.networkPort);
        };
    }

    private void ShowJoinOtherButton()
    {
        var r = new Rect(10, 10, 200, 45);
        if (GUI.Button(r, "Join Other Session"))
        {
            StartListening();
        };
    }

    private void ShowDisconnectButton()
    {
        var r = new Rect(10, 10, 200, 45);
        if (GUI.Button(r, "Disconnect"))
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
}
