using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SendRateChanger))]
public class SendRateChangerInput : NetworkBehaviour {

    [Range(1,5)]
    public int amount = 1;

	void Update ()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey(KeyCode.E)) GetComponent<SendRateChanger>().Increase(amount);
        if (Input.GetKey(KeyCode.Q)) GetComponent<SendRateChanger>().Decrease(amount);
    }
}
