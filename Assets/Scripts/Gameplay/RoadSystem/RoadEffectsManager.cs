using UnityEngine;

public class RoadEffectsManager : MonoBehaviour {
    [System.Serializable]
    public class RoadEffect {
        public RoadType type;
        public ParticleSystem particles;
        public float slipCoefficient;
    }

    public enum RoadType {
        Normal,
        Snow,
        Desert
    }

    [SerializeField] private RoadEffect[] effects;

    public void ApplyEffects(RoadType type) {
        foreach (var effect in effects) {
            if (effect.particles != null) {
                effect.particles.gameObject.SetActive(effect.type == type);
            }
        }
        // You would also apply the slip coefficient to the player's physics here
    }
}
