using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LoadingText : MonoBehaviour
{
    [SerializeField] string _baseText = "Loading";
    [SerializeField] float _secondsBetweenDots = 0.5f;

    private int _dotsCount;
    private TMP_Text _loadingText;

    private void Awake()
    {
        _loadingText = GetComponent<TMP_Text>();
        StartCoroutine(AnimateLoadingText());
    }

    private IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            if (_dotsCount > 3)
            {
                _dotsCount = 0;
            }

            _loadingText.text = _baseText + new string('.', _dotsCount);

            _dotsCount++;
            yield return new WaitForSeconds(_secondsBetweenDots);
        }
    }
}
