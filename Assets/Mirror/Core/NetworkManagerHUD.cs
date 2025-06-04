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

        private bool stylesInitialized = false;

        private GUIStyle buttonStyle;
        private GUIStyle textFieldStyle;
        private GUIStyle labelStyle;
        private GUIStyle boxStyle;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            if (!stylesInitialized)
            {
                InitStyles();
                stylesInitialized = true;
            }

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
            GUILayout.BeginVertical(boxStyle);

            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons(buttonHeight, spacing);

                // Tlaèítko "Navštívit náš web" pouze v hlavním menu
                GUILayout.Space(spacing);
                if (GUILayout.Button("Informace o høe", buttonStyle, GUILayout.Height(buttonHeight)))
                {
                    Application.OpenURL("https://www.linktr.ee/AetherClash");
                }
            }
            else
            {
                StatusLabels(buttonHeight, spacing);
            }

            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                GUILayout.Space(spacing);

                if (GUILayout.Button("Klient je pøipraven", buttonStyle, GUILayout.Height(buttonHeight)))
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

        void InitStyles()
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontSize = 19;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.normal.background = MakeTex(2, 2, new Color32(43, 28, 60, 255)); // Tmavì modrá
            buttonStyle.hover.background = MakeTex(2, 2, new Color32(103, 61, 165, 255)); // Pozadí tlaèítek - stisknuto
            buttonStyle.border = new RectOffset(4, 4, 4, 4);
            buttonStyle.margin = new RectOffset(4, 4, 4, 4);

            textFieldStyle = new GUIStyle(GUI.skin.textField);
            textFieldStyle.fontSize = 18;
            textFieldStyle.normal.textColor = Color.white;
            textFieldStyle.normal.background = MakeTex(2, 2, new Color32(103, 61, 165, 255)); // Pozadí políèek
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 18;
            labelStyle.normal.textColor = Color.white;

            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = MakeTex(2, 2, new Color32(26, 77, 119, 255)); // Modrofialová
            boxStyle.normal.textColor = Color.white;
            boxStyle.padding = new RectOffset(10, 10, 10, 10);
        }

        Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        void StartButtons(int buttonHeight, int spacing)
        {
            if (!NetworkClient.active)
            {
#if UNITY_WEBGL
                if (GUILayout.Button("Single Player", buttonStyle, GUILayout.Height(buttonHeight)))
                {
                    NetworkServer.listen = false;
                    manager.StartHost();
                }
#else
                if (GUILayout.Button("Zapnout server a klienta", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StartHost();
#endif
                GUILayout.Space(spacing);

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Klient", buttonStyle, GUILayout.Width(80), GUILayout.Height(buttonHeight)))
                    manager.StartClient();

                GUILayout.Space(10);

                manager.networkAddress = GUILayout.TextField(manager.networkAddress, textFieldStyle, GUILayout.Width(180), GUILayout.Height(buttonHeight));

                GUILayout.Space(10);

                if (Transport.active is PortTransport portTransport)
                {
                    customPort = GUILayout.TextField(customPort, textFieldStyle, GUILayout.Width(80), GUILayout.Height(buttonHeight));
                    if (ushort.TryParse(customPort, out ushort port))
                        portTransport.Port = port;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(spacing);

#if UNITY_WEBGL
                GUILayout.Box("(WebGL cannot be server)", boxStyle);
#else
                if (GUILayout.Button("Pouze server", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StartServer();
#endif
            }
            else
            {
                GUILayout.Label($"Pøipojuji do {manager.networkAddress}...", labelStyle, GUILayout.Height(buttonHeight));
                GUILayout.Space(spacing);
                if (GUILayout.Button("Zrušit pokus o pøipojení", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StopClient();
            }
        }

        void StatusLabels(int buttonHeight, int spacing)
        {
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: bìží pomocí {Transport.active}", labelStyle, GUILayout.Height(buttonHeight));
            }
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: bìží pomocí {Transport.active}", labelStyle, GUILayout.Height(buttonHeight));
            }
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: pøipojeno na {manager.networkAddress} pomocí {Transport.active}", labelStyle, GUILayout.Height(buttonHeight));
            }

            GUILayout.Space(spacing);
        }

        void StopButtons(int buttonHeight, int spacing)
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                GUILayout.BeginHorizontal();
#if UNITY_WEBGL
                if (GUILayout.Button("Stop Single Player", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StopHost();
#else
                if (GUILayout.Button("Zastavit server", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StopHost();
                GUILayout.Space(10);
                if (GUILayout.Button("Zastavit klienta", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StopClient();
#endif
                GUILayout.EndHorizontal();
            }
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Zastavit klienta", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StopClient();
            }
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Zastavit server", buttonStyle, GUILayout.Height(buttonHeight)))
                    manager.StopServer();
            }
        }
    }
}
