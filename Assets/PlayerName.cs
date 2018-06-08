using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerName : NetworkBehaviour {

    [SyncVar]
    public string deviceName;
    [SyncVar]
    public string UID;

	void Start ()
    {
        if (!isLocalPlayer) return;
        Debug.Log("Client call CmdSetNameAndId");
        CmdSetNameAndId(SystemInfo.deviceName, SystemInfo.deviceUniqueIdentifier);
	}
	
	[Command]
	private void CmdSetNameAndId(string newName, string newUID)
    {
        Debug.Log("Server CmdSetNameAndId run. " + newName);
        deviceName = newName;
        UID = newUID;
        //RpcSetNameAndId(newName, newUID);
	}
    /*
    [ClientRpc]
    private void RpcSetNameAndId(string newName, string newUID)
    {
        deviceName = newName;
        UID = newUID;
        Debug.LogFormat("RpcSetNameAndId; Name: {0}, UID: {1}", deviceName, UID);
        gameObject.name = newName;
    }*/
}
