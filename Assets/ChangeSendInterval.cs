using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSendInterval : MonoBehaviour {

    [Range(1f,100f)]
    public float SendRate = 30;
    private float CurrentSendRate;

	void Start ()
    {
        CurrentSendRate = Network.sendRate;
	}
	
	void Update ()
    {
        if (CurrentSendRate == SendRate) return;

        CurrentSendRate = SendRate;
        Network.sendRate = SendRate;
        Debug.Log("Network sendrate: " + Network.sendRate);
	}
}
