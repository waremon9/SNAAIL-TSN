using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private TMPro.TMP_Text _roomName;
    private HorizontalLayoutGroup _horizontalLayout;

    public GameObject roomPlayerPrefab;
    private Dictionary<ulong,GameObject> _playerList;
    
    private void OnEnable()
    {
        if (_networkManager == null)
            return;
        _networkManager.OnClientConnectedCallback += AddPlayerInRoom;
        _networkManager.OnClientDisconnectCallback += RemovePlayerInRoom;
    }
    
    private void OnDisable()
    {
        if (_networkManager == null)
            return;
        _networkManager.OnClientConnectedCallback -= AddPlayerInRoom;
        _networkManager.OnClientDisconnectCallback -= RemovePlayerInRoom;
    }
    
    private void Start()
    {
        _networkManager = NetworkManager.Singleton;
        _horizontalLayout = GetComponentInChildren<HorizontalLayoutGroup>();
    }

    void ChangeRoomName(string newName)
    {
        _roomName.text = newName;
    }

    void AddPlayerInRoom(ulong playerId)
    {
        GameObject newPlayer = Instantiate(roomPlayerPrefab, _horizontalLayout.transform);
        _playerList.Add(playerId, newPlayer);
        newPlayer.GetComponentInChildren<TMPro.TMP_Text>().text = playerId.ToString();
    }

    void RemovePlayerInRoom(ulong playerId)
    {
        GameObject playerToRemove;
        _playerList.TryGetValue(playerId, out playerToRemove);
        if (playerToRemove == null)
            return;
        Destroy(playerToRemove);
    }

}
