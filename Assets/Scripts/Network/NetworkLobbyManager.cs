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
public class NetworkLobbyManager : Singleton<NetworkLobbyManager>
{
    public UnityEvent<Lobby> onLobbyCreated;
    public UnityEvent<Lobby> onLobbyJoined;
    public UnityEvent<string> onLobbyLeft;
    public UnityEvent<Lobby> onRefreshLobby;
    
    ConcurrentQueue<string> _createdLobbyIds = new ConcurrentQueue<string>();

    public Lobby currentLobby;

    public async void CreateLobby(string lobbyName, bool privacy)
    {
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = privacy;
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        currentLobby = lobby;
        onLobbyCreated?.Invoke(lobby);
        onLobbyJoined?.Invoke(lobby);
        
        _createdLobbyIds.Enqueue(lobby.Id);
        
        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 5));
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
            UpdateCurrentLobby(lobby);
            CanvasManager.GetInstance().SwitchCanvas(CanvasType.Room);
            currentLobby = lobby;
            onLobbyJoined?.Invoke(lobby);
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
            UpdateCurrentLobby(lobby);
            CanvasManager.GetInstance().SwitchCanvas(CanvasType.Room);
            currentLobby = lobby;
            onLobbyJoined?.Invoke(lobby);
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
            // ...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async Task<List<Player>> GetPlayersInLobby(string lobbyId)
    {
        var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);

        return lobby.Players;

    }

    public async void UpdateCurrentLobby(Lobby lobby)
    {
        if (lobby.HostId != AuthenticationService.Instance.PlayerId)
            return;
        lobby.Players.ForEach(x => LobbyService.Instance.UpdatePlayerAsync(
            lobbyId: lobby.Id,
            playerId: x.Id,
            options: new UpdatePlayerOptions()
            {
               
            }));
    }
    
    public async void LeaveLobby(string lobbyId)
    {
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            onLobbyLeft?.Invoke(lobbyId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    void OnApplicationQuit()
    {
        while (_createdLobbyIds.TryDequeue(out var lobbyId))
        {
            LobbyService.Instance.DeleteLobbyAsync(lobbyId);
        }
    }
    
}
