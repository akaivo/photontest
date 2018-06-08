using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnHighlightManager : NetworkBehaviour
{
    
    public GameObject highLightManagerPrefab;

    //only windows pc spawns highlight manager locally. 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

    void Start()
    {
        if(isLocalPlayer)//without this also someone else's spawned player on win pc spawns an extra manager. we can't have that
        {
            CmdSpawnHighlightManager();
        }
    }

    [Command]
    public void CmdSpawnHighlightManager()
    {
        GameObject manager = Instantiate(highLightManagerPrefab) as GameObject; 
        NetworkServer.SpawnWithClientAuthority(manager, gameObject);
    }

#endif

}
