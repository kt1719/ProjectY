using System;
using UnityEngine;

namespace Mirror
{
    /// <summary>
    /// Component that will display the clients ping in milliseconds
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Ping Display")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-ping-display")]
    public class NetworkPingDisplay : MonoBehaviour
    {
        public Color color = Color.white;
        public int padding = 2;
        public int size = 25;
        int width = 150;
        int height = 25;

        void OnGUI()
        {
            // only while client is active
            if (!NetworkClient.active) return;

            // show rtt in bottom right corner, right aligned
            GUI.color = color;
            Rect rect = new Rect(Screen.width - width - padding, Screen.height - height - padding, width, height);
            GUIStyle style = GUI.skin.GetStyle("Label");
            style.fontSize = size;
            style.alignment = TextAnchor.MiddleRight;
            GUI.Label(rect, $"RTT: {Math.Round(NetworkTime.rtt * 1000)}ms", style);
            GUI.color = Color.white;
        }
    }
}
