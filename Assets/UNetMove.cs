using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UNetMove : NetworkBehaviour {

    [Range(1f, 10f)]
    public float speed = 2f;

	void Update ()
    {

        if (!isLocalPlayer) return;

        float dx = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float dz = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        transform.Translate(dx, 0, dz);

        if (Input.GetKey(KeyCode.Space)) transform.position = Vector3.zero;

    }
}
