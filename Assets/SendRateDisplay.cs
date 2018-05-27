using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SendRateChanger))]
public class SendRateDisplay : MonoBehaviour {

    private int SendRate;
    private float SendInterval;

    private void OnEnable()
    {
        GetComponent<SendRateChanger>().OnSendRateChange += updateValues;
    }

    private void OnDisable()
    {
        GetComponent<SendRateChanger>().OnSendRateChange -= updateValues;
    }

    private void updateValues(int newSendRate, float newSendInterval)
    {
        SendRate = newSendRate;
        SendInterval = newSendInterval;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 200, 20), "SendRate: " + SendRate);
        GUI.Label(new Rect(10, 20, 200, 20), "SendInterval: " + SendInterval);
    }
}
