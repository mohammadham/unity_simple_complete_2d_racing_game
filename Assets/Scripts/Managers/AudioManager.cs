using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSettings audioSettings;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        InitializeSources();
    }

    private void InitializeSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.clip = audioSettings.backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = audioSettings.masterVolume;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, audioSettings.masterVolume);
    }

    public void PlayCrash() => PlaySFX(audioSettings.crashSound);
}
