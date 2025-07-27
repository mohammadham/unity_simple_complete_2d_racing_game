using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "Game/Difficulty")]
public class Difficulty : ScriptableObject
{
    public string difficultyName;
    [Range(0.5f, 2f)] public float speedMultiplier = 1f;
    [Range(0.5f, 3f)] public float spawnRateMultiplier = 1f;

    public float GetSpeedMultiplier() => speedMultiplier;
    public float GetSpawnRateMultiplier() => spawnRateMultiplier;
}
