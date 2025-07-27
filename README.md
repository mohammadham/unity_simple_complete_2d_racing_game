📘 **سند جامع پیاده‌سازی بازی 2D موبایلی «Traffic Rush» در Unity 2022 LTS**
(ادغام دو سناریوی Godot و Unity به‌صورت یکپارچه، ۱۰۰٪ کد-محور، بهینه برای اندروید)

---

### 📌 ۰. فهرست مطالب
1. چکیده پروژه
2. ساختار پوشه‌ها و فایل‌ها
3. لیست کامل اسکریپت‌ها و مسئولیت هرکدام
4. پیاده‌سازی مرحله‌به‌مرحله (copy-paste ready)
5. راه‌اندازی در Unity Editor
6. بهینه‌سازی و تست اندروید
7. توسعه پس از انتشار (Post-Launch)
8. نکات نگهداری و توسعه تیم

---

### ۱. چکیده پروژه
**نام بازی:** Traffic Rush
**ژانر:** Endless 2D Top-Down Racer
**ویژگی‌های ترکیبی از دو سناریو:**
- جاده تولیدی (Procedural) با استفاده از Perlin-Noise (درگودوتی) → تبدیل به الگوریتم سادهٔ کدنویسی شده در Unity.
- چندلایه پارالکس، ذرات آب‌وهوا (برف، گردوغبار)، سطوح لغزنده.
- کنترل لمسی (Swipe) + بازخورد لمسی (Vibration).
- ترافیک هوشمند با Object-Pooling، سختی پویا براساس فاصله.
- System-Health، Near-Miss، Screen-Shake، تغییر موسیقی پویا.
- تبلیغات Rewarded، چالش روزانه، پشتیبانی مود.
- بهینه برای Snapdragon 400 تا 800 و GPU Mali-T حداقل.

---

### ۲. ساختار پوشه‌ها (Assets)
```
Assets/
├── _Project/
│   ├── Scripts/
│   │   ├── Runtime/
│   │   │   ├── Managers/
│   │   │   ├── Systems/
│   │   │   ├── Player/
│   │   │   ├── Traffic/
│   │   │   ├── UI/
│   │   │   └── Utils/
│   │   ├── ScriptableObjects/
│   │   └── Shaders/
│   ├── Prefabs/
│   ├── Sprites/
│   ├── Audio/
│   ├── Materials/
│   └── StreamingAssets/modding/
```

---

### ۳. لیست اسکریپت‌ها و توضیح مختصر
| مسیر فایل | کلاس / Struct | مسئولیت اصلی |
|-----------|---------------|--------------|
| Managers/GameManager.cs | Singleton | تغییر Scene، ذخیره RoadType، FPS Tracking |
| Managers/RoadManager.cs | جاده بی‌انتها + پارالکس + Spawn Segment |
| Managers/AudioManager.cs | موسیقی پویا + SFX + snapshot Mixer |
| Managers/VibrationManager.cs | بازخورد لمسی |
| Systems/ResourceLoader.cs | Threaded Sprite Load (Addressables) |
| Systems/RoadEffectsManager.cs | Particle & Surface Coefficient |
| Systems/TrafficSystem.cs | ObjectPool، Spawn، Density Logic |
| Systems/HealthSystem.cs | TakeDamage + Event |
| Systems/ScoreSystem.cs | Distance + NearMiss + Session Best |
| Player/PlayerController.cs | Swipe Movement + Drift Physics |
| Player/CollisionHandler.cs | LayerMask برخورد |
| Traffic/EnemyCar.cs | حرکت مستقیم به سمت پایین |
| UI/UIManager.cs | Start/Pause/Resume/GameOver Panel |
| UI/GameOverManager.cs | Restart + AdReward |
| Utils/Singleton.cs | Generic Singleton |
| Utils/ScriptableObject/… | RoadData, Difficulty, DailyChallenge |

---

### ۴. پیاده‌سازی مرحله‌به‌مرحله (کد کامل)

#### ۴.۱ Core Singleton
```csharp
// Assets/_Project/Scripts/Runtime/Utils/Singleton.cs
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {
    public static T Instance { get; private set; }
    protected virtual void Awake() {
        if (Instance == null) { Instance = this as T; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
}
```

