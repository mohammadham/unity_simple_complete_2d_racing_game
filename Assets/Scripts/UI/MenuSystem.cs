using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings() {
        // Logic to open a settings panel
        Debug.Log("Settings opened.");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
