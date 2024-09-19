using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using Fusion;

public class DisplayPlayerName : NetworkBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;

    private bool _playerNameFetched;

    [Networked]
    private string _playerName { get; set; }

    private void Awake()
    {
        _playerNameText.text = null;
    }

    private void LateUpdate()
    {
        // We don't want players to see their own name
        if (HasStateAuthority && !_playerNameFetched && PlayFabClientAPI.IsClientLoggedIn())
        {
            _playerNameFetched = true;
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoComplete, OnGetAccountInfoFailed);
        }

        if (!HasStateAuthority && _playerNameText.text != _playerName)
        {
            _playerNameText.text = _playerName;
        }

        _playerNameText.transform.LookAt(_playerNameText.transform.position + Camera.main.transform.forward, Camera.main.transform.up);
    }

    private void OnGetAccountInfoComplete(GetAccountInfoResult result)
    {
        _playerName = result.AccountInfo.TitleInfo.DisplayName;
        Debug.Log("Successfully fetched PlayFab display name: " + _playerName);
    }

    private void OnGetAccountInfoFailed(PlayFabError error)
    {
        Debug.Log("Failed to fetch PlayFab display name: " + error.GenerateErrorReport());
    }
}