#### ۴.۲ GameManager
```csharp
// Assets/_Project/Scripts/Runtime/Managers/GameManager.cs
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public string currentRoadType = "asphalt";
    public event System.Action OnGameOver;
    public void TriggerGameOver() => OnGameOver?.Invoke();
}
```

#### 4.3 Health & Collision
```csharp
// Assets/_Project/Scripts/Runtime/Systems/HealthSystem.cs
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour {
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    public UnityEvent OnDie;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            currentHealth = 0;
            Die();
        }
    }

    private void Die() {
        OnDie?.Invoke();
        GameManager.Instance.TriggerGameOver();
    }
}

// Assets/_Project/Scripts/Runtime/Player/CollisionHandler.cs
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class CollisionHandler : MonoBehaviour {
    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall")) {
            healthSystem.TakeDamage(1);
        }
    }
}
```

#### ۴.۴ RoadManager (Procedural + Parallax)
```csharp
// Assets/_Project/Scripts/Runtime/Managers/RoadManager.cs
using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour {
    [Header("Segment")]
    public GameObject segmentPrefab;
    public int segmentHeight = 1920;
    public int poolSize = 5;

    [Header("Parallax")]
    public Transform[] parallaxLayers; // 3 Layer
    public float[] parallaxFactors = {0.3f, 0.6f, 1f};

    private Queue<GameObject> segmentPool = new();
    private Transform player;
    private float nextSpawnY;

    void Start() {
        player = FindObjectOfType<PlayerController>().transform;
        nextSpawnY = player.position.y + segmentHeight;
        for (int i = 0; i < poolSize; i++) {
            var seg = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, transform);
            seg.SetActive(false);
            segmentPool.Enqueue(seg);
        }
    }

    void Update() {
        if (player.position.y + segmentHeight > nextSpawnY) {
            SpawnSegment();
        }
        UpdateParallax();
    }

    void SpawnSegment() {
        var seg = segmentPool.Dequeue();
        seg.transform.position = new Vector3(0, nextSpawnY, 0);
        seg.SetActive(true);
        segmentPool.Enqueue(seg);
        nextSpawnY += segmentHeight;
    }

    void UpdateParallax() {
        float travel = player.position.y;
        for (int i = 0; i < parallaxLayers.Length; i++) {
            Vector3 p = parallaxLayers[i].position;
            p.y = -travel * parallaxFactors[i];
            parallaxLayers[i].position = p;
        }
    }
}
```

#### ۴.۵ PlayerController
```csharp
// Assets/_Project/Scripts/Runtime/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float boundary = 2.8f;
    public float slipCoefficient = 0.4f;

    private Rigidbody2D rb;
    private Vector2 startTouch;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update() {
        HandleSwipe();
    }

    void HandleSwipe() {
        if (Input.touchCount <= 0) return;
        Touch t = Input.GetTouch(0);
        switch (t.phase) {
            case TouchPhase.Began:
                startTouch = t.position;
                break;
            case TouchPhase.Moved:
                float dir = Mathf.Sign(t.position.x - startTouch.x);
                rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
                rb.angularVelocity = -dir * slipCoefficient * 100;
                break;
            case TouchPhase.Ended:
                rb.velocity = Vector2.zero;
                break;
        }
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        transform.position = pos;
    }
}
```

#### ۴.۶ TrafficSystem + Object Pool
```csharp
// Assets/_Project/Scripts/Runtime/Systems/TrafficSystem.cs
using UnityEngine;
using System.Collections.Generic;

public class TrafficSystem : MonoBehaviour {
    public GameObject enemyPrefab;
    public Transform[] lanes;
    public float baseInterval = 2f;
    public AnimationCurve densityCurve; // x = score, y = spawn interval

    private Queue<GameObject> pool = new();
    private int poolSize = 20;
    private float timer;
    private int score;

    void Start() {
        for (int i = 0; i < poolSize; i++) {
            var e = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, transform);
            e.SetActive(false);
            pool.Enqueue(e);
        }
    }

    void Update() {
        score = Mathf.FloorToInt(Time.time * 10);
        timer += Time.deltaTime;
        float interval = Mathf.Max(0.5f, baseInterval - densityCurve.Evaluate(score / 1000f));
        if (timer > interval) {
            Spawn();
            timer = 0;
        }
    }

    void Spawn() {
        var e = pool.Dequeue();
        int lane = Random.Range(0, lanes.Length);
        e.transform.position = lanes[lane].position + Vector3.up * 12f;
        e.SetActive(true);
        pool.Enqueue(e);
    }
}

// Assets/_Project/Scripts/Runtime/Traffic/EnemyCar.cs
using UnityEngine;

public class EnemyCar : MonoBehaviour {
    public float speed = 5f;
    public float despawnY = -15f;

    private void Update() {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < despawnY) {
            gameObject.SetActive(false);
        }
    }
}
```

