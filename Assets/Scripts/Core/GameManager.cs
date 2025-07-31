public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private RoadConfig[] roadConfigs;
    private int currentRoadType;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsync(string sceneName) {
        // Scene loading with transition queue
    }
}
