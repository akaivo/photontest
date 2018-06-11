using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ManagerStealer : NetworkBehaviour {

    private NetworkIdentity stealerIdentity;
    private NetworkIdentity managerIdentity;

    private void Start()
    {
        stealerIdentity = GetComponent<NetworkIdentity>();
    }

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
        managerIdentity = StaticHighlightManager.Instance.GetComponent<NetworkIdentity>();
        managerIdentity.RemoveClientAuthority(managerIdentity.clientAuthorityOwner);
        managerIdentity.AssignClientAuthority(stealerIdentity.connectionToClient);
    }

    private void OnDestroy()
    {
        //otherwise the manager gets destroyed with the owning player
        managerIdentity.RemoveClientAuthority(managerIdentity.clientAuthorityOwner);
    }
}
