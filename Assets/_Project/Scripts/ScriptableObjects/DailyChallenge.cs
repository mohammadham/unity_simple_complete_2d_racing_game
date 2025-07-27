using UnityEngine;

[CreateAssetMenu(menuName = "TrafficRush/DailyChallenge")]
public class DailyChallenge : ScriptableObject {
    public string title;
    public System.Func<bool> condition; // e.g. survive 120s
    public int rewardCoins;
}
