using Meshicon.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CustomNetworkDiscovery))]
public class MyDiscoveryHUD : MonoBehaviour {

    private bool listening;
    public CustomNetworkDiscovery discovery;

    private void OnGUI()
    {
        if(listening)
        {
            ShowAvailableSessions();
        } else
        {
            ShowJoinOtherButton();
        }
    }

    private void ShowAvailableSessions()
    {
        var broadcasts = discovery.BroadcastsReceived;
        int i = 0;
        foreach (var kvpair in broadcasts)
        {
            Debug.Log(kvpair.Key + ":" + kvpair.Value);
            var r = new Rect(10, 10 + 50 * i, 200, 45);
            if (GUI.Button(r, kvpair.Value.broadcastData.ToString()))
            {
                Debug.Log("Clicked: " + kvpair.Key + ":" + kvpair.Value);
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
