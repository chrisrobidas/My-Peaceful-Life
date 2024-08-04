using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;

public class DisplayPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;

    private bool _playerNameFetched;

    private void Update()
    {
        if (!_playerNameFetched && PlayFabClientAPI.IsClientLoggedIn())
        {
            _playerNameFetched = true;
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoComplete, OnGetAccountInfoFailed);
        }

        _playerNameText.transform.LookAt(_playerNameText.transform.position + Camera.main.transform.forward, Camera.main.transform.up);
    }

    private void OnGetAccountInfoComplete(GetAccountInfoResult result)
    {
        string playFabDisplayName = result.AccountInfo.TitleInfo.DisplayName;
        _playerNameText.text = playFabDisplayName;
        Debug.Log("Successfully fetched PlayFab display name: " + playFabDisplayName);
    }

    private void OnGetAccountInfoFailed(PlayFabError error)
    {
        Debug.Log("Failed to fetch PlayFab display name: " + error.GenerateErrorReport());
    }
}
