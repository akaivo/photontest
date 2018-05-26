using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : Photon.MonoBehaviour {

    public GameObject cube;

    public void OnJoinedRoom()
    {
        StartCoroutine(delayActivation());
    }

    private IEnumerator delayActivation()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        cube.SetActive(true);
        Debug.Log("Activated");
    }
}
