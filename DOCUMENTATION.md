# راهنمای پیاده‌سازی بازی ۲ بعدی رانندگی در Unity

این راهنما به شما کمک می‌کند تا با استفاده از اسکریپت‌های موجود، بازی را در محیط یونیتی پیاده‌سازی کنید.

## ۱. ساختار پروژه

ابتدا مطمئن شوید که ساختار پوشه‌های پروژه شما به شکل زیر است:

```
Assets/
│
├── Scripts/
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── AudioManager.cs
│   │   ├── MenuManager.cs
│   │   └── RoadManager.cs
│   ├── Player/
│   │   └── PlayerController.cs
│   ├── Obstacles/
│   │   ├── Obstacle.cs
│   │   └── ObstacleSpawner.cs
│
├── Prefabs/
│   ├── Player.prefab
│   ├── Obstacle.prefab
│   └── UI/
│       ├── StartButton.prefab
│       ├── PauseButton.prefab
│       └── RestartButton.prefab
│
├── Scenes/
│   └── MainScene.unity
│
└── Audio/
    └── (فایل‌های صوتی)
```

## ۲. تنظیم صحنه اصلی (Main Scene)

۱. یک صحنه جدید بسازید و آن را با نام `MainScene` در پوشه `Scenes` ذخیره کنید.
۲. یک `GameObject` خالی در صحنه ایجاد کنید و نام آن را به `GameManager` تغییر دهید. اسکریپت `GameManager.cs` را به آن اضافه کنید.
۳. یک `GameObject` خالی دیگر ایجاد کنید و نام آن را `AudioManager` بگذارید. اسکریپت `AudioManager.cs` را به آن اضافه کنید. در این آبجکت، صداهای مورد نیاز بازی (مانند موسیقی پس‌زمینه، صدای برخورد و...) را در آرایه `Sounds` تعریف کنید.
۴. یک `GameObject` خالی دیگر با نام `MenuManager` ایجاد کرده و اسکریپت `MenuManager.cs` را به آن اضافه کنید.

## ۳. ساخت جاده و پس‌زمینه

۱. یک `GameObject` از نوع `Sprite` در صحنه ایجاد کنید و نام آن را `Background` بگذارید.
۲. اسکریپت `RoadManager.cs` را به `Background` اضافه کنید.
۳. یک عکس پس‌زمینه مناسب را به فیلد `Background Sprite` در `RoadManager` اختصاص دهید.
۴. سرعت حرکت پس‌زمینه را از طریق `Scroll Speed` تنظیم کنید.

## ۴. ساخت بازیکن (Player)

۱. یک `GameObject` برای ماشین بازیکن بسازید (می‌توانید از یک `Sprite` استفاده کنید). نام آن را `Player` بگذارید.
۲. اسکریپت `PlayerController.cs` را به `Player` اضافه کنید.
۳. یک `Rigidbody2D` به `Player` اضافه کنید و `Gravity Scale` آن را روی `0` تنظیم کنید.
۴. یک `BoxCollider2D` به `Player` اضافه کنید و گزینه `Is Trigger` را فعال کنید.
۵. تگ `Player` را به این `GameObject` اختصاص دهید.
۶. تنظیمات مربوط به سرعت حرکت (`Move Speed`) و فاصله بین خطوط (`Lane Distance`) را در `PlayerController` تنظیم کنید.
۷. `Player` را به یک `Prefab` تبدیل کرده و در پوشه `Prefabs` ذخیره کنید.

## ۵. ساخت موانع (Obstacles)

۱. یک `GameObject` برای موانع (مثلاً ماشین دشمن) بسازید. نام آن را `Obstacle` بگذارید.
۲. اسکریپت `Obstacle.cs` را به آن اضافه کنید.
۳. یک `Rigidbody2D` با `Gravity Scale` صفر و یک `BoxCollider2D` به آن اضافه کنید.
۴. تگ `Obstacle` را به این `GameObject` اختصاص دهید.
۵. `Obstacle` را به یک `Prefab` تبدیل کرده و در پوشه `Prefabs` ذخیره کنید. می‌توانید چندین نوع مانع با همین روش بسازید.

## ۶. ساخت Obstacle Spawner

۱. یک `GameObject` خالی در بالای صفحه (خارج از دید دوربین) ایجاد کنید و نام آن را `ObstacleSpawner` بگذارید.
۲. اسکریپت `ObstacleSpawner.cs` را به آن اضافه کنید.
۳. `Prefab`های موانعی که ساخته‌اید را به آرایه `Obstacle Prefabs` در `ObstacleSpawner` اضافه کنید.
۴. نرخ تولید موانع (`Spawn Rate`) و محدوده افقی تولید (`Spawn Range X`) را تنظیم کنید.

## ۷. ساخت رابط کاربری (UI)

۱. یک `Canvas` در صحنه ایجاد کنید.
۲. سه دکمه (Button) در داخل `Canvas` بسازید و نام آن‌ها را به `StartButton`، `PauseButton` و `RestartButton` تغییر دهید.
۳. در `MenuManager`، این سه دکمه را به فیلدهای مربوطه (`Start Button`, `Pause Button`, `Restart Button`) اختصاص دهید.
۴. برای هر دکمه، رویداد `OnClick` را تنظیم کنید:
    - **StartButton**: تابع `MenuManager.StartGame()` را فراخوانی کند.
    - **PauseButton**: تابع `MenuManager.PauseGame()` را فراخوانی کند.
    - **RestartButton**: تابع `MenuManager.RestartGame()` را فراخوانی کند.
۵. دکمه‌ها را به `Prefab` تبدیل کرده و در پوشه `Prefabs/UI` ذخیره کنید.

## ۸. اتصال نهایی

۱. مطمئن شوید که در `GameManager`، `MenuManager` و `AudioManager`، اسکریپت‌ها به درستی به `GameObject`های خود متصل شده‌اند و به صورت Singleton عمل می‌کنند.
۲. در `PlayerController`، تگ `Obstacle` برای تشخیص برخورد استفاده می‌شود، پس مطمئن شوید که موانع این تگ را دارند.
۳. در `GameManager`، صداهای "Start" و "GameOver" باید در `AudioManager` تعریف شده باشند تا در زمان شروع و پایان بازی پخش شوند.

با انجام این مراحل، بازی شما آماده اجرا خواهد بود. موفق باشید!
