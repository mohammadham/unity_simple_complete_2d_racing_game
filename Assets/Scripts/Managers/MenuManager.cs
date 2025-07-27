using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startButtonPrefab;
    [SerializeField] private GameObject pauseButtonPrefab;
    [SerializeField] private GameObject restartButtonPrefab;
    [SerializeField] private Transform uiContainer;

    private GameObject startButton;
    private GameObject pauseButton;
    private GameObject restartButton;

    private void Start()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        HideAllButtons();
        startButton = Instantiate(startButtonPrefab, uiContainer);
        AnimateButton(startButton);
    }

    public void ShowInGameUI()
    {
        HideAllButtons();
        pauseButton = Instantiate(pauseButtonPrefab, uiContainer);
    }

    public void ShowGameOver()
    {
        HideAllButtons();
        restartButton = Instantiate(restartButtonPrefab, uiContainer);
    }

    private void HideAllButtons()
    {
        if (startButton) Destroy(startButton);
        if (pauseButton) Destroy(pauseButton);
        if (restartButton) Destroy(restartButton);
    }

    private void AnimateButton(GameObject btn)
    {
        // انیمیشن ساده: نوسان یا scale
        StartCoroutine(PulseAnimation(btn.transform));
    }

    private System.Collections.IEnumerator PulseAnimation(Transform t)
    {
        while (true)
        {
            t.localScale = Vector3.Lerp(t.localScale, Vector3.one * 1.1f, 0.1f);
            yield return new WaitForSeconds(0.5f);
            t.localScale = Vector3.Lerp(t.localScale, Vector3.one, 0.1f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
