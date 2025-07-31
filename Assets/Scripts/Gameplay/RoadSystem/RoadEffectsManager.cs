using UnityEngine;

public enum RoadType { Snowy, Desert, Normal }

public class RoadEffectsManager : MonoBehaviour {
    [System.Serializable]
    public class RoadEffect {
        public RoadType type;
        public ParticleSystem particles;
        public float slipCoefficient;
    }

    [SerializeField] private RoadEffect[] effects;

    public void ApplyEffects(RoadType type) {
        foreach (var effect in effects) {
            effect.particles.gameObject.SetActive(effect.type == type);
        }
    }
}
