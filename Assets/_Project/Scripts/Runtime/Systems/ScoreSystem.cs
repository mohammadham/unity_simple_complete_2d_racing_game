using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    public static readonly string HighScoreKey = "HighScore";

    public float scoreMultiplier = 10f;

    private Transform playerTransform;
    private float initialY;
    private float currentScore;
    private bool isGameOver;

    public float CurrentScore => currentScore;
    public int HighScore { get; private set; }

    private void Start() {
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
        if (playerTransform != null) {
            initialY = playerTransform.position.y;
        }

        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        GameManager.Instance.OnGameOver += HandleGameOver;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnGameOver -= HandleGameOver;
        }
    }

    private void Update() {
        if (isGameOver || playerTransform == null) return;

        float distance = playerTransform.position.y - initialY;
        currentScore = Mathf.Max(0, distance * scoreMultiplier);
    }

    private void HandleGameOver() {
        isGameOver = true;
        if ((int)currentScore > HighScore) {
            HighScore = (int)currentScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }
    }
}
