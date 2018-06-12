#if ENABLE_UNET
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Meshicon.Networking
{
    public struct ExpiringBroadcastResult
    {
        public string serverAddress;
        public string broadcastData;
        public DateTime expiration;
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Meshicon/CustomNetworkDiscovery")]
    public class CustomNetworkDiscovery : MonoBehaviour
    {
        const int k_MaxBroadcastMsgSize = 1024;

        // config data
        [SerializeField]
        int m_BroadcastPort = 47777;

        [SerializeField]
        int m_BroadcastKey = 2222;

        [SerializeField]
        int m_BroadcastVersion = 1;

        [SerializeField]
        int m_BroadcastSubVersion = 1;

        [SerializeField]
        int m_BroadcastInterval = 1000;

        [SerializeField]
        string m_BroadcastData = "HELLO";

        // runtime data
        int m_HostId = -1;
        public bool Running { get; private set; }

        public bool IsServer { get; private set; }
        public bool IsClient { get; private set; }

        byte[] m_MsgOutBuffer;
        byte[] m_MsgInBuffer;
        HostTopology m_DefaultTopology;
        Dictionary<string, ExpiringBroadcastResult> m_BroadcastsReceived = new Dictionary<string, ExpiringBroadcastResult>();

        public int broadcastPort
        {
            get { return m_BroadcastPort; }
            set { m_BroadcastPort = value; }
        }

        public int broadcastKey
        {
            get { return m_BroadcastKey; }
            set { m_BroadcastKey = value; }
        }

        public int broadcastVersion
        {
            get { return m_BroadcastVersion; }
            set { m_BroadcastVersion = value; }
        }

        public int broadcastSubVersion
        {
            get { return m_BroadcastSubVersion; }
            set { m_BroadcastSubVersion = value; }
        }

        public int broadcastInterval
        {
            get { return m_BroadcastInterval; }
            set { m_BroadcastInterval = value; }
        }

        public string broadcastData
        {
            get { return m_BroadcastData; }
            set
            {
                m_BroadcastData = value;
                m_MsgOutBuffer = StringToBytes(m_BroadcastData);
            }
        }

        public int hostId
        {
            get { return m_HostId; }
            set { m_HostId = value; }
        }

        public Dictionary<string, ExpiringBroadcastResult> BroadcastsReceived
        {
            get { return m_BroadcastsReceived; }
        }

        static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public bool Initialize()
        {
            if (m_BroadcastData.Length >= k_MaxBroadcastMsgSize)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + k_MaxBroadcastMsgSize); }
                return false;
            }

            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }

            m_MsgOutBuffer = StringToBytes(m_BroadcastData);
            m_MsgInBuffer = new byte[k_MaxBroadcastMsgSize];
            m_BroadcastsReceived.Clear();

            ConnectionConfig cc = new ConnectionConfig();
            cc.AddChannel(QosType.Unreliable);
            m_DefaultTopology = new HostTopology(cc, 1);

            return true;
        }

        // listen for broadcasts
        public bool StartAsClient()
        {
            if (m_HostId != -1 || Running)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery StartAsClient already started"); }
                return false;
            }

            if (m_MsgInBuffer == null)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsClient, NetworkDiscovery is not initialized"); }
                return false;
            }

            m_HostId = NetworkTransport.AddHost(m_DefaultTopology, m_BroadcastPort);
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsClient - addHost failed"); }
                return false;
            }

            byte error;
            NetworkTransport.SetBroadcastCredentials(m_HostId, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, out error);

            Running = true;
            IsClient = true;
            if (LogFilter.logDebug) { Debug.Log("StartAsClient Discovery listening"); }
            return true;
        }

        // perform actual broadcasts
        public bool StartAsServer()
        {
            if (m_HostId != -1 || Running)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery StartAsServer already started"); }
                return false;
            }

            m_HostId = NetworkTransport.AddHost(m_DefaultTopology, 0);
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsServer - addHost failed"); }
                return false;
            }

            byte err;
            if (!NetworkTransport.StartBroadcastDiscovery(m_HostId, m_BroadcastPort, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, m_MsgOutBuffer, m_MsgOutBuffer.Length, m_BroadcastInterval, out err))
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + err); }
                return false;
            }

            Running = true;
            IsServer = true;
            if (LogFilter.logDebug) { Debug.Log("StartAsServer Discovery broadcasting"); }
            DontDestroyOnLoad(gameObject);
            return true;
        }

        public void StopBroadcast()
        {
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StopBroadcast not initialized"); }
                return;
            }

            if (!Running)
            {
                Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
                return;
            }
            if (IsServer)
            {
                NetworkTransport.StopBroadcastDiscovery();
            }

            NetworkTransport.RemoveHost(m_HostId);
            m_HostId = -1;
            Running = false;
            IsServer = false;
            IsClient = false;
            m_MsgInBuffer = null;
            if (LogFilter.logDebug) { Debug.Log("Stopped Discovery broadcasting"); }
        }

        void Update()
        {
            if (m_HostId == -1)
                return;

            if (IsServer)
                return;

            NetworkEventType networkEvent;
            do
            {
                int connectionId;
                int channelId;
                int receivedSize;
                byte error;
                networkEvent = NetworkTransport.ReceiveFromHost(m_HostId, out connectionId, out channelId, m_MsgInBuffer, k_MaxBroadcastMsgSize, out receivedSize, out error);

                if (networkEvent == NetworkEventType.BroadcastEvent)
                {
                    NetworkTransport.GetBroadcastConnectionMessage(m_HostId, m_MsgInBuffer, k_MaxBroadcastMsgSize, out receivedSize, out error);

                    string senderAddr;
                    int senderPort;
                    NetworkTransport.GetBroadcastConnectionInfo(m_HostId, out senderAddr, out senderPort, out error);

                    var recv = new ExpiringBroadcastResult();
                    recv.serverAddress = senderAddr;
                    var byteData = new byte[receivedSize];
                    Buffer.BlockCopy(m_MsgInBuffer, 0, byteData, 0, receivedSize);
                    recv.broadcastData = BytesToString(byteData);
                    recv.expiration = DateTime.Now.AddSeconds(5);
                    m_BroadcastsReceived[senderAddr] = recv;

                    OnReceivedBroadcast(senderAddr, BytesToString(m_MsgInBuffer));
                }
            }
            while (networkEvent != NetworkEventType.Nothing);
            RemoveOldReceivedBroadcasts();
        }

        private void RemoveOldReceivedBroadcasts()
        {
        }

        void OnDestroy()
        {
            if (IsServer && Running && m_HostId != -1)
            {
                NetworkTransport.StopBroadcastDiscovery();
                NetworkTransport.RemoveHost(m_HostId);
            }

            if (IsClient && Running && m_HostId != -1)
            {
                NetworkTransport.RemoveHost(m_HostId);
            }
        }

        public virtual void OnReceivedBroadcast(string fromAddress, string data)
        {
            //Debug.("Got broadcast from [" + fromAddress + "] " + data);
        }
    }
}
#endif

