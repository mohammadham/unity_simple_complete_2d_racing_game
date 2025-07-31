using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    public void StartGame()
    {
        // Assuming SceneLoader is in the scene
        FindObjectOfType<SceneLoader>().LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
