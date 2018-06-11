using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerIdentity : NetworkBehaviour {

    [SyncVar(hook = "OnNameChange")]
    [SerializeField]
    private string deviceName;
    private void OnNameChange(string newName)
    {
        //When using a hook, variable is not automatically set.
        deviceName = newName;
        gameObject.name = "Player - " + newName;
    }

    [SyncVar]
    [SerializeField]
    private string UID = string.Empty;

	public void Start()
    {
        if (isLocalPlayer)
        {
            //Send my data to server.
            CmdSetNameAndId(SystemInfo.deviceName, SystemInfo.deviceUniqueIdentifier);
        } else
        {
            //Hook is not called on startup automatically.
            OnNameChange(deviceName);
        }
	}
	
	[Command]
	private void CmdSetNameAndId(string newName, string newUID)
    {
        //Server sets syncvars. Because only it can.
        deviceName = newName;
        UID = newUID;
	}

    [SerializeField]
    private bool isHighlighted;

    public void Set(bool value)
    {
        isHighlighted = value;
        GetComponent<Renderer>().material.color = value ? Color.blue : Color.white;
    }

    public bool hasUID(string someUID)
    {
        return UID.Equals(someUID);
    }

    public string GetDeviceName()
    {
        return deviceName;
    }

    public string GetUID()
    {
        return UID;
    }
}
