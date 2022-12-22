using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkLobbyManager : Singleton<NetworkLobbyManager>
{
    public UnityEvent<Lobby> onLobbyCreated;
    public UnityEvent<Lobby> onLobbyJoined;
    public UnityEvent<string> onLobbyLeft;
    public UnityEvent<Lobby> onRefreshLobby;
    
    ConcurrentQueue<string> _createdLobbyIds = new ConcurrentQueue<string>();

    private Lobby _currentLobby;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
    public async void CreateLobby(string lobbyName, bool privacy)
    {
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = privacy;
        _currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        onLobbyCreated?.Invoke(_currentLobby);
        onLobbyJoined?.Invoke(_currentLobby);
        _createdLobbyIds.Enqueue(_currentLobby.Id);
        UpdateCurrentLobby();     
        StartCoroutine(HeartbeatLobbyCoroutine(_currentLobby.Id, 5));
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
    
    public async Task<QueryResponse> FetchLobbiesWithOption()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
    
            return lobbies;
            //...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinLobbyWithCode(string lobbyCode)
    {
        Debug.Log($" Player {AuthenticationService.Instance.PlayerId} is trying to connect");
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            _currentLobby = lobby;
            UpdateCurrentLobby();
            onLobbyJoined?.Invoke(lobby);
            NetworkManager.Singleton.SceneManager.LoadScene("BuildingLife", LoadSceneMode.Additive);
            
            CanvasManager.GetInstance().gameObject.SetActive(false);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyWithId(string lobbyId)
    {
        Debug.Log($" Player {AuthenticationService.Instance.PlayerId} is trying to connect");
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            _currentLobby = lobby;
            UpdateCurrentLobby();
            onLobbyJoined?.Invoke(lobby); 
            NetworkManager.Singleton.SceneManager.LoadScene("BuildingLife", LoadSceneMode.Additive);
            
            CanvasManager.GetInstance().gameObject.SetActive(false);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            // Quick-join a random lobby with a maximum capacity of 10 or more players.
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            options.Filter = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.MaxPlayers,
                    op: QueryFilter.OpOptions.GE,
                    value: "10")
            };

            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            onLobbyJoined?.Invoke(lobby);
            
            NetworkManager.Singleton.SceneManager.LoadScene("BuildingLife", LoadSceneMode.Additive);
            
            CanvasManager.GetInstance().gameObject.SetActive(false);
            
            // ...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

<<<<<<< HEAD
=======
    public async Task<List<Unity.Services.Lobbies.Models.Player>> GetPlayersInLobby(string lobbyId)
    {
        var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);

        return lobby.Players;

    }

>>>>>>> origin/master
    [ServerRpc]
    public void UpdateCurrentLobby()
    {
        _currentLobby.Players.ForEach(x => LobbyService.Instance.UpdatePlayerAsync(
            lobbyId: _currentLobby.Id,
            playerId: x.Id,
            options: new UpdatePlayerOptions()
            {
               
            }));
    }

    public async void LeaveLobby()
    {
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(_currentLobby.Id, playerId);
            onLobbyLeft?.Invoke(_currentLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    void OnClientConnected(ulong obj)
    {
        Debug.Log($"blablablablabla{obj}");
    }
    
    void OnApplicationQuit()
    {
        while (_createdLobbyIds.TryDequeue(out var lobbyId))
        {
            LobbyService.Instance.DeleteLobbyAsync(lobbyId);
        }
    }
    
}
