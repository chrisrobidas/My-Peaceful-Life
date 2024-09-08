using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeOutPanel : MonoBehaviour
{
    [SerializeField] private float _initialWaitDuration = 0.5f;
    [SerializeField] private float _fadeOutDuration = 2.0f;

    private Image _blackPanelImage;

    private void Awake()
    {
        _blackPanelImage = GetComponent<Image>();
        _blackPanelImage.enabled = true;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(_initialWaitDuration);

        Color imageColor = _blackPanelImage.color;

        // Loop over the fade duration
        for (float elapsedTime = 0; elapsedTime < _fadeOutDuration; elapsedTime += Time.deltaTime)
        {
            // Calculate the percentage of time passed
            float normalizedTime = elapsedTime / _fadeOutDuration;

            // Set the alpha value based on the time passed
            imageColor.a = Mathf.Lerp(1f, 0f, normalizedTime);
            _blackPanelImage.color = imageColor;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the alpha is fully set to 0 after the fade
        imageColor.a = 0f;
        _blackPanelImage.color = imageColor;

        _blackPanelImage.enabled = false;
    }
}
