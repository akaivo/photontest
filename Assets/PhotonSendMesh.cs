using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSendMesh : Photon.MonoBehaviour {

    public ButtonManager buttonManager;
    public Mesh mesh;
    public int chunkLength = 10000;
    private MeshFilter meshFilter;
    private int m_length;
    private int m_incomingOffset;
    private byte[] m_incomingCompressedMesh;
    private bool m_sending;

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

        SendMeshHeader(compressedSerializedMesh);
        StartCoroutine(SendMeshChunks(compressedSerializedMesh));
    }

    private void SendMeshHeader(byte[] compressedSerializedMesh)
    {
        photonView.RPC("ReceiveMeshHeader",PhotonTargets.AllViaServer, compressedSerializedMesh.Length);
    }

    private IEnumerator SendMeshChunks(byte[] compressedSerializedMesh)
    {
        int outgoingOffset = 0;

        byte[] chunk = new byte[chunkLength];

        while (outgoingOffset < compressedSerializedMesh.Length)
        {
            int length = Math.Min(chunkLength, compressedSerializedMesh.Length - outgoingOffset);
            Buffer.BlockCopy(compressedSerializedMesh, outgoingOffset,
                             chunk, 0,
                             length);

            outgoingOffset += chunkLength;
            m_sending = true;
            bool isLast = outgoingOffset >= compressedSerializedMesh.Length;
            photonView.RPC("ReceiveMeshChunk", PhotonTargets.AllViaServer, chunk, length, isLast);
            yield return new WaitWhile(() => m_sending);
        }
    }

    [PunRPC]
    private void ReceiveMeshHeader(int length)
    {
        m_length = length;
        m_incomingOffset = 0;
        m_incomingCompressedMesh = new byte[m_length];
        Debug.Log("Header. Length: " + length);
    }

    [PunRPC]
    private void ReceiveMeshChunk(byte[] chunk, int length, bool isLast)
    {
        m_sending = false;
        Buffer.BlockCopy(chunk, 0, m_incomingCompressedMesh, m_incomingOffset, length);
        m_incomingOffset += chunk.Length;
        if(isLast)
        {
            BuildMesh();
        }
    }

    private void BuildMesh()
    {
        Debug.Log("Received mesh!");
        Debug.LogFormat("Compressed serialized mesh size: {0} KB", m_incomingCompressedMesh.LongLength / 1000);

        byte[] serializedMesh = CLZF2.Decompress(m_incomingCompressedMesh);
        Debug.LogFormat("Serialized mesh size: {0} KB", serializedMesh.LongLength / 1000);

        List<Mesh> receivedMesh = (List<Mesh>)SimpleMeshSerializer.Deserialize(serializedMesh);
        meshFilter.mesh = receivedMesh[0];
    }

    public void OnJoinedRoom()
    {
        buttonManager.AddButton("Send " + gameObject.name, Send);
    }

    private void Send()
    {
        SendMesh(mesh);
    }
}
