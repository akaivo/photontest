using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour {

    private void OnGUI()
    {
        var playerNames = FindObjectsOfType<PlayerName>();
        foreach (var playerName in playerNames)
        {
            GUI.Button(new Rect(100, 10, 150, 50), playerName.deviceName);
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
