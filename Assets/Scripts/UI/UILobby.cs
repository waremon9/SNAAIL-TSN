using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UILobby : MonoBehaviour
{
    [SerializeField]
    private Image _privacyIcon;

    [SerializeField]
    private TMP_Text _lobbyName;
    
    [SerializeField]
    private TMP_Text _availableSlots;
    
    private string _lobbyCode;

    private Button _button;

    private bool _isPrivate;

    private string _lobbyId;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ConnectToLobby);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ConnectToLobby);
    }

    public void SetPricavy(bool privacy)
    {
        _isPrivate = privacy;
        
        if(privacy)
            _privacyIcon.gameObject.SetActive(true);
        else
            _privacyIcon.gameObject.SetActive(false);
    }

    public void SetLobbyName(string lobbyName)
    {
        _lobbyName.text = lobbyName;
    }

    public void SetAvailableSlot(int availableSlot, int maxPlayers)
    {
        _availableSlots.text = $"{availableSlot} / {maxPlayers}";
    }

    public void SetLobbyCode(string lobbyCode)
    {
        _lobbyCode = lobbyCode;
        Debug.Log(_lobbyCode);
    }

    public void SetLobbyId(string lobbyId)
    {
        _lobbyId = lobbyId;
    }
    
    void ConnectToLobby()
    {
        if(_isPrivate)
            NetworkLobbyManager.GetInstance().JoinLobbyWithCode(_lobbyCode);
        else
            NetworkLobbyManager.GetInstance().JoinLobbyWithId(_lobbyId);
    }
}
