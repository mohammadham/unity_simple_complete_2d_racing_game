using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text speedText;
    private float score;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Update() {
        // Example score calculation
        score += Time.deltaTime * 10;
        scoreText.text = "Score: " + (int)score;

        // Example speed display
        // You'd get this value from the Player's Rigidbody or controller
        float speed = 150f;
        speedText.text = "Speed: " + (int)speed + " KPH";
    }
}
