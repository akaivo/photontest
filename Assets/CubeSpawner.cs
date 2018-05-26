using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : Photon.MonoBehaviour {

    public void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("NetworkedCube", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

}
