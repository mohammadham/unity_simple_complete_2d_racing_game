using UnityEngine;

public enum GameState { Menu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Difficulty difficulty;

    private GameState currentState = GameState.Menu;

    private void Start()
    {
        menuManager.ShowStartMenu();
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        menuManager.ShowInGameUI();
        enemySpawner.spawnInterval /= difficulty.GetSpawnRateMultiplier();
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        menuManager.ShowGameOver();
    }

    public void RestartGame()
    {
        // Reset تمام اشیا
        foreach (var enemy in FindObjectsOfType<EnemySpawner>())
            Destroy(enemy.gameObject);

        // یا بهتر: از Scene Management استفاده کنید
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }
}
