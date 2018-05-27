using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkTransform))]
public class SendRateChanger : NetworkBehaviour
{

    [Range(1, 100)]
    public int SendRate = 30;
    private int CurrentSendRate = 0;

    public event Action<int, float> OnSendRateChange;

    void Update ()
    {
        SetSendRate(SendRate);
	}

    public void Increase(int amount)
    {
        SetSendRate(CurrentSendRate + amount);
    }

    public void Decrease(int amount)
    {
        SetSendRate(CurrentSendRate - amount);
    }

    private void SetSendRate(int newSendRate)
    {
        if (!isLocalPlayer) return;
        if (newSendRate <= 0 || newSendRate > 100) return;
        if (CurrentSendRate == newSendRate) return;

        CurrentSendRate = newSendRate;
        SendRate = newSendRate;
        GetComponent<NetworkTransform>().sendInterval = 1f / newSendRate;

        Debug.Log("Network transform sendrate changed: " + newSendRate);
        Debug.Log("Network transform sendInterval changed: " + GetComponent<NetworkTransform>().sendInterval);
        if (OnSendRateChange != null)
        {
            OnSendRateChange(newSendRate, GetComponent<NetworkTransform>().sendInterval);
        }

    }
}
