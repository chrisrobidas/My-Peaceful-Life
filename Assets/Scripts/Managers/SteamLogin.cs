using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using Steamworks;
using UnityEngine;

public class SteamLogin : MonoBehaviour
{
    private void Start()
    {
        if (SteamManager.Initialized)
        {
            // Execute PlayFab API call to log in with steam ticket
            PlayFabClientAPI.LoginWithSteam(new LoginWithSteamRequest
            {
                CreateAccount = true,
                SteamTicket = GetSteamAuthTicket()
            }, OnComplete, OnFailed);
        }
    }

    //This method returns
    public string GetSteamAuthTicket()
    {
        byte[] ticketBlob = new byte[1024];
        uint ticketSize;

        SteamNetworkingIdentity identity = new SteamNetworkingIdentity();

        // Retrieve ticket; hTicket should be a field in the class so you can use it to cancel the ticket later
        // When you pass an object, the object can be modified by the callee. This function modifies the byte array you've passed to it.
        HAuthTicket hTicket = SteamUser.GetAuthSessionTicket(ticketBlob, ticketBlob.Length, out ticketSize, ref identity);

        // Resize the buffer to actual length
        Array.Resize(ref ticketBlob, (int)ticketSize);

        // Convert bytes to string
        StringBuilder sb = new StringBuilder();
        foreach (byte b in ticketBlob)
        {
            sb.AppendFormat("{0:x2}", b);
        }
        return sb.ToString();
    }

    // Utility callbacks to log the result
    private void OnComplete(LoginResult obj)
    {
        Debug.Log("Login using Steam was successful!");
    }

    private void OnFailed(PlayFabError error)
    {
        Debug.Log("Login using Steam failed: " + error.GenerateErrorReport());
    }
}
