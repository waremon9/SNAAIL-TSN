using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILeaveRoom : MonoBehaviour
{
    private Button _leaveRoomButton;

    private void OnEnable()
    {
        _leaveRoomButton.onClick.AddListener(RemovePlayerFromLobby);
    }

    private void OnDisable()
    {
        _leaveRoomButton.onClick.RemoveListener(RemovePlayerFromLobby);
    }
    
    private void Awake()
    {
        _leaveRoomButton = GetComponent<Button>();
    }

    void RemovePlayerFromLobby()
    {
        CanvasManager.GetInstance().SwitchCanvas(CanvasType.MainMenu);
    }
    
}
