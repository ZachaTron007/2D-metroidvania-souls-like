using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class PostProcessingEffectScript : MonoBehaviour
{
    public static PostProcessingEffectScript instance { get; private set; }
    private Volume volume;
    [SerializeField] Vignette vignette;
    [SerializeField] private float vignetteIntensity = 1f;
    [SerializeField] private float vignetteSmoothness = .5f;
    [SerializeField] private float HurtVignetteTime;
    private float time;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(instance);
        }
        instance = this;
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out Vignette vignette)) {
            this.vignette = vignette;
        }
    }

    public void HurtVignette() {
        vignetteIntensity = .353f;
        vignette.intensity.value = .353f;
        vignette.smoothness.value = 1f;
        vignette.color.value = Color.red;
        time = HurtVignetteTime;
    }

    private void Update() {
        if (time > 0) {
            time -= Time.deltaTime;
            if (time <= 0) {
                time = 0;
                vignette.intensity.value = 0;
            }
            if (time <= vignetteSmoothness) {
                float value = Mathf.Lerp(vignetteIntensity, 0, 1-(time / vignetteSmoothness));
                vignette.intensity.value = value;
            }
        }
    }
}
