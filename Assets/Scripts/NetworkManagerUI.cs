using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
public class NetworkManagerUI : MonoBehaviour {
    [SerializeField]
    private Button hostButton;
    [SerializeField]
    private Button serverButton;
    [SerializeField]
    private Button clientButton;
    private NetworkManager _networkManager;

    private void Awake() {
        this._networkManager = NetworkManager.Singleton;

        this.hostButton.onClick.AddListener(() => {
            this._networkManager.StartHost();
        });
        this.serverButton.onClick.AddListener(() => {
            this._networkManager.StartServer();
        });
        this.clientButton.onClick.AddListener(() => {
            this._networkManager.StartClient();
        });
    }
}