using UnityEngine;

public class RoadEffectsManager : MonoBehaviour {
    // This manager is responsible for handling visual and physical effects related to the road.
    // This helps to centralize environment-specific logic.

    // Key responsibilities would include:
    // 1.  **Particle Effects:** Spawning and managing particles like snow, rain, or dust based
    //     on the current road type (e.g., from a RoadData ScriptableObject).
    // 2.  **Surface Properties:** Modifying the player's physics based on the surface. For example,
    //     an "ice" road type could reduce the player's grip or traction.

    private PlayerController playerController;
    private ParticleSystem currentParticleSystem;

    private void Start() {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Example of a function that could be called when the road type changes:
    /*
    public void ApplyRoadData(RoadData data) {
        if (playerController != null) {
            // The slipCoefficient from the PlayerController could be modified by the road's slipFactor.
            // playerController.slipCoefficient = baseSlipCoefficient * data.slipFactor;
        }

        // Stop and clear any existing particles.
        if (currentParticleSystem != null) {
            Destroy(currentParticleSystem.gameObject);
        }

        // Instantiate and play new particles from the RoadData.
        if (data.particlePrefab != null) {
            currentParticleSystem = Instantiate(data.particlePrefab, transform);
            currentParticleSystem.Play();
        }
    }
    */

    // Full implementation requires specific particle prefabs and RoadData assets to be created
    // in the Unity Editor.
}
