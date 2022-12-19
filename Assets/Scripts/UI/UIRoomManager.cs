using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomManager : MonoBehaviour
{
    private TMPro.TMP_Text _roomName;
    
    [SerializeField]
    private VerticalLayoutGroup _verticalLayout;

    public GameObject roomPlayerPrefab;
    private Dictionary<string,GameObject> _playerList = new();
    
    private void OnEnable()
    {
        NetworkRoomManager.GetInstance().onLobbyCreated.AddListener(ChangeRoomName);
        NetworkRoomManager.GetInstance().onConnectionToLobby.AddListener(AddPlayerInRoom);
        NetworkRoomManager.GetInstance().onLeaveLobby.AddListener(RemovePlayerInRoom);
    }
    
    private void OnDisable()
    {
        NetworkRoomManager.GetInstance().onLobbyCreated.RemoveListener(ChangeRoomName);
        NetworkRoomManager.GetInstance().onConnectionToLobby.RemoveListener(AddPlayerInRoom);
        NetworkRoomManager.GetInstance().onLeaveLobby.RemoveListener(RemovePlayerInRoom);
    }

    void ChangeRoomName(Lobby lobby)
    {
        _roomName.text =  "room :" + lobby.Name;
    }

    void AddPlayerInRoom(string playerId)
    {
        GameObject newPlayer = Instantiate(roomPlayerPrefab, _verticalLayout.transform);
        _playerList.Add(playerId, newPlayer);
        newPlayer.GetComponentInChildren<TMPro.TMP_Text>().text = playerId;
    }

    void RemovePlayerInRoom(string lobbyId, string playerId)
    {
        GameObject playerToRemove = _playerList[playerId];
        Debug.Log(playerToRemove);
        if (playerToRemove == null)
            return;
        _playerList.Remove(playerId);
        Destroy(playerToRemove);
    }

}
