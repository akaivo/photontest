using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSendMesh : Photon.MonoBehaviour {

    public Mesh mesh;
    private MeshFilter meshFilter;

    private float m_xpos, m_ypos;
    private float m_height = 45;
    private float m_spacing = 5f;
    private bool m_canSend;

    void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.sharedMesh;
	}

    private void SendMesh(Mesh m)
    {
        Debug.Log("Sending mesh...");

        byte[] serializedMesh = SimpleMeshSerializer.Serialize(new Mesh[] { mesh });
        Debug.Log("Serialized mesh size: " + serializedMesh.LongLength);

        byte[] compressedSerializedMesh = CLZF2.Compress(serializedMesh);
        Debug.Log("Compressed serialized mesh size: " + compressedSerializedMesh.LongLength);

        photonView.RPC("ReceiveMesh", PhotonTargets.AllViaServer, compressedSerializedMesh);
    }

    [PunRPC]
    private void ReceiveMesh(byte[] compressedSerializedMesh)
    {
        Debug.Log("Received mesh!");

        Debug.Log("Compressed serialized mesh size: " + compressedSerializedMesh.LongLength);

        byte[] serializedMesh = CLZF2.Decompress(compressedSerializedMesh);
        Debug.Log("Serialized mesh size: " + serializedMesh.LongLength);

        List<Mesh> receivedMesh = (List<Mesh>)SimpleMeshSerializer.Deserialize(serializedMesh);
        meshFilter.mesh = receivedMesh[0];
    }

    private void OnGUI()
    {
        m_xpos = 10f;
        m_ypos = 10f;

        if (m_canSend && GUI.Button(GetNextRect(), "Send Mesh"))
        {
            SendMesh(mesh);
        };
    }

    public void OnJoinedRoom()
    {
        m_canSend = true;
    }

    private Rect GetNextRect()
    {
        Rect r = new Rect(m_xpos, m_ypos, 200, m_height);
        m_ypos += m_height + m_spacing;
        return r;
    }
}
