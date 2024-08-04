using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip _mainMusic;

    private AudioSource _soundAudioSource;
    private AudioSource _musicAudioSource;

    public void PlaySound(AudioClip clip)
    {
        _soundAudioSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _soundAudioSource = gameObject.AddComponent<AudioSource>();
        _musicAudioSource = gameObject.AddComponent<AudioSource>();
        _musicAudioSource.loop = true;
        PlayMusic(_mainMusic);
    }
}
