using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button serverOnlyButton;
    public Button stopHostButton;
    public Button stopClientButton;
    public Text statusText;
    public InputField addressInput;
    public InputField portInput;

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        serverOnlyButton.onClick.AddListener(StartServerOnly);
        stopHostButton.onClick.AddListener(StopHost);
        stopClientButton.onClick.AddListener(StopClient);
        
        UpdateStatus();
    }

    void StartHost()
    {
        NetworkManager.singleton.StartHost();
        UpdateStatus();
    }

    void StartClient()
    {
        NetworkManager.singleton.networkAddress = addressInput.text;
        NetworkManager.singleton.GetComponent<TelepathyTransport>().port = ushort.Parse(portInput.text);
        NetworkManager.singleton.StartClient();
        UpdateStatus();
    }

    void StartServerOnly()
    {
        NetworkManager.singleton.StartServer();
        UpdateStatus();
    }

    void StopHost()
    {
        NetworkManager.singleton.StopHost();
        UpdateStatus();
    }

    void StopClient()
    {
        NetworkManager.singleton.StopClient();
        UpdateStatus();
    }

    void UpdateStatus()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            statusText.text = "Host: running";
        else if (NetworkServer.active)
            statusText.text = "Server: running";
        else if (NetworkClient.isConnected)
            statusText.text = "Client: connected";
        else
            statusText.text = "Offline";
    }
}
