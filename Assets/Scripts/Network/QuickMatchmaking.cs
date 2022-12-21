using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickMatchmaking : MonoBehaviour
{
    [SerializeField] 
    private Button _quickPlayButton;

    private Lobby _connectedLobby;
    private QueryResponse _lobbies;
    private UnityTransport _transport;
    private const string JoinCodeKey = "j";
    private string _playerId;

    private void Start()
    {
        _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
    }

    private void OnEnable()
    {
        _quickPlayButton.onClick.AddListener(CreateOrJoinLobby);
    }

    private void OnDisable()
    {
        _quickPlayButton.onClick.RemoveListener(CreateOrJoinLobby);
    }

    public async void CreateOrJoinLobby()
    {
        //await Authenticate();
        _playerId = AuthenticationService.Instance.PlayerId;
        _connectedLobby = await QuickJoinLobby() ?? await CreateLobby();
    }

    private async Task Authenticate()
    {
        var options = new InitializationOptions();
        await UnityServices.InitializeAsync(options);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        _playerId = AuthenticationService.Instance.PlayerId;

    }

    private async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            var allocation = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            SetTransportAsClient(allocation);
            
            NetworkManager.Singleton.StartClient();
            CanvasManager.GetInstance().gameObject.SetActive(false);
            return lobby;

        }
        catch (Exception e)
        {
            Debug.Log($"No lobbies available");
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        try
        {
            const int maxPlayers = 4;

            var allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            var options = new CreateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject>
                    { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }

            };

            var lobby = await Lobbies.Instance.CreateLobbyAsync("ddd", maxPlayers, options);
            
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id,15));

            _transport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();
            
            NetworkManager.Singleton.SceneManager.LoadScene("Building_Life", LoadSceneMode.Additive);
            CanvasManager.GetInstance().gameObject.SetActive(false);
            
            return lobby;

        }
        catch (Exception e)
        {
            Debug.Log("Failed creating a lobby");
            return null;
        }
    }
    
    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
    
        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    private void SetTransportAsClient(JoinAllocation allocation)
    {
        _transport.SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData );
    }
    
    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            if (_connectedLobby == null)
                return;
            if (_connectedLobby.HostId == _playerId) Lobbies.Instance.DeleteLobbyAsync(_connectedLobby.Id);
            else Lobbies.Instance.RemovePlayerAsync(_connectedLobby.Id, _playerId);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
