#if ENABLE_UNET
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Meshicon.Networking
{
    [CustomEditor(typeof(CustomNetworkDiscovery), true)]
    [CanEditMultipleObjects]
    public class CustomNetworkDiscoveryEditor : Editor
    {
        bool m_Initialized;
        CustomNetworkDiscovery m_Discovery;

        SerializedProperty m_BroadcastPortProperty;
        SerializedProperty m_BroadcastKeyProperty;
        SerializedProperty m_BroadcastVersionProperty;
        SerializedProperty m_BroadcastSubVersionProperty;
        SerializedProperty m_BroadcastIntervalProperty;
        SerializedProperty m_ReceivedBroadcastLifetimeProperty;
        SerializedProperty m_BroadcastDataProperty;

        GUIContent m_BroadcastPortLabel;
        GUIContent m_BroadcastKeyLabel;
        GUIContent m_BroadcastVersionLabel;
        GUIContent m_BroadcastSubVersionLabel;
        GUIContent m_BroadcastIntervalLabel;
        GUIContent m_ReceivedBroadcastLifetimeLabel;
        GUIContent m_BroadcastDataLabel;

        void Init()
        {
            if (m_Initialized)
            {
                if(m_BroadcastPortProperty == null)
                {

                } else
                {
                    return;
                }
            }

            m_Initialized = true;
            m_Discovery = target as CustomNetworkDiscovery;

            m_BroadcastPortProperty = serializedObject.FindProperty("m_BroadcastPort");
            m_BroadcastKeyProperty = serializedObject.FindProperty("m_BroadcastKey");
            m_BroadcastVersionProperty = serializedObject.FindProperty("m_BroadcastVersion");
            m_BroadcastSubVersionProperty = serializedObject.FindProperty("m_BroadcastSubVersion");
            m_BroadcastIntervalProperty = serializedObject.FindProperty("m_BroadcastInterval");
            m_ReceivedBroadcastLifetimeProperty = serializedObject.FindProperty("m_receivedBroadCastLifetime");
            m_BroadcastDataProperty = serializedObject.FindProperty("m_BroadcastData");

            m_BroadcastPortLabel = new GUIContent("Broadcast Port", "The network port to broadcast to, and listen on.");
            m_BroadcastKeyLabel = new GUIContent("Broadcast Key", "The key to broadcast. This key typically identifies the application.");
            m_BroadcastVersionLabel = new GUIContent("Broadcast Version", "The version of the application to broadcast. This is used to match versions of the same application.");
            m_BroadcastSubVersionLabel = new GUIContent("Broadcast SubVersion", "The sub-version of the application to broadcast.");
            m_BroadcastIntervalLabel = new GUIContent("Broadcast Interval", "How often in milliseconds to broadcast when running as a server.");
            m_ReceivedBroadcastLifetimeLabel = new GUIContent("Received BC Lifetime", "How many seconds to keep the broadcast in the broadcast dictionary after last receive.");
            m_BroadcastDataLabel = new GUIContent("Broadcast Data", "The data to broadcast when not using the NetworkManager");
        }

        public override void OnInspectorGUI()
        {
            Init();
            serializedObject.Update();
            DrawControls();
            serializedObject.ApplyModifiedProperties();
        }

        void DrawControls()
        {
            if (m_Discovery == null)
                return;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_BroadcastPortProperty, m_BroadcastPortLabel);
            EditorGUILayout.PropertyField(m_BroadcastKeyProperty, m_BroadcastKeyLabel);
            EditorGUILayout.PropertyField(m_BroadcastVersionProperty, m_BroadcastVersionLabel);
            EditorGUILayout.PropertyField(m_BroadcastSubVersionProperty, m_BroadcastSubVersionLabel);
            EditorGUILayout.PropertyField(m_BroadcastIntervalProperty, m_BroadcastIntervalLabel);
            EditorGUILayout.PropertyField(m_ReceivedBroadcastLifetimeProperty, m_ReceivedBroadcastLifetimeLabel);
            EditorGUILayout.PropertyField(m_BroadcastDataProperty, m_BroadcastDataLabel);

            EditorGUILayout.Separator();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            if (Application.isPlaying)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Host Id", m_Discovery.HostId.ToString());
                EditorGUILayout.LabelField("Running", m_Discovery.Running.ToString());
                EditorGUILayout.LabelField("Is Server", m_Discovery.IsServer.ToString());
                EditorGUILayout.LabelField("Is Client", m_Discovery.IsClient.ToString());
            }
        }
    }
}
#endif
