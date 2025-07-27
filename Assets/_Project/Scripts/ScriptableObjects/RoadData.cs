// ScriptableObjects/RoadData.cs
using UnityEngine;

[CreateAssetMenu(menuName = "TrafficRush/RoadData")]
public class RoadData : ScriptableObject {
    public string typeName;           // snowy, desert, …
    public float slipFactor = 1f;
    public ParticleSystem particlePrefab;
    public AudioClip ambientSFX;
}
