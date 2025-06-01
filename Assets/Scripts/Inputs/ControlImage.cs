using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInputs;

[RequireComponent(typeof(Image))]
public class ControlImage : MonoBehaviour
{
    [SerializeField] private Sprite _pcControlSprite;
    [SerializeField] private Sprite _playstationControlSprite;
    [SerializeField] private Sprite _xboxControlSprite;

    private Image _controlImage;

    private void Awake()
    {
        _controlImage = GetComponent<Image>();
        _controlImage.sprite = _pcControlSprite;
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForLocalPlayerPlayerInputs());
    }

    private IEnumerator WaitForLocalPlayerPlayerInputs()
    {
        while (PlayersManager.Instance?.GetLocalPlayer() == null)
            yield return null;

        PlayerInputs localPlayerPlayerInputs = PlayersManager.Instance.GetLocalPlayerPlayerInputs();
        localPlayerPlayerInputs.OnControlSchemeChanged += UpdateControlImage;
        UpdateControlImage(localPlayerPlayerInputs.LastControlSchemeType);
    }

    private void UpdateControlImage(ControlSchemeType scheme)
    {
        switch(scheme)
        {
            case ControlSchemeType.PlayStation:
                _controlImage.sprite = _playstationControlSprite;
                break;
            case ControlSchemeType.Xbox:
                _controlImage.sprite = _xboxControlSprite;
                break;
            default:
                _controlImage.sprite = _pcControlSprite;
                break;
        }
    }
}
