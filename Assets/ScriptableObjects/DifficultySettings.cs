using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "ScriptableObjects/DifficultySettings", order = 2)]
public class DifficultySettings : ScriptableObject
{
    public float carSpeed = 5f;
    public float enemySpawnRate = 2f;
    public int maxHealth = 3;
}
