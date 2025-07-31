using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private RoadConfig[] roadConfigs;
    private int currentRoadType;
    public float Difficulty { get; private set; }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void LoadSceneAsync(string sceneName) {
        // Scene loading with transition queue
    }
}

// Placeholder for RoadConfig ScriptableObject
[CreateAssetMenu(fileName = "New RoadConfig", menuName = "Racing Game/Road Config")]
public class RoadConfig : ScriptableObject {
    public string roadName;
    public Material roadMaterial;
    public GameObject[] obstacles;
}
