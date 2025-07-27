using UnityEngine;
using UnityEngine.UI; // For Text or TextMeshProUGUI

public class UIManager : MonoBehaviour {
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI Elements")]
    [SerializeField] private Text scoreText; // Or TMPro.TextMeshProUGUI
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text highScoreText;

    private ScoreSystem scoreSystem;

    private void Start() {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        if (gameOverPanel != null) {
            gameOverPanel.SetActive(false);
        }
        GameManager.Instance.OnGameOver += ShowGameOverPanel;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameOver -= ShowGameOverPanel;
        }
    }

    private void Update() {
        if (scoreSystem != null && scoreText != null) {
            scoreText.text = "Score: " + Mathf.FloorToInt(scoreSystem.CurrentScore);
        }
    }

    private void ShowGameOverPanel() {
        if (gameOverPanel != null) {
            gameOverPanel.SetActive(true);
        }

        if (scoreSystem != null) {
            if(finalScoreText != null) finalScoreText.text = "Score: " + Mathf.FloorToInt(scoreSystem.CurrentScore);
            if(highScoreText != null) highScoreText.text = "High Score: " + scoreSystem.HighScore;
        }

        if (scoreText != null) {
            scoreText.gameObject.SetActive(false);
        }
    }
}
