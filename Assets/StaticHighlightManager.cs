using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StaticHighlightManager : NetworkBehaviour {

    public static StaticHighlightManager Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if(Instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start ()
    {
	    	
	}
	
	void Update ()
    {
	    if(!hasAuthority)
        {
            ReadGrabAuthorityInput();
        }
	}

    private void ReadGrabAuthorityInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdRequestAuthority();
        }
    }

    [Command]
    private void CmdRequestAuthority()
    {

    }
}
