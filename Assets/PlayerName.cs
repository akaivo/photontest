using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerName : NetworkBehaviour {

    public string deviceName;
    public string UID;

	void Start ()
    {
        Debug.Log("Client call CmdSetNameAndId");
        CmdSetNameAndId(SystemInfo.deviceName, SystemInfo.deviceUniqueIdentifier);
	}
	
	[Command]
	private void CmdSetNameAndId(string newName, string newUID)
    {
        Debug.Log("Server CmdSetNameAndId run. " + newName);
        RpcSetNameAndId(newName, newUID);
	}

    [ClientRpc]
    private void RpcSetNameAndId(string newName, string newUID)
    {
        deviceName = newName;
        UID = newUID;
        Debug.LogFormat("RpcSetNameAndId; Name: {0}, UID: {1}", deviceName, UID);
        gameObject.name = newName;
    }
}
