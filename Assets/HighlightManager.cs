using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HighlightManager : NetworkBehaviour {

    [SyncVar]
    public string currentHighlightUID;

    private void OnGUI()
    {
        if (!hasAuthority) return;

        var playerNames = FindObjectsOfType<PlayerName>();
        int count = playerNames.Length;
        for (int i = 0; i < count; i++)
        {
            ButtonPerPlayer(playerNames[i], i);
        }
    }

    private void ButtonPerPlayer(PlayerName playerName, int i)
    {
        bool highlighted = playerName.UID.Equals(currentHighlightUID);
        GUI.color = highlighted ? Color.yellow : Color.white;
        var r = new Rect(10, 200 + 50 * i, 200, 45);
        if (GUI.Button(r, playerName.deviceName))
        {
            CmdSetHighlightUID(playerName.UID);
        }
    }

    [Command]
    private void CmdSetHighlightUID(string newSelectedUID)
    {
        currentHighlightUID = newSelectedUID;
    }
}
