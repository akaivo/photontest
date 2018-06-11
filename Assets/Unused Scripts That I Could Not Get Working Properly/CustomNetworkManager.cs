using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public NetworkIdentity highLightManager;

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //If we don't remove ownreship, then highlight manager gets despawned along with the owning client.
        if(highLightManager.clientAuthorityOwner == conn)
            highLightManager.RemoveClientAuthority(conn);
        base.OnServerDisconnect(conn);
    }
}
