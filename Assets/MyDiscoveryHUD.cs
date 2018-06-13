using Meshicon.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CustomNetworkDiscovery))]
public class MyDiscoveryHUD : MonoBehaviour {

    private bool listening;
    public CustomNetworkDiscovery discovery;
    public MyNetworkManager manager;

    private void OnGUI()
    {
        if(manager.IsPureClient)
        {
            ShowDisconnectButton();
        }
        else
        {
            if(listening)
            {
                ShowAvailableSessions();
            } else
            {
                ShowJoinOtherButton();
            }
            if(!manager.IsHosting)
            {
                if (GUI.Button(new Rect(10, 10 + 300, 200, 45), "Host"))
                {
                    manager.StartHosting();
                };
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
                listening = false;
                break;
            };
            i++;
        }

        if (GUI.Button(new Rect(10, 10 + 50 * i, 200, 45), "Cancel"))
        {
            StartBroadcasting();
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
        if (discovery.Running) discovery.StopBroadcast();
        listening = discovery.StartAsClient();
    }

    private void StartBroadcasting()
    {
        if (discovery.Running) discovery.StopBroadcast();
        listening = false;
        discovery.StartAsServer();
    }
}
