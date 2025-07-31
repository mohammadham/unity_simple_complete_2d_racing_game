using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
    [SerializeField] private Text finalScoreText;
    [SerializeField] private GameObject gameOverUI;

    void Start() {
        gameOverUI.SetActive(true);
        // Assuming the score is passed via PlayerPrefs or a GameManager
        finalScoreText.text = "Final Score: " + PlayerPrefs.GetInt("LastScore", 0);
    }

    public void RestartGame() {
        SceneManager.LoadScene("Game");
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
