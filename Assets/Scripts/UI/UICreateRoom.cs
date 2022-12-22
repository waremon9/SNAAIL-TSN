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
    [SerializeField]
    private TMP_Text _errorMessage;

    [SerializeField] 
    private Toggle pricavyToogle;
    private void OnEnable()
    {
        _createRoomButton.onClick.AddListener(CreateNewServerAndRoom);
    }

    private void OnDisable()
    {
        _createRoomButton.onClick.RemoveListener(CreateNewServerAndRoom);
    }
    

    void CreateNewServerAndRoom()
    {        
        if (_lobbyName.text == null)
        {
            _errorMessage.text = "Please specify a room name.";
            return;
        }
        
        
        HostingManager.GetInstance().StartCoroutine(nameof(HostingManager.ConfigureTransportAndStartNgoAsHost), _maxPlayer);
        NetworkLobbyManager.GetInstance().CreateLobby(_lobbyName.text, pricavyToogle.isOn);
        CanvasManager.GetInstance().SwitchCanvas(CanvasType.Room);
    }
}
