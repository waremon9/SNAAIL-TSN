using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class NetworkLobbyManager : Singleton<NetworkLobbyManager>
{
    public UnityEvent<Lobby> onLobbyCreated;
    public UnityEvent<string> onLobbyJoined;
    public UnityEvent<string, string> onLobbyLeft;
    public async void CreateLobby(string lobbyName, bool privacy)
    {
        int maxPlayers = 4;
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = privacy;
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        onLobbyCreated?.Invoke(lobby);
        onLobbyJoined?.Invoke(AuthenticationService.Instance.PlayerId);
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
        try
        {
            await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            if(LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode).IsCompleted)
                onLobbyJoined?.Invoke(AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinLobbyWithId(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            if (LobbyService.Instance.JoinLobbyByIdAsync(lobbyId).IsCompleted)
                onLobbyJoined?.Invoke(AuthenticationService.Instance.PlayerId);
            
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    //I'll rework that asap
    public async void LeaveLobby(string lobbyId)
    {
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
            onLobbyLeft?.Invoke(lobbyId, playerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
