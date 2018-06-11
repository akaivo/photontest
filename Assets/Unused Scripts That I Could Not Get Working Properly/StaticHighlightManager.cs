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

    [SyncVar(hook = "OnUIDChange")]
    public string currentHighlightUID;
    private void OnUIDChange(string newUID)
    {
        SetPlayerHighlight(currentHighlightUID, false);
        SetPlayerHighlight(newUID, true);
        currentHighlightUID = newUID;

    }

    private void SetPlayerHighlight(string newUID, bool v)
    {
        foreach (var playerIdentity in FindObjectsOfType<PlayerIdentity>())
        {
            if (playerIdentity.hasUID(newUID))
            {
                playerIdentity.Set(v);
            }
        }
    }

    public override void OnStartClient()
    {
        SetPlayerHighlight(currentHighlightUID, true);
    }

    private void OnGUI()
    {
        //draw GUI only for owner (and also allow changes along with it)
        if (!hasAuthority) return;

        var playerNames = FindObjectsOfType<PlayerIdentity>();
        int count = playerNames.Length;
        for (int i = 0; i < count; i++)
        {
            ButtonPerPlayer(playerNames[i], i);
        }
    }

    /// <summary> Draw button and read input</summary>
    private void ButtonPerPlayer(PlayerIdentity playerIdentity, int i)
    {
        bool highlighted = playerIdentity.hasUID(currentHighlightUID);
        GUI.color = highlighted ? Color.yellow : Color.white;
        var r = new Rect(10, 200 + 50 * i, 200, 45);
        if (GUI.Button(r, playerIdentity.GetDeviceName()))
        {
            CmdSetHighlightUID(playerIdentity.GetUID());
        }
    }

    [Command]
    private void CmdSetHighlightUID(string newSelectedUID)
    {
        currentHighlightUID = newSelectedUID;
    }
}
