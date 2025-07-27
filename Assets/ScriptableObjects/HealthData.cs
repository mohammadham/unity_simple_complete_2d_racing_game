using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthData", menuName = "ScriptableObjects/HealthData", order = 1)]
public class HealthData : ScriptableObject
{
    public int maxHealth = 3;
}
