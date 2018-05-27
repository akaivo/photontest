using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SendRateChanger))]
public class SendRateDisplay : NetworkBehaviour {

    private int SendRate;
    private float SendInterval;

    private void OnEnable()
    {
        if (!isLocalPlayer) return;
        GetComponent<SendRateChanger>().OnSendRateChange += updateValues;
    }

    private void OnDisable()
    {
        if (!isLocalPlayer) return;
        GetComponent<SendRateChanger>().OnSendRateChange -= updateValues;
    }

    private void updateValues(int newSendRate, float newSendInterval)
    {
        if (!isLocalPlayer) return;
        SendRate = newSendRate;
        SendInterval = newSendInterval;
    }

    private void OnGUI()
    {
        if (!isLocalPlayer) return;
        GUI.Label(new Rect(10, 5, 200, 20), "SendRate: " + SendRate);
        GUI.Label(new Rect(10, 20, 200, 20), "SendInterval: " + SendInterval);
    }
}
