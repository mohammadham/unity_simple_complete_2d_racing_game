# راهنمای پیاده‌سازی بازی مسابقه‌ای دو بعدی در یونیتی

این راهنما شما را در فرآیند ساخت یک بازی مسابقه‌ای دو بعدی ساده با استفاده از اسکریپت‌های ارائه شده راهنمایی می‌کند.

## ۱. راه‌اندازی پروژه

*   یک پروژه جدید دو بعدی در Unity Hub ایجاد کنید.
*   پوشه‌های زیر را در پنجره `Project` یونیتی بسازید:
    *   `Assets/Scripts`
    *   `Assets/Sprites`
    *   `Assets/Prefabs`
    *   `Assets/Scenes`
    *   `Assets/Animations`

## ۲. وارد کردن اسکریپت‌ها

*   تمام اسکریپت‌های C# (`RoadManager.cs`, `AudioManager.cs`, `Car.cs`, `GameActionPlayer.cs`, `MenuManager.cs`, `EnemyCar.cs`, `ButtonAnimator.cs`) را که ایجاد کردم، به پوشه `Assets/Scripts` بکشید و رها کنید (Drag & Drop).

## ۳. ساخت صحنه اصلی بازی (Game Scene)

*   یک صحنه جدید بسازید (`File -> New Scene`) و آن را با نام `GameScene` در پوشه `Assets/Scenes` ذخیره کنید.

## ۴. تنظیم RoadManager

*   یک `GameObject` خالی در صحنه بسازید (`GameObject -> Create Empty`) و نام آن را به `RoadManager` تغییر دهید.
*   اسکریپت `RoadManager.cs` را به `GameObject` مربوط به `RoadManager` اضافه کنید.
*   در پنجره `Inspector` برای `RoadManager`:
    *   **Road Prefab**: یک `Sprite` برای جاده در پوشه `Assets/Sprites` قرار دهید و آن را به یک `Prefab` تبدیل کنید (با کشیدن از پوشه `Sprites` به پوشه `Prefabs`). سپس این `Prefab` را به فیلد `Road Prefab` در `RoadManager` اختصاص دهید.
    *   **Background**: یک `GameObject` از نوع `Sprite Renderer` برای پس‌زمینه بسازید و آن را به فیلد `Background` در `RoadManager` بکشید.
    *   **Background Sprites**: چندین `Sprite` برای پس‌زمینه در پوشه `Assets/Sprites` قرار دهید و آن‌ها را به آرایه `Background Sprites` در `RoadManager` اضافه کنید.

## ۵. تنظیم AudioManager

*   یک `GameObject` خالی بسازید و نام آن را به `AudioManager` تغییر دهید.
*   اسکریپت `AudioManager.cs` را به آن اضافه کنید.
*   دو `GameObject` فرزند برای `AudioManager` بسازید و به هر کدام یک کامپوننت `Audio Source` اضافه کنید. یکی را `BackgroundMusicSource` و دیگری را `SoundEffectsSource` نامگذاری کنید.
*   این دو `Audio Source` را به فیلدهای مربوطه در اسکریپت `AudioManager` بکشید.
*   فایل‌های صوتی خود را (موسیقی پس‌زمینه و صدای تصادف) در پروژه وارد کرده و آن‌ها را به فیلدهای `Background Music Clips` و `Car Crash Sound` در `AudioManager` اختصاص دهید.

## ۶. ساخت ماشین بازیکن (Player Car)

*   یک `Sprite` برای ماشین بازیکن در پوشه `Assets/Sprites` قرار دهید.
*   آن را به صحنه بکشید تا یک `GameObject` جدید ایجاد شود. نام آن را `PlayerCar` بگذارید.
*   به `PlayerCar` کامپوننت‌های زیر را اضافه کنید:
    *   `Rigidbody 2D`
    *   `Box Collider 2D`
    *   اسکریپت `Car.cs`
    *   اسکریپت `GameActionPlayer.cs`
