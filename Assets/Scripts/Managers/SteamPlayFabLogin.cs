using System;
using System.Collections;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using Steamworks;
using UnityEngine;

public class SteamPlayFabLogin : MonoBehaviour
{
    private const int MAX_LOGIN_RETRIES = 5;
    private const float INITIAL_RETRY_DELAY = 1f;

    HAuthTicket hAuthTicket;

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            TryLoginToPlayFabWithSteam(0);
        }
        else
        {
            Debug.LogError("Steam is not initialized.");
        }
    }

    private void TryLoginToPlayFabWithSteam(int retryCount, float delay = 0)
    {
        if (delay > 0)
        {
            StartCoroutine(RetryAfterDelay(retryCount, delay));
            return;
        }

        byte[] ticketBlob = new byte[1024];
        uint ticketSize = 0;

        string steamTicket = GetSteamAuthTicket(ref ticketBlob, ref ticketSize);

        // Execute PlayFab API call to log in with steam ticket
        PlayFabClientAPI.LoginWithSteam(new LoginWithSteamRequest
        {
            CreateAccount = true,
            SteamTicket = steamTicket
        }, OnLoginWithSteamToPlayFabComplete, (error) => OnLoginWithSteamToPlayFabFailed(error, retryCount));
    }

    private IEnumerator RetryAfterDelay(int retryCount, float delay)
    {
        yield return new WaitForSeconds(delay);
        TryLoginToPlayFabWithSteam(retryCount);
    }

    private string GetSteamAuthTicket(ref byte[] ticketBlob, ref uint ticketSize)
    {
        SteamNetworkingIdentity identity = new SteamNetworkingIdentity();

        // When you pass an object, the object can be modified by the callee. This function modifies the byte array you've passed to it.
        hAuthTicket = SteamUser.GetAuthSessionTicket(ticketBlob, ticketBlob.Length, out ticketSize, ref identity);

        // Resize the buffer to actual length
        Array.Resize(ref ticketBlob, (int)ticketSize);

        // Convert bytes to hexadecimal string
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte byteToConvert in ticketBlob)
        {
            stringBuilder.AppendFormat("{0:x2}", byteToConvert);
        }
        return stringBuilder.ToString();
    }

    private void OnLoginWithSteamToPlayFabComplete(LoginResult loginResult)
    {
        string steamDisplayName = SteamFriends.GetPersonaName();

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = steamDisplayName
        }, OnDisplayNameUpdateComplete, OnDisplayNameUpdateFailed);

        if (loginResult.NewlyCreated)
        {
            Debug.Log("Created new PlayFab account using Steam for user " + steamDisplayName + " successfully!");
        }

        Debug.Log("Login to PlayFab using Steam was successful!");

        // Cancel the Steam auth ticket when done authenticating
        SteamUser.CancelAuthTicket(hAuthTicket);
        Debug.Log("Canceled Steam auth ticket.");
    }

    private void OnLoginWithSteamToPlayFabFailed(PlayFabError error, int retryCount)
    {
        Debug.LogError("Login to PlayFab using Steam failed: " + error.GenerateErrorReport());

        // Cancel the Steam auth ticket when done authenticating
        SteamUser.CancelAuthTicket(hAuthTicket);
        Debug.Log("Canceled Steam auth ticket.");

        if (retryCount < MAX_LOGIN_RETRIES)
        {
            float delay = INITIAL_RETRY_DELAY * Mathf.Pow(2, retryCount);
            Debug.Log($"Retrying login to PlayFab using Steam in {delay} seconds... Attempt {retryCount + 1}");
            TryLoginToPlayFabWithSteam(retryCount + 1, delay);
        }
        else
        {
            Debug.LogError("Max retries reached. Login to PlayFab using Steam failed.");
        }
    }

    private void OnDisplayNameUpdateComplete(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("PlayFab display name updated to: " + result.DisplayName);
    }

    private void OnDisplayNameUpdateFailed(PlayFabError error)
    {
        Debug.Log("Failed to update PlayFab display name: " + error.GenerateErrorReport());
    }
}
