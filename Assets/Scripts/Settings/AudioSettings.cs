using UnityEngine;

[CreateAssetMenu(fileName = "AudioSettings", menuName = "Game/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public AudioClip backgroundMusic;
    public AudioClip crashSound;
    public AudioClip buttonClick;
    [Range(0, 1)] public float masterVolume = 1f;
}
