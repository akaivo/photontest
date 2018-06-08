using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnHighlightManager : NetworkBehaviour {

    public GameObject highLightManagerPrefab;

    void Start()
    {
    CmdSpawnHighlightManager(); //THIS SENDS A REQUEST TO THE SERVER TO SPAWN A MANAGER.
    }

    [Command]    //LIKE WITH ANY COMMAND THIS CODE IS ONLY EXECUTED ON THE SERVER.
    public void CmdSpawnHighlightManager()
    {
        GameObject manager = Instantiate(highLightManagerPrefab) as GameObject; //SpawnWithClientAuthority WORKS JUST LIKE NetworkServer.Spawn ...THE
                                                                                                            //GAMEOBJECT MUST BE INSTANTIATED FIRST.

        NetworkServer.SpawnWithClientAuthority(manager, gameObject); //THIS WILL SPAWN THE troop THAT WAS CREATED ABOVE AND GIVE AUTHORITY TO THIS PLAYER. THIS PLAYER (GAMEOBJECT) MUST
                                                                   //HAVE A NetworkIdentity ON IT WITH Local Player Authority CHECKED.
    }
}
