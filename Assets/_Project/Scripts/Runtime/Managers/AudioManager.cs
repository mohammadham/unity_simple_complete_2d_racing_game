using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip collisionClip;

    private void Start() {
        // Start playing background music on a loop
        if (musicSource != null && backgroundMusic != null) {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // Subscribe to game over event to play collision sound
        GameManager.Instance.OnGameOver += PlayCollisionSound;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameOver -= PlayCollisionSound;
        }
    }

    public void PlaySoundEffect(AudioClip clip) {
        if (sfxSource != null && clip != null) {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void PlayCollisionSound() {
        PlaySoundEffect(collisionClip);
    }
}
