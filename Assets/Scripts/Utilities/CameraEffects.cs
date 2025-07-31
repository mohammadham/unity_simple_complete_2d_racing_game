using UnityEngine;
using System.Collections;

public class CameraEffects : MonoBehaviour {
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private float remainingShakeTime = 0f;

    void Start() {
        originalPosition = transform.localPosition;
    }

    void Update() {
        if (remainingShakeTime > 0) {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            remainingShakeTime -= Time.deltaTime;
        } else {
            remainingShakeTime = 0f;
            transform.localPosition = originalPosition;
        }
    }

    public void Shake() {
        originalPosition = transform.localPosition;
        remainingShakeTime = shakeDuration;
    }
}
