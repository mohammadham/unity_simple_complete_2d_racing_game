using UnityEngine;

public class HapticFeedback : MonoBehaviour {
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrationService;

    static HapticFeedback() {
        using (var plugin = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            vibrationService = plugin.GetStatic<AndroidJavaObject>("currentActivity")
                .Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
    }
#endif

    public static void Vibrate(long milliseconds) {
#if UNITY_ANDROID && !UNITY_EDITOR
        vibrationService.Call("vibrate", milliseconds);
#else
        Debug.Log("HapticFeedback.Vibrate() called. Will only vibrate on Android device.");
#endif
    }
}
