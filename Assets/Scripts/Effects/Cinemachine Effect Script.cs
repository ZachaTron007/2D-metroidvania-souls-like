using UnityEngine;

using Cinemachine;

public class CinemachineEffectScript : MonoBehaviour
{
    public static CinemachineEffectScript instance { get; private set; }
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin cameraShake;
    private float shakeTime;
    private float startShakeTime;
    private float AmplitudeStart = 0f;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(instance);
        }
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraShake = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ScreenShake(float magnitude, float duration) {
        
        cameraShake.m_AmplitudeGain = magnitude;
        AmplitudeStart = magnitude;
        startShakeTime = duration;
        shakeTime = duration;
    }

    private void Update() {
        if (shakeTime > 0) {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0) {
                cameraShake.m_AmplitudeGain = 0;
            }
            cameraShake.m_AmplitudeGain = Mathf.Lerp(AmplitudeStart, 0, 1 - (shakeTime / startShakeTime));
        }
    }
}
