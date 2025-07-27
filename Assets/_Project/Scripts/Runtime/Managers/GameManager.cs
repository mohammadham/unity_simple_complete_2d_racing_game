// Managers/GameManager.cs
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public string currentRoadType = "asphalt";
    public event System.Action OnGameOver;
    public void TriggerGameOver() => OnGameOver?.Invoke();
}
