using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startButton;
    public GameObject pauseButton;
    public GameObject gameCanvas;

    void Start()
    {
        HidePauseAndRestartButtons();
    }

    public void StartGame()
    {
        if(startButton != null)
        {
            startButton.SetActive(false);
        }
        if(gameCanvas != null)
        {
            gameCanvas.SetActive(true);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseButton != null)
        {
            pauseButton.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }
    }

    public void ShowRestartButton()
    {
        // Call from GameOverManager
        if (gameCanvas != null)
        {
            gameCanvas.SetActive(false);
        }
        // Display restart panel
    }

    void HidePauseAndRestartButtons()
    {
        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }
    }
}
