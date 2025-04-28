using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Manager HUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;

        public int offsetX;
        public int offsetY;

        private string customPort = "7777";

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            int width = 400;
            int buttonHeight = 40;
            int spacing = 10;

            bool isConnectedOrServer = NetworkClient.isConnected || NetworkServer.active;

            if (isConnectedOrServer)
            {
                width = 300;
                buttonHeight = 25;
            }

            int x = (Screen.width - width) / 2 + offsetX;
            int y = isConnectedOrServer ? 10 : (Screen.height / 2) + offsetY;

            GUILayout.BeginArea(new Rect(x, y, width, 9999));

            GUILayout.BeginVertical("box");

            if (!NetworkClient.isConnected && !NetworkServer.active)
                StartButtons(buttonHeight, spacing);
            else
                StatusLabels(buttonHeight, spacing);

            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                GUILayout.Space(spacing);

                if (GUILayout.Button("Klient je pøipraven", GUILayout.Height(buttonHeight)))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                        NetworkClient.AddPlayer();
                }
            }

            GUILayout.Space(spacing);
            StopButtons(buttonHeight, spacing);

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        void StartButtons(int buttonHeight, int spacing)
        {
            if (!NetworkClient.active)
            {
#if UNITY_WEBGL
                if (GUILayout.Button("Single Player", GUILayout.Height(buttonHeight)))
                {
                    NetworkServer.listen = false;
                    manager.StartHost();
                }
#else
                if (GUILayout.Button("Zapnout server a klienta)", GUILayout.Height(buttonHeight)))
                    manager.StartHost();
#endif

                GUILayout.Space(spacing);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Klient", GUILayout.Width(80), GUILayout.Height(buttonHeight)))
                    manager.StartClient();

                GUILayout.Space(10);

                manager.networkAddress = GUILayout.TextField(manager.networkAddress, GUILayout.Width(180), GUILayout.Height(buttonHeight));

                GUILayout.Space(10);

                if (Transport.active is PortTransport portTransport)
                {
                    customPort = GUILayout.TextField(customPort, GUILayout.Width(80), GUILayout.Height(buttonHeight));
                    if (ushort.TryParse(customPort, out ushort port))
                        portTransport.Port = port;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(spacing);

#if UNITY_WEBGL
                GUILayout.Box("(WebGL cannot be server)");
#else
                if (GUILayout.Button("Pouze server", GUILayout.Height(buttonHeight)))
                    manager.StartServer();
#endif
            }
            else
            {
                GUILayout.Label($"Pøipojuji do {manager.networkAddress}...", GUILayout.Height(buttonHeight));
                GUILayout.Space(spacing);
                if (GUILayout.Button("Zrušit pokus o pøipojení", GUILayout.Height(buttonHeight)))
                    manager.StopClient();
            }
        }

        void StatusLabels(int buttonHeight, int spacing)
        {
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: bìží pomocí {Transport.active}", GUILayout.Height(buttonHeight));
            }
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: bìží pomocí {Transport.active}", GUILayout.Height(buttonHeight));
            }
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: pøipojeno na {manager.networkAddress} pomocí {Transport.active}", GUILayout.Height(buttonHeight));
            }

            GUILayout.Space(spacing);
        }

        void StopButtons(int buttonHeight, int spacing)
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                GUILayout.BeginHorizontal();
#if UNITY_WEBGL
                if (GUILayout.Button("Stop Single Player", GUILayout.Height(buttonHeight)))
                    manager.StopHost();
#else
                if (GUILayout.Button("Zastavit server", GUILayout.Height(buttonHeight)))
                    manager.StopHost();
                GUILayout.Space(10);
                if (GUILayout.Button("Zastavit klienta", GUILayout.Height(buttonHeight)))
                    manager.StopClient();
#endif
                GUILayout.EndHorizontal();
            }
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Zastavit klienta", GUILayout.Height(buttonHeight)))
                    manager.StopClient();
            }
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Zastavit klienta", GUILayout.Height(buttonHeight)))
                    manager.StopServer();
            }
        }
    }
}
