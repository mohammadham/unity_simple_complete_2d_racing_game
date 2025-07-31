using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
