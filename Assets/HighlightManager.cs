using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HighlightManager : NetworkBehaviour {

    private void OnGUI()
    {
        if (!hasAuthority) return;

        var playerNames = FindObjectsOfType<PlayerName>();
        int count = playerNames.Length;
        for (int i = 0; i < count; i++)
        {
            var r = new Rect(10, 200 + 50 * i, 200, 50);
            GUI.Button(r, playerNames[i].deviceName);
        }
    }
}
