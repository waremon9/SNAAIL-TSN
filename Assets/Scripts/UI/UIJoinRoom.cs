using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;

public class UIJoinRoom : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _joinCode;
    [SerializeField]
    private Button _connectToServerButton;

    [SerializeField]
    private ScrollRect _scrollRect;

    [SerializeField] 
    private TMP_Text _errorMessage;

    public GameObject UILobby;
    
    private void OnEnable()
    {
        _connectToServerButton.onClick.AddListener(ConnectToServer);
        HostingManager.GetInstance().onServerJoined.AddListener(FetchLobbies);
        NetworkLobbyManager.GetInstance().onLobbyJoined.AddListener(OnLobbyJoined);
    }

    private void OnDisable()
    {
        _connectToServerButton.onClick.RemoveListener(ConnectToServer);
        HostingManager.GetInstance().onServerJoined.RemoveListener(FetchLobbies);
        NetworkLobbyManager.GetInstance().onLobbyJoined.RemoveListener(OnLobbyJoined);
    }

    void ConnectToServer()
    {
        if (_joinCode.text == null)
        {
            _errorMessage.text = "Please specify a code.";
            return;
        }
        HostingManager.GetInstance()
            .StartCoroutine(nameof(HostingManager.ConfigureTransportAndStartNgoAsConnectingPlayer), _joinCode.text);
        HostingManager.GetInstance().onServerJoined?.Invoke();

    }

    async void FetchLobbies()
    {
        var lobbies = await NetworkLobbyManager.GetInstance().FetchLobbiesWithOption();
        
        lobbies.Results.ForEach(PopulateServerList);
    }

    void PopulateServerList(Lobby lobby)
    {
        GameObject SpawnedUILobby = Instantiate(UILobby, _scrollRect.content);
        UILobby lobbyInfo = SpawnedUILobby.GetComponent<UILobby>();
        lobbyInfo.SetPricavy(lobby.IsPrivate);
        lobbyInfo.SetLobbyName(lobby.Name);
        lobbyInfo.SetAvailableSlot(lobby.AvailableSlots, lobby.MaxPlayers);
        lobbyInfo.SetLobbyId(lobby.Id);
        if(lobby.IsPrivate)
            lobbyInfo.SetLobbyCode(lobby.LobbyCode);
        
    }

    void OnLobbyJoined(string playerId)
    {
        CanvasManager.GetInstance().SwitchCanvas(CanvasType.Room);   
    }
    
}
