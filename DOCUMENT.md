# 2D Racing Game Implementation Guide for Unity

This document provides step-by-step instructions for setting up the 2D racing game in the Unity Editor using the C# scripts and project structure we have created.

## 📂 Step 1: Project Setup

1.  **Open your Unity Project.**
2.  **Import Scripts:** Copy the entire `Assets` folder (containing the `Scripts`, `Art`, `Audio`, etc. directories) into your Unity project's root folder. Unity will automatically compile the scripts.
3.  **Prepare Assets:**
    *   **Art:** Place your background images and vehicle sprites into the `Assets/Art/Backgrounds` and `Assets/Art/Vehicles` folders, respectively.
    *   **Audio:** Place your background music and sound effects (like crash sounds) into the `Assets/Audio/Music` and `Assets/Audio/SFX` folders.

## ⚙️ Step 2: Creating Settings (`ScriptableObject`s)

Before setting up the scene, create the configuration assets that will control the game's behavior.

1.  **Create a `Settings` Folder:** In the `Project` window, create a new folder (e.g., `Assets/_Settings`) to store your setting assets.
2.  **Create Difficulty Asset:**
    *   Right-click inside the `_Settings` folder and go to **Create > Game > Difficulty**.
    *   Name the new asset (e.g., `EasyDifficulty`).
    *   Select the asset and adjust the **Speed Multiplier** and **Spawn Rate Multiplier** in the Inspector.
3.  **Create Road Settings Asset:**
    *   Right-click and go to **Create > Game > Road Settings**.
    *   Name it `DefaultRoad`.
    *   Assign your background sprite to the **Background Sprite** field and set the **Scroll Speed**.
4.  **Create Audio Settings Asset:**
    *   Right-click and go to **Create > Game > Audio Settings**.
    *   Name it `DefaultAudio`.
    *   Drag your audio clips from the `Project` window to the **Background Music**, **Crash Sound**, and **Button Click** fields.

## 씬 Step 3: Scene and GameObject Setup

Now, let's build the main game scene.

1.  **Game Managers (Empty GameObject):**
    *   Create an empty GameObject and name it `_GameManagers`.
    *   Add the following scripts to it as components:
        *   `GameManager`
        *   `AudioManager`
        *   `RoadManager`
        *   `EnemySpawner`
        *   `MenuManager`

2.  **Configure Manager Components:**
    *   Select `_GameManagers`. In the Inspector, you will see all the scripts you added.
    *   **Game Manager:** Drag your `EasyDifficulty` asset into the **Difficulty** field.
    *   **Audio Manager:** Drag your `DefaultAudio` asset into the **Audio Settings** field.
    *   **Road Manager:** Drag your `DefaultRoad` asset into the **Road Settings** field.
    *   **Enemy Spawner:** You will need an "Enemy" prefab for this (see Step 4).
    *   **Menu Manager:** You will need "Button" prefabs for this (see Step 5).

3.  **Player Car:**
    *   Drag your player car sprite into the scene to create a new GameObject.
    *   Name it `Player`.
    *   Add the following components to the `Player` GameObject:
        *   `PlayerController`
        *   `CollisionHandler`
        *   `Rigidbody2D` (set **Gravity Scale** to 0 and check **Is Kinematic** if you don't want physics forces other than your script moving it).
        *   A `BoxCollider2D` (or another 2D collider). Check **Is Trigger**.
    *   Configure its components:
        *   **Player Controller:** Drag your `EasyDifficulty` asset into the **Difficulty** field.
        *   **Collision Handler:** Drag the `_GameManagers` GameObject into both the **Game Manager** and **Audio Manager** fields.

4.  **Road Segment and Background:**
    *   Create a **Prefab** for your road segments. This should be a sprite that can be seamlessly tiled vertically.
    *   Drag this prefab into the **Road Segment Prefab** field on the `RoadManager` component.

## 🚗 Step 4: Creating Prefabs

Prefabs are essential for spawning objects like enemies and UI buttons.

1.  **Enemy Car Prefab:**
    *   Create an enemy car GameObject in the scene just like the player.
    *   Add a `Rigidbody2D` (Gravity Scale 0) and a `BoxCollider2D` (Is Trigger).
    *   **Important:** Set its **Tag** to `Enemy`. The `CollisionHandler` script looks for this tag.
    *   Create a new folder `Assets/Prefabs/Vehicles`.
    *   Drag the enemy car GameObject from the Hierarchy into this folder to create a prefab. You can now delete the instance from the scene.
    *   Go back to `_GameManagers` and drag this new enemy prefab into the **Enemy Prefab** field on the `EnemySpawner` component.

2.  **UI Button Prefabs:**
    *   Create a **Canvas** in your scene (**Create > UI > Canvas**).
    *   Inside the Canvas, create a **Button** (**Create > UI > Button**). Style it as you wish (e.g., for the Start button).
    *   Create three buttons: `StartButton`, `PauseButton`, `RestartButton`.
    *   Create a new folder `Assets/Prefabs/Buttons`.
    *   Drag each button into this folder to create prefabs.
    *   On the `_GameManagers` object, drag these button prefabs into the corresponding fields on the `MenuManager` component.
    *   **Assign Button Actions:**
        *   Select the `StartButton` prefab. In the `Button` component's `OnClick()` event, drag the `_GameManagers` GameObject into the object field. From the function dropdown, select **GameManager > StartGame()**.
        *   Do the same for the `RestartButton` (**GameManager > RestartGame()**) and `PauseButton` (**GameManager > PauseGame()**).

## 🚀 Step 5: Final Connections and Play

1.  **Assign UI Container:**
    *   On the `_GameManagers` object, find the `MenuManager` component.
    *   Drag your **Canvas** GameObject from the Hierarchy into the **UI Container** field. This tells the `MenuManager` where to create the buttons.
2.  **Main Camera:** Ensure your **Main Camera** is set to **Orthographic** projection for a 2D game.
3.  **Run the Game:** Press the Play button in Unity. You should see the start menu, and the game should be fully playable!

This guide provides the complete workflow to get your game running. You can now tweak the settings in your `ScriptableObject` assets without ever needing to change the code. Good luck!