*   در `Rigidbody 2D`، مقدار `Gravity Scale` را روی `0` تنظیم کنید.
*   در `Inspector` برای `PlayerCar`:
    *   **Crash Effect**: یک سیستم ذرات (Particle System) برای افکت تصادف بسازید (یا از Asset Store دانلود کنید)، آن را به یک `Prefab` تبدیل کرده و به فیلد `Crash Effect` در اسکریپت `Car` اختصاص دهید.

## ۷. ساخت ماشین دشمن (Enemy Car)

*   یک `Sprite` برای ماشین دشمن در پوشه `Assets/Sprites` قرار دهید.
*   آن را به صحنه بکشید و نامش را `EnemyCar` بگذارید.
*   به `EnemyCar` کامپوننت‌های زیر را اضافه کنید:
    *   `Box Collider 2D`
    *   اسکریپت `EnemyCar.cs`
*   یک `Tag` جدید با نام `EnemyCar` بسازید (`Inspector -> Add Tag...`) و آن را به `GameObject` مربوط به `EnemyCar` اختصاص دهید.
*   `EnemyCar` را به یک `Prefab` تبدیل کرده و از صحنه حذف کنید. شما بعداً می‌توانید این `Prefab` را به صورت داینامیک در بازی ایجاد کنید (مثلاً از طریق `RoadManager`).

## ۸. تنظیم MenuManager و UI

*   یک `GameObject` خالی بسازید و نام آن را به `MenuManager` تغییر دهید.
*   اسکریپت `MenuManager.cs` را به آن اضافه کنید.
*   یک `Canvas` در صحنه بسازید (`GameObject -> UI -> Canvas`).
*   داخل `Canvas`، سه پنل (`Panel`) بسازید:
    *   `MainMenuPanel`
    *   `PauseMenuPanel`
    *   `GameOverMenuPanel`
*   این پنل‌ها را به فیلدهای مربوطه در اسکریپت `MenuManager` بکشید.
*   در هر پنل، دکمه‌های مورد نیاز را بسازید (`GameObject -> UI -> Button`):
    *   **MainMenuPanel**: دکمه `Start`. به رویداد `OnClick` این دکمه، تابع `MenuManager.StartGame` را متصل کنید.
    *   **PauseMenuPanel**: دکمه‌های `Resume` و `Restart`. به ترتیب توابع `MenuManager.ResumeGame` و `MenuManager.RestartGame` را به آن‌ها متصل کنید.
    *   **GameOverMenuPanel**: دکمه `Restart`. تابع `MenuManager.RestartGame` را به آن متصل کنید.
*   به دکمه `Start` در منوی اصلی، اسکریپت `ButtonAnimator.cs` را اضافه کنید تا انیمیشن ساده‌ای داشته باشد.
*   در ابتدای بازی، `PauseMenuPanel` و `GameOverMenuPanel` را غیرفعال کنید.

## ۹. ساخت صحنه منوی اصلی (Main Menu Scene)

*   یک صحنه جدید بسازید و آن را با نام `MainMenu` در پوشه `Assets/Scenes` ذخیره کنید.
*   در این صحنه، فقط `Canvas` حاوی `MainMenuPanel` را نگه دارید.
*   در تنظیمات ساخت (`File -> Build Settings`)، صحنه `MainMenu` را به عنوان اولین صحنه (index 0) و `GameScene` را به عنوان دومین صحنه (index 1) قرار دهید.

## ۱۰. اجرای بازی

اکنون می‌توانید با اجرای صحنه `MainMenu` بازی را تست کنید. با کلیک بر روی دکمه `Start` باید به صحنه بازی منتقل شوید. در صحنه بازی، با لمس و کشیدن انگشت، ماشین حرکت می‌کند و با برخورد با ماشین‌های دشمن، از جان ماشین کم شده و در نهایت منوی `Game Over` نمایش داده می‌شود.
