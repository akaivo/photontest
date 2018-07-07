using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// This script automatically connects to Photon (using the settings file),
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;

    public byte Version = 1;
    public int SendRate = 50;
    public int SendRateOnSerialize = 25;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = true;

    public virtual void Start()
    {
        PhotonNetwork.sendRate = SendRate;
        PhotonNetwork.sendRateOnSerialize = SendRateOnSerialize;
        Debug.Log("SendRate: " + PhotonNetwork.sendRate);
        Debug.Log("SendRateOnSerialize: " + PhotonNetwork.sendRateOnSerialize);

        PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }
    }


    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = SystemInfo.deviceUniqueIdentifier;
        PhotonNetwork.JoinOrCreateRoom("photontest", new RoomOptions() { MaxPlayers = 10 }, null);
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinOrCreateRoom("photontest", new RoomOptions() { MaxPlayers = 10 }, null);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()");
    }
}
