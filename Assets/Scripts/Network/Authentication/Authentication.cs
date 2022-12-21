using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParrelSync;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Authentication : Singleton<Authentication>
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymouslyAsync();
    }

    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            
            var options = new InitializationOptions();
        
            #if UNITY_EDITOR
                        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
            #endif
            
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
}