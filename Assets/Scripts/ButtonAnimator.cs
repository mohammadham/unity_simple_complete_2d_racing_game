using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    public float scaleAmount = 1.1f;
    public float animationSpeed = 2f;

    private Vector3 initialScale;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (button.interactable)
        {
            float scale = 1 + Mathf.PingPong(Time.time * animationSpeed, scaleAmount - 1);
            transform.localScale = initialScale * scale;
        }
    }
}
