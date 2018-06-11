using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ManagerStealer : NetworkBehaviour {

	void Update ()
    {
        if (!isLocalPlayer) return;
        if(Input.GetKeyDown(KeyCode.T))
        {
            CmdTakeManagerAuthority();
        }
	}

    [Command]
    private void CmdTakeManagerAuthority()
    {
        StaticHighlightManager.Instance.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    }
}
