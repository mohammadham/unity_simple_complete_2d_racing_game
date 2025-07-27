using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("UI Elements")]
    public GameObject startButton;
    public GameObject pauseButton;
    public GameObject restartButton;

    void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        startButton.SetActive(true);
        pauseButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        pauseButton.SetActive(true);
        restartButton.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
