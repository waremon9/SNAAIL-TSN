using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Networking.Transport.Relay;

public class UIRoomManager : MonoBehaviour
{
    public GameObject roomPlayerPrefab;
    
    [SerializeField]
    private TMP_Text _roomName;
    
    [SerializeField]
    private TMP_Text _joinCode;
    
    [SerializeField]
    private TMP_Text _roomId;

    [SerializeField]
    private VerticalLayoutGroup _verticalLayout;

    private Dictionary<string,GameObject> _playerList = new();
    
    private void OnEnable()
    {
        HostingManager.GetInstance().onServerCreated.AddListener(ChangeRoomInfo);
    }
    
    private void OnDisable()
    {
        HostingManager.GetInstance().onServerCreated.RemoveListener(ChangeRoomInfo);
    }

    void ChangeRoomInfo()
    {
        //_roomName.text =  $"room : {lobby.Name}";
        //_roomId.text = $"Room ID : {lobby.Id}";
        _joinCode.text = $"Server Join Code : {HostingManager.GetInstance().JoinCode}";
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
