using UnityEngine;

public class HapticFeedback : MonoBehaviour {
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrationService;
    private static AndroidJavaClass vibrationEffectClass;
    private static int defaultAmplitude;

    static HapticFeedback() {
        using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity")) {
            vibrationService = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            // For newer Android versions
            if (AndroidVersion.SDK_INT >= 26) {
                vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
            }
        }
    }
#endif

    public static void Vibrate(long milliseconds) {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (AndroidVersion.SDK_INT >= 26) {
            var effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, defaultAmplitude);
            vibrationService.Call("vibrate", effect);
        } else {
            vibrationService.Call("vibrate", milliseconds);
        }
#else
        // Fallback for editor or other platforms
        Handheld.Vibrate();
#endif
    }
}

#if UNITY_ANDROID && !UNITY_EDITOR
public static class AndroidVersion {
    private static int? sdk_int;
    public static int SDK_INT {
        get {
            if (sdk_int == null) {
                using (var version = new AndroidJavaClass("android.os.Build$VERSION")) {
                    sdk_int = version.GetStatic<int>("SDK_INT");
                }
            }
            return sdk_int.Value;
        }
    }
}
#endif
