using UnityEngine;

[CreateAssetMenu(fileName = "RoadSettings", menuName = "Game/Road Settings")]
public class RoadSettings : ScriptableObject
{
    public Sprite backgroundSprite;
    public float scrollSpeed = 2f;
    public int laneCount = 3;
}
