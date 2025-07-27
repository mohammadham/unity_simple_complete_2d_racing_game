// Managers/VibrationManager.cs
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
