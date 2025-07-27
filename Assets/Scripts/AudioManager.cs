using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource soundEffectsSource;

    [Header("Audio Clips")]
    public AudioClip[] backgroundMusicClips;
    public AudioClip carCrashSound;

    private void Start()
    {
        // Play a random background music
        if (backgroundMusicSource != null && backgroundMusicClips.Length > 0)
        {
            backgroundMusicSource.clip = backgroundMusicClips[Random.Range(0, backgroundMusicClips.Length)];
            backgroundMusicSource.Play();
        }
    }

    public void PlayCarCrashSound()
    {
        if (soundEffectsSource != null && carCrashSound != null)
        {
            soundEffectsSource.PlayOneShot(carCrashSound);
        }
    }
}
