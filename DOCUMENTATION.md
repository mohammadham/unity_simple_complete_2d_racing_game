# 2D Driving Game - Unity Project Setup Guide

This document provides a step-by-step guide to setting up the 2D Driving Game project in the Unity Editor.

## 1. Project Setup

1.  **Open the Project in Unity:** Open Unity Hub, click "Open", and select the root folder of this project.
2.  **Folder Structure:** The project follows a standard Unity project structure. All the necessary folders have been created for you.

    ```
    Assets/
    ├── Scripts/
    │   ├── Managers/
    │   ├── Systems/
    │   ├── Enemies/
    │   └── UI/
    ├── Sprites/
    │   ├── Backgrounds/
    │   ├── Cars/
    │   └── UI/
    ├── Audio/
    │   ├── Music/
    │   └── SFX/
    └── ScriptableObjects/
    ```

## 2. Creating ScriptableObjects

1.  **Create HealthData:**
    *   In the `Project` window, navigate to `Assets/ScriptableObjects`.
    *   Right-click and select `Create > ScriptableObjects > HealthData`.
    *   Name the new asset `PlayerHealthData`.
    *   You can adjust the `Max Health` value in the Inspector.

2.  **Create DifficultySettings:**
    *   In the `Project` window, navigate to `Assets/ScriptableObjects`.
    *   Right-click and select `Create > ScriptableObjects > DifficultySettings`.
    *   Name the new asset `EasyDifficulty`.
    *   You can adjust the `Car Speed`, `Enemy Spawn Rate`, and `Max Health` values in the Inspector.

## 3. Scene Setup

1.  **Create a New Scene:** Go to `File > New Scene` and save it as `MainScene` in the `Assets` folder.

2.  **Create Game Manager:**
    *   Create an empty GameObject and name it `GameManager`.
    *   Add the following scripts to the `GameManager` object:
        *   `RoadManager` (from `Assets/Scripts/Managers`)
        *   `AudioManager` (from `Assets/Scripts/Managers`)
        *   `UIManager` (from `Assets/Scripts/UI`)
        *   `GameOverManager` (from `Assets/Scripts/UI`)
        *   `EnemySpawner` (from `Assets/Scripts/Enemies`)

3.  **Create Player Car:**
    *   Create a new 2D sprite (`GameObject > 2D Object > Sprite`) and name it `Player`.
    *   Add a `PlayerController` script to it.
    *   Add a `HealthSystem` script to it.
    *   Add a `CollisionHandler` script to it.
    *   Add a `Rigidbody2D` component and set `Gravity Scale` to 0.
    *   Add a `BoxCollider2D` component.
    *   Drag the `PlayerHealthData` ScriptableObject to the `Health Data` field in the `HealthSystem` component.
    *   Drag the `HealthSystem` component to the `Health System` field in the `CollisionHandler` component.

4.  **Create Enemy Car Prefab:**
    *   Create a new 2D sprite and name it `Enemy`.
    *   Add a `Rigidbody2D` component and set `Gravity Scale` to 0.
    *   Add a `BoxCollider2D` component.
    *   Set the `Tag` to `Enemy`.
    *   Create a simple `EnemyMovement.cs` script to move the enemy down the screen.
    *   Drag the `Enemy` GameObject into the `Assets` folder to create a prefab. Delete the `Enemy` from the scene.

5.  **Configure RoadManager:**
    *   Select the `GameManager` object.
    *   In the `RoadManager` component, you can set the `Background Sprite` and `Scroll Speed`.
    *   Drag the `Enemy` prefab to the `Enemy Car Prefab` field.
    *   Create a few empty GameObjects as spawn points for the enemies and drag them to the `Spawn Points` array.

## 4. UI Setup

1.  **Create Canvas:**
    *   Create a new Canvas (`GameObject > UI > Canvas`).
    *   Set the `Render Mode` to `Screen Space - Camera`.
    *   Drag the `Main Camera` to the `Render Camera` field.

2.  **Create UI Elements:**
    *   Create a `StartButton` and link its `OnClick` event to the `UIManager.StartGame()` method.
    *   Create a `PauseButton` and link its `OnClick` event to the `UIManager.PauseGame()` method.
    *   Create a `RestartButton` and link its `OnClick` event to the `GameOverManager.RestartGame()` method.
    *   Create a `GameOverPanel` (a `Panel` UI element) and add the `RestartButton` as a child.
    *   Connect all the UI elements to their respective fields in the `UIManager` and `GameOverManager` components on the `GameManager` object.

## 5. Running the Game

1.  **Press Play:** Press the play button in the Unity Editor to start the game.
2.  **Gameplay:**
    *   Click the `StartButton` to begin.
    *   Use touch controls to move the player car left and right.
    *   Avoid colliding with enemy cars.
    *   If you collide with an enemy, your health will decrease.
    *   When your health reaches zero, the game over panel will appear.
    *   Click the `RestartButton` to play again.

This guide should provide you with everything you need to get the project up and running. Enjoy!
