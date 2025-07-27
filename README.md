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

#### ۴.۳ RoadManager (Procedural + Parallax)
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

#### ۴.۴ PlayerController
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

#### ۴.۵ TrafficSystem + Object Pool
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
```

#### ۴.۶ Vibration (Android)
```csharp
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

#### ۴.۷ ScriptableObject نمونه
```csharp
// Assets/_Project/Scripts/ScriptableObjects/RoadData.cs
using UnityEngine;

[CreateAssetMenu(menuName = "TrafficRush/RoadData")]
public class RoadData : ScriptableObject {
    public string typeName;           // snowy, desert, …
    public float slipFactor = 1f;
    public ParticleSystem particlePrefab;
    public AudioClip ambientSFX;
}
```

---

### ۵. راه‌اندازی در Unity

#### 5.1 Scene Setup
1. Scene جدید: `MainGame`
2. Main Camera: Orthographic, Size = 5
3. GameObject Empty: `GameManager` + `GameManager.cs`, `AudioManager.cs`, `RoadManager.cs`, `TrafficSystem.cs`
4. Canvas → `UIManager`
5. Player prefab: SpriteRenderer + Rigidbody2D (gravity 0) + BoxCollider2D + PlayerController + CollisionHandler + HealthSystem
6. Enemy prefab: SpriteRenderer + Rigidbody2D (gravity 0) + BoxCollider2D + tag = Enemy
7. 3 Background Quad → Material Unlit/Texture → به ParallaxLayers واگذار شود.

#### 5.2 Physics Layers
- Player: Layer 8
- Enemy: Layer 9
- Wall/Guardrail: Layer 10
Collision Matrix: فقط 8↔9 و 8↔10 فعال.

#### 5.3 Build Settings
- Platform: Android
- Min API 21
- Scripting Backend: IL2CPP
- Target Architectures: ARM64
- Compression: LZ4HC
- .NET Standard 2.1

---

### ۶. بهینه‌سازی و تست

| ابزار | هدف |
|-------|-----|
| Unity Profiler | FrameTime < 16 ms |
| Addressables | Textures/Audio async |
| TextureImporter | Override for Android → ETC2_RGBA8, Max 1024 |
| GPU Instancing | برای ذرات و دشمنان |
| Mobile Fidelity | URP 2D Renderer + Use 2D Lights |

### ۶.۱ تست دستگاه‌ها
- Xiaomi Redmi 9A (SD 439)
- Samsung A52 (SD 720G)
- Pixel 6 (SD 888)

---

### ۷. Post-Launch

#### ۷.۱ چالش روزانه
```csharp
// DailyChallenge.cs
[CreateAssetMenu(menuName="TrafficRush/DailyChallenge")]
public class DailyChallenge : ScriptableObject {
    public string title;
    public System.Func<bool> condition; // e.g. survive 120s
    public int rewardCoins;
}
```

#### ۷.۲ AdMob Rewarded
- Google Mobile SDK → `AdManager.cs`
```csharp
public void ShowRewarded(System.Action onReward) {
    if (rewardedAd.IsLoaded()) rewardedAd.Show(userEarnedRewardEvent: onReward);
}
```

#### ۷.۳ پشتیبانی مود
- پوشه `StreamingAssets/modding` خوانده می‌شود.
- RoadPack.json → RoadData جدید لود می‌شود.

---

### ۸. نکات نگهداری
- از Git + LFS برای فایل‌های حجیم استفاده شود.
- `asmdef` برای هر گروه اسکریپت جهت کاهش زمان کامپایل.
- جلسه Code-Review هفتگی.
- Unit Test برای سیستم امتیاز و Health (Unity Test Runner).

---

### 📦 فایل‌های ضمیمه
1. `CHANGELOG.md` (هر کامیت + توضیح)
2. `README.md` (نصب و راه‌اندازی سریع)
3. `Docs/SetupAndroid.md` (Build Settings + Keystore)
4. `Docs/ModdingGuide.md` (فرمت JSON + Sprite Naming)
