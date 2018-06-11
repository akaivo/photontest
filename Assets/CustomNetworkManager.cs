using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public NetworkIdentity staticHM;

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(staticHM.clientAuthorityOwner != null)
            staticHM.RemoveClientAuthority(conn);
        base.OnServerDisconnect(conn);
    }
}
