using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame()
    {
        isGameActive = true;
        Time.timeScale = 1f;
        AudioManager.Instance?.PlaySound("Start");
    }

    public void PauseGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        MenuManager.Instance.restartButton.SetActive(true);
        AudioManager.Instance?.PlaySound("GameOver");
    }
}
