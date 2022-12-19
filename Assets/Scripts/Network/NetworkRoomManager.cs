using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;

public class NetworkRoomManager : Singleton<NetworkRoomManager>
{
    public UnityEvent<Lobby> onLobbyCreated;
    public UnityEvent<string> onConnectionToLobby;
    public UnityEvent<string, string> onLeaveLobby;
    private void Start()
    {
        UnityServices.InitializeAsync();
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
                return;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    
    public async Task<Lobby> CreateNewLobby(string lobbyName,int maxPlayers,bool lobbyPrivacy)
    {
        await SignInAnonymouslyAsync();
        CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
        {
            IsPrivate = lobbyPrivacy
        };

         Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);
         onLobbyCreated?.Invoke(lobby);
         onConnectionToLobby?.Invoke(AuthenticationService.Instance.PlayerInfo.Id);
        return lobby;
    }

    public async Task<List<string>> FetchConnectedLobbies()
    {
        try
        {
            return await LobbyService.Instance.GetJoinedLobbiesAsync();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
            return null;
        }
    }

    async Task<QueryResponse> FetchLobbies(int lobbiesToShow, bool showOnlyOpenLobbies)
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();

            if (showOnlyOpenLobbies)
            {
                options.Filters = new List<QueryFilter>()
                {
                    new QueryFilter(field: QueryFilter.FieldOptions.AvailableSlots, op: QueryFilter.OpOptions.GT,
                        value: "0")
                };
            }

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            return lobbies;

        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
            return null;
        }
    }

    async Task<bool> JoinLobbyWithId(string lobbyId)
    {
        try
        {
            await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            return true;
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
            return false;
        }
    }
    
    async Task<bool> JoinLobbyWithCode(string lobbyCode)
    {
        try
        {
            await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            return true;
        }
        catch (LobbyServiceException e)
        {
            Debug.LogWarning(e);
            return false;
        } 
    }

    public async void LeaveLobby()
    {
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;
            string lobbyId = LobbyService.Instance.GetJoinedLobbiesAsync().ToString();
            onLeaveLobby?.Invoke(lobbyId, playerId);
            await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    
}
