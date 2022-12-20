using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIJoinRoom : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _joinCode;
    [SerializeField]
    private Button _connectToServerButton;

    [SerializeField] 
    private TMP_Text _errorMessage;
    
    private void OnEnable()
    {
        _connectToServerButton.onClick.AddListener(ConnectToServer);
    }

    private void OnDisable()
    {
        _connectToServerButton.onClick.RemoveListener(ConnectToServer);
    }

    async void ConnectToServer()
    {
        if (_joinCode.text == null)
        {
            _errorMessage.text = "Please specify a code.";
            return;
        }

        HostingManager.GetInstance()
            .StartCoroutine(nameof(HostingManager.ConfigureTransportAndStartNgoAsConnectingPlayer), _joinCode.text);
        //Populate Lobby List
    }
}
