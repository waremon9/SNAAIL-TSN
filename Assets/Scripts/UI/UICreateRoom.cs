using System;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using Random = UnityEngine.Random;

public class UICreateRoom : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _lobbyName;
    [SerializeField]
    private Toggle _privacyToggle;
    [SerializeField]
    private Button _createRoomButton;
    [SerializeField]
    private int _maxPlayer = 4;
    
    private NetworkRoomManager _lobbyManager;

    private void OnEnable()
    {
        _createRoomButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _createRoomButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void Awake()
    {
        _lobbyManager = NetworkRoomManager.GetInstance();
        
    }

    void OnButtonClicked()
    {
        _lobbyName.text ??= Random.Range(1, int.MaxValue).ToString();
        
        _lobbyManager.CreateNewLobby(_lobbyName.text, _maxPlayer, _privacyToggle.isOn);

        CanvasManager.GetInstance().SwitchCanvas(CanvasType.Room);
    }
}
