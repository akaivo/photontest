using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Photon.MonoBehaviour {

    [Range(1f, 10f)]
    public float speed = 2f;
    
	void Update ()
    {

        if (!photonView.isMine) return;

        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(dx, 0, dz);

        if (Input.GetKey(KeyCode.Space)) transform.position = Vector3.zero;

    }
}
