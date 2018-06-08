using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerName : NetworkBehaviour {

    [SyncVar(hook = "OnNameChange")]
    public string deviceName;
    private void OnNameChange(string newName)
    {
        //When using a hook, variable is not automatically set.
        deviceName = newName;
        Debug.Log("Hook: " + newName);
        gameObject.name = "Player - " + newName;
    }

    [SyncVar]
    public string UID;

	public void Start()
    {
        if (isLocalPlayer)
        {
            //Send my data to server.
            Debug.Log("Client call CmdSetNameAndId");
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
        Debug.Log("Server CmdSetNameAndId run. " + newName);
        deviceName = newName;
        UID = newUID;
	}
}
