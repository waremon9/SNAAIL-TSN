using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;

public class UIRoomManager : Singleton<UIRoomManager>
{
    public GameObject roomPlayerPrefab;
    
    [SerializeField]
    private TMP_Text _roomName;
    
    [SerializeField]
    private TMP_Text _joinCode;
    
    [SerializeField]
    private TMP_Text _lobbyCodeText;

    [SerializeField]
    private Button _refreshButton;
    
    [SerializeField]
    private VerticalLayoutGroup _verticalLayout;
    
    private string _lobbyId;
    
    private void OnEnable()
    {
        _refreshButton.onClick.AddListener(NetworkLobbyManager.GetInstance().UpdateCurrentLobby);
        HostingManager.GetInstance().onServerCreated.AddListener(ChangeServerJoinCodeInfo);
        NetworkLobbyManager.GetInstance().onLobbyCreated.AddListener(ChangeServerNameAndId);
        NetworkLobbyManager.GetInstance().onLobbyJoined.AddListener(RefreshPlayerInLobby);
        NetworkLobbyManager.GetInstance().onRefreshLobby.AddListener(RefreshPlayerInLobby);

    }
    
    private void OnDisable()
    {
        _refreshButton.onClick.RemoveListener(NetworkLobbyManager.GetInstance().UpdateCurrentLobby);
        HostingManager.GetInstance().onServerCreated.RemoveListener(ChangeServerJoinCodeInfo);
        NetworkLobbyManager.GetInstance().onLobbyCreated.RemoveListener(ChangeServerNameAndId);
        NetworkLobbyManager.GetInstance().onLobbyJoined.RemoveListener(RefreshPlayerInLobby);
    }

    void ChangeServerJoinCodeInfo()
    {
        _joinCode.text = $"Server Join Code : {HostingManager.GetInstance().JoinCode}";
    }

    void ChangeServerNameAndId(Lobby lobby)
    {
        _lobbyId = lobby.Id;
        _roomName.text =  $"room : {lobby.Name}";
        _lobbyCodeText.text = $"Room Code : {lobby.LobbyCode}";
    }
    
    private void RefreshPlayerInLobby(Lobby lobby)
    {
        RemovePlayerInLobby();

        var players = lobby.Players;
        
        players.ForEach(x => SetupNewPlayerLobby(x.Id));
    }

    void SetupNewPlayerLobby(string playerId)
    {
        Debug.Log($"Creating player : {playerId}");
        GameObject newPlayer = Instantiate(roomPlayerPrefab, _verticalLayout.transform);
        newPlayer.GetComponentInChildren<TMP_Text>().text = playerId;
    }
    
    void RemovePlayerInLobby()
    {
        foreach (var componentsInChild in _verticalLayout.GetComponentsInChildren<UIRoomPlayer>())
        {
            componentsInChild.RemoveSelf();
        }
    }

    public string GetLobbyId()
    {
        Debug.Log(_lobbyId);
        return _lobbyId;
    }
}
