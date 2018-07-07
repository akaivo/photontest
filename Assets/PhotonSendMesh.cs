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
        Debug.LogFormat("Serialized mesh size: {0} KB", serializedMesh.LongLength / 1000);

        byte[] compressedSerializedMesh = CLZF2.Compress(serializedMesh);
        Debug.LogFormat("Compressed serialized mesh size: {0} KB", compressedSerializedMesh.LongLength / 1000);

        photonView.RPC("ReceiveMesh", PhotonTargets.AllViaServer, compressedSerializedMesh);
    }

    [PunRPC]
    private void ReceiveMesh(byte[] compressedSerializedMesh)
    {
        Debug.Log("Received mesh!");

        Debug.LogFormat("Compressed serialized mesh size: {0} KB", compressedSerializedMesh.LongLength / 1000);

        byte[] serializedMesh = CLZF2.Decompress(compressedSerializedMesh);
        Debug.LogFormat("Serialized mesh size: {0} KB", serializedMesh.LongLength / 1000);

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
