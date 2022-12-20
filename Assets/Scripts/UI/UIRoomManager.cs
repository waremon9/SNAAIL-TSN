using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Networking.Transport.Relay;

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
    private VerticalLayoutGroup _verticalLayout;

    private Dictionary<string,GameObject> _playerList = new();

    private string _lobbyId;
    
    private void OnEnable()
    {
        HostingManager.GetInstance().onServerCreated.AddListener(ChangeServerJoinCodeInfo);
        NetworkLobbyManager.GetInstance().onLobbyCreated.AddListener(ChangeServerNameAndId);
        NetworkLobbyManager.GetInstance().onLobbyJoined.AddListener(AddPlayerInRoom);
        NetworkLobbyManager.GetInstance().onLobbyLeft.AddListener(RemovePlayerInRoom);
    }
    
    private void OnDisable()
    {
        HostingManager.GetInstance().onServerCreated.RemoveListener(ChangeServerJoinCodeInfo);
        NetworkLobbyManager.GetInstance().onLobbyCreated.RemoveListener(ChangeServerNameAndId);
        NetworkLobbyManager.GetInstance().onLobbyJoined.RemoveListener(AddPlayerInRoom);
        NetworkLobbyManager.GetInstance().onLobbyLeft.RemoveListener(RemovePlayerInRoom);
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

    void AddPlayerInRoom(string playerId)
    {
        if (_playerList.ContainsKey(playerId))
            return;
        GameObject newPlayer = Instantiate(roomPlayerPrefab, _verticalLayout.transform);
        _playerList.Add(playerId, newPlayer);
        newPlayer.GetComponentInChildren<TMP_Text>().text = playerId;
    }

    void RemovePlayerInRoom(string lobbyId, string playerId)
    {
        GameObject playerToRemove = _playerList[playerId];
        _playerList.Remove(playerId);
        Destroy(playerToRemove);
    }

    public string GetLobbyId()
    {
        Debug.Log(_lobbyId);
        return _lobbyId;
    }
}
