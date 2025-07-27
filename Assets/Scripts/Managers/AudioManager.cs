using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Ambient Sounds")]
    public AudioClip backgroundMusic;
    public AudioClip crashSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        PlayBackgroundMusic();
    }

    void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayCrashSound()
    {
        if (crashSound != null)
        {
            audioSource.PlayOneShot(crashSound);
        }
    }
}
