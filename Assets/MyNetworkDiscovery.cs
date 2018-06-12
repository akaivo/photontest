using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkDiscovery : NetworkDiscovery {

    private void Awake()
    {
        //Initialize();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
    }
}
