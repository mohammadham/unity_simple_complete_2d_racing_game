using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    // This function can be assigned to a UI Button's OnClick event in the Inspector.
    public void RestartGame() {
        // Reload the currently active scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // This function is for a potential "Rewarded Ad" button.
    public void ContinueWithAd() {
        // Placeholder for ad logic.
        Debug.Log("Show rewarded ad to continue...");
        // In a real implementation, you would call your AdManager here.
        // If the ad is successful, you would revive the player instead of restarting.
    }
}
