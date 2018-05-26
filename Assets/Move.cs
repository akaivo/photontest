using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Photon.MonoBehaviour {

    [Range(0.01f, 1f)]
    public float speed = 0.1f;

    private Vector3 direction;
    
	void Update ()
    {

        if (!photonView.isMine) return;

        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;

        transform.Translate(direction * speed);

        if (Input.GetKey(KeyCode.Space)) transform.position = Vector3.zero;

    }
}