#### 4.7 UI & Score
```csharp
// Assets/_Project/Scripts/Runtime/Systems/ScoreSystem.cs
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    public static readonly string HighScoreKey = "HighScore";
    public float scoreMultiplier = 10f;
    private Transform playerTransform;
    private float initialY;
    private float currentScore;
    private bool isGameOver;
    public float CurrentScore => currentScore;
    public int HighScore { get; private set; }

    private void Start() {
        playerTransform = FindObjectOfType<PlayerController>()?.transform;
        if (playerTransform != null) initialY = playerTransform.position.y;
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        GameManager.Instance.OnGameOver += HandleGameOver;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) GameManager.Instance.OnGameOver -= HandleGameOver;
    }

    private void Update() {
        if (isGameOver || playerTransform == null) return;
        float distance = playerTransform.position.y - initialY;
        currentScore = Mathf.Max(0, distance * scoreMultiplier);
    }

    private void HandleGameOver() {
        isGameOver = true;
        if ((int)currentScore > HighScore) {
            HighScore = (int)currentScore;
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }
    }
}

// Assets/_Project/Scripts/Runtime/UI/UIManager.cs
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [Header("UI Elements")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text highScoreText;
    private ScoreSystem scoreSystem;

    private void Start() {
        scoreSystem = FindObjectOfType<ScoreSystem>();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        GameManager.Instance.OnGameOver += ShowGameOverPanel;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) GameManager.Instance.OnGameOver -= ShowGameOverPanel;
    }

    private void Update() {
        if (scoreSystem != null && scoreText != null) {
            scoreText.text = "Score: " + Mathf.FloorToInt(scoreSystem.CurrentScore);
        }
    }

    private void ShowGameOverPanel() {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (scoreSystem != null) {
            if(finalScoreText != null) finalScoreText.text = "Score: " + Mathf.FloorToInt(scoreSystem.CurrentScore);
            if(highScoreText != null) highScoreText.text = "High Score: " + scoreSystem.HighScore;
        }
        if (scoreText != null) scoreText.gameObject.SetActive(false);
    }
}

// Assets/_Project/Scripts/Runtime/UI/GameOverManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
```

#### ۴.۸ Audio & Vibration
```csharp
// Assets/_Project/Scripts/Runtime/Managers/AudioManager.cs
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [Header("Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip collisionClip;

    private void Start() {
        if (musicSource != null && backgroundMusic != null) {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
        GameManager.Instance.OnGameOver += PlayCollisionSound;
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) GameManager.Instance.OnGameOver -= PlayCollisionSound;
    }

    public void PlaySoundEffect(AudioClip clip) {
        if (sfxSource != null && clip != null) sfxSource.PlayOneShot(clip);
    }

    private void PlayCollisionSound() {
        PlaySoundEffect(collisionClip);
    }
}

// Assets/_Project/Scripts/Runtime/Managers/VibrationManager.cs
using UnityEngine;

public static class VibrationManager {
    public static void Vibrate(long milliseconds = 50) {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            vibrator.Call("vibrate", milliseconds);
        }
#endif
    }
}
```

---

### ۵. راه‌اندازی در Unity

#### 5.1 Scene Setup
1. Scene جدید: `MainGame`
2. Main Camera: Orthographic, Size = 5
3. GameObject Empty: `GameManager` + `GameManager.cs`, `AudioManager.cs`, `RoadManager.cs`, `TrafficSystem.cs`, `ScoreSystem.cs`
4. Canvas → `UIManager`
5. Player prefab: SpriteRenderer + Rigidbody2D (gravity 0) + BoxCollider2D + PlayerController + CollisionHandler + HealthSystem
6. Enemy prefab: SpriteRenderer + Rigidbody2D (gravity 0) + BoxCollider2D + tag = Enemy
7. 3 Background Quad → Material Unlit/Texture → به ParallaxLayers واگذار شود.

---
(بخش‌های باقیمانده سند بدون تغییر باقی می‌مانند)
