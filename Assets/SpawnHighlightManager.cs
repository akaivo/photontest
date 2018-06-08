using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnHighlightManager : NetworkBehaviour
{
    
    public GameObject highLightManagerPrefab;

    void Start()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        CmdSpawnHighlightManager();
#endif
    }

    [Command]
    public void CmdSpawnHighlightManager()
    {
        GameObject manager = Instantiate(highLightManagerPrefab) as GameObject; 
        NetworkServer.SpawnWithClientAuthority(manager, gameObject);
    }
}
