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

    void Update ()
    {
	    if(hasAuthority)
        {
            Debug.Log("Mi Mi Mi");
        }
	}
}
