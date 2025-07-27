# Traffic Rush - 2D Mobile Game

## 📌 0. Table of Contents
1.  [Project Overview](#1-project-overview)
2.  [Folder and File Structure](#2-folder-and-file-structure)
3.  [Complete List of Scripts and Their Responsibilities](#3-complete-list-of-scripts-and-their-responsibilities)
4.  [Step-by-Step Implementation (Copy-Paste Ready)](#4-step-by-step-implementation-copy-paste-ready)
5.  [Setup in Unity Editor](#5-setup-in-unity-editor)
6.  [Optimization and Android Testing](#6-optimization-and-android-testing)
7.  [Post-Launch Development](#7-post-launch-development)
8.  [Maintenance and Team Development Tips](#8-maintenance-and-team-development-tips)

---

## 1. Project Overview
**Game Name:** Traffic Rush
**Genre:** Endless 2D Top-Down Racer
**Combined Features from Two Scenarios:**
- Procedural road generation using a simple coded algorithm in Unity.
- Multi-layer parallax, weather particles (snow, dust), slippery surfaces.
- Touch control (Swipe) + Haptic feedback (Vibration).
- Smart traffic with Object-Pooling, dynamic difficulty based on distance.
- System-Health, Near-Miss, Screen-Shake, dynamic music change.
- Rewarded ads, daily challenges, mod support.
- Optimized for Snapdragon 400 to 800 and at least Mali-T GPU.

---

## 2. Folder and File Structure (Assets)
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

## 3. Complete List of Scripts and Their Responsibilities
| File Path | Class / Struct | Main Responsibility |
|-----------|---------------|--------------|
| Managers/GameManager.cs | Singleton | Scene changes, saving RoadType, FPS Tracking |
| Managers/RoadManager.cs | Endless road + Parallax + Segment Spawn |
| Managers/AudioManager.cs | Dynamic music + SFX + Mixer snapshot |
| Managers/VibrationManager.cs | Haptic feedback |
| Systems/ResourceLoader.cs | Threaded Sprite Load (Addressables) |
| Systems/RoadEffectsManager.cs | Particle & Surface Coefficient |
| Systems/TrafficSystem.cs | ObjectPool, Spawn, Density Logic |
| Systems/HealthSystem.cs | TakeDamage + Event |
| Systems/ScoreSystem.cs | Distance + NearMiss + Session Best |
| Player/PlayerController.cs | Swipe Movement + Drift Physics |
| Player/CollisionHandler.cs | LayerMask collision |
| Traffic/EnemyCar.cs | Straight downward movement |
| UI/UIManager.cs | Start/Pause/Resume/GameOver Panel |
| UI/GameOverManager.cs | Restart + AdReward |
| Utils/Singleton.cs | Generic Singleton |
| Utils/ScriptableObject/… | RoadData, Difficulty, DailyChallenge |

---

## 4. Step-by-Step Implementation (Copy-Paste Ready)

### 4.1 Core Singleton
```csharp
// Utils/Singleton.cs
public class Singleton<T> : MonoBehaviour where T : Component {
    public static T Instance { get; private set; }
    protected virtual void Awake() {
        if (Instance == null) { Instance = this as T; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
}
```

### 4.2 GameManager
```csharp
// Managers/GameManager.cs
public class GameManager : Singleton<GameManager> {
    public string currentRoadType = "asphalt";
    public event System.Action OnGameOver;
    public void TriggerGameOver() => OnGameOver?.Invoke();
}
```

### 4.3 RoadManager (Procedural + Parallax)
```csharp
// Managers/RoadManager.cs
using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour {
    [Header("Segment")]
    public GameObject segmentPrefab;
    public int segmentHeight = 1920;
    public int poolSize = 5;

    [Header("Parallax")]
    public Transform[] parallaxLayers; // 3 Layers
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

### 4.4 PlayerController
```csharp
// Player/PlayerController.cs
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

### 4.5 TrafficSystem + Object Pool
```csharp
// Systems/TrafficSystem.cs
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
```

### 4.6 Vibration (Android)
```csharp
// Managers/VibrationManager.cs
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

### 4.7 Sample ScriptableObject
```csharp
// ScriptableObjects/RoadData.cs
[CreateAssetMenu(menuName = "TrafficRush/RoadData")]
public class RoadData : ScriptableObject {
    public string typeName;           // snowy, desert, …
    public float slipFactor = 1f;
    public ParticleSystem particlePrefab;
    public AudioClip ambientSFX;
}
```

---

## 5. Setup in Unity

### 5.1 Scene Setup
1.  New Scene: `MainGame`
2.  Main Camera: Orthographic, Size = 5
3.  Empty GameObject: `GameManager` + `GameManager.cs`, `AudioManager.cs`, `RoadManager.cs`, `TrafficSystem.cs`
4.  Canvas → `UIManager`
5.  Player prefab: SpriteRenderer + Rigidbody2D (gravity 0) + BoxCollider2D + PlayerController + CollisionHandler + HealthSystem
6.  Enemy prefab: SpriteRenderer + Rigidbody2D (gravity 0) + BoxCollider2D + tag = Enemy
7.  3 Background Quads → Material Unlit/Texture → Assign to ParallaxLayers.

### 5.2 Physics Layers
-   Player: Layer 8
-   Enemy: Layer 9
-   Wall/Guardrail: Layer 10
Collision Matrix: Only 8↔9 and 8↔10 are active.

### 5.3 Build Settings
-   Platform: Android
-   Min API 21
-   Scripting Backend: IL2CPP
-   Target Architectures: ARM64
-   Compression: LZ4HC
-   .NET Standard 2.1

---

## 6. Optimization and Testing

| Tool | Purpose |
|-------|-----|
| Unity Profiler | FrameTime < 16 ms |
| Addressables | Async Textures/Audio |
| TextureImporter | Override for Android → ETC2_RGBA8, Max 1024 |
| GPU Instancing | For particles and enemies |
| Mobile Fidelity | URP 2D Renderer + Use 2D Lights |

### 6.1 Test Devices
-   Xiaomi Redmi 9A (SD 439)
-   Samsung A52 (SD 720G)
-   Pixel 6 (SD 888)

---

## 7. Post-Launch

### 7.1 Daily Challenge
```csharp
// DailyChallenge.cs
[CreateAssetMenu(menuName="TrafficRush/DailyChallenge")]
public class DailyChallenge : ScriptableObject {
    public string title;
    public System.Func<bool> condition; // e.g. survive 120s
    public int rewardCoins;
}
```

### 7.2 AdMob Rewarded
-   Google Mobile SDK → `AdManager.cs`
```csharp
public void ShowRewarded(System.Action onReward) {
    if (rewardedAd.IsLoaded()) rewardedAd.Show(userEarnedRewardEvent: onReward);
}
```

### 7.3 Mod Support
-   The `StreamingAssets/modding` folder is read.
-   `RoadPack.json` → New RoadData is loaded.

---

## 8. Maintenance Tips
-   Use Git + LFS for large files.
-   Use `asmdef` for each script group to reduce compile time.
-   Weekly Code-Review sessions.
-   Unit Test for score and health systems (Unity Test Runner).

---

## 📦 Attached Files
1.  `CHANGELOG.md` (each commit + explanation)
2.  `README.md` (quick installation and setup)
3.  `Docs/SetupAndroid.md` (Build Settings + Keystore)
4.  `Docs/ModdingGuide.md` (JSON format + Sprite Naming)
