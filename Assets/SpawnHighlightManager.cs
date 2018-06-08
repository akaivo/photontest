﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnHighlightManager : NetworkBehaviour
{
    
    public GameObject highLightManagerPrefab;

    void Start()
    {
        //only windows pc spawns highlight manager locally. 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if(isLocalPlayer)//without this also someone elses spawned player on win pc spawns an extra manager
        {
            CmdSpawnHighlightManager();
        }
#endif
    }

    [Command]
    public void CmdSpawnHighlightManager()
    {
        GameObject manager = Instantiate(highLightManagerPrefab) as GameObject; 
        NetworkServer.SpawnWithClientAuthority(manager, gameObject);
    }
}
