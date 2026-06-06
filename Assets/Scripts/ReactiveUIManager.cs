using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ReactiveUIManager : MonoBehaviour
{
    [Header("UI Overlays")]
    [SerializeField] private Image stressOverlay;
    [SerializeField] private Image blurOverlay;
    [SerializeField] private Image noiseOverlay;
    [SerializeField] private Image glitchOverlay;

    [Header("Post Processing")]
    [SerializeField] private Volume volume;

    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;

    private int currentStress;

    private void Awake()
    {
        if (volume != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out chromaticAberration);
            volume.profile.TryGet(out filmGrain);
        }
    }

    private void Update()
    {
        if (currentStress > 80)
        {
            float glitchAlpha = Mathf.PingPong(Time.time * 10f, 1f) * 0.45f;
            SetAlpha(glitchOverlay, glitchAlpha);
        }
        else
        {
            SetAlpha(glitchOverlay, 0f);
        }
    }

    public void SetStress(int stress)
    {
        currentStress = Mathf.Clamp(stress, 0, 100);

        SetAlpha(stressOverlay, currentStress / 100f * 0.75f);
        SetAlpha(blurOverlay, currentStress / 100f * 0.35f);

        float noiseAlpha = currentStress > 60
            ? (currentStress - 60) / 40f * 0.35f
            : 0f;

        SetAlpha(noiseOverlay, noiseAlpha);

        if (vignette != null)
            vignette.intensity.value = currentStress / 100f * 0.55f;

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = currentStress > 60
                ? (currentStress - 60) / 40f * 0.5f
                : 0f;

        if (filmGrain != null)
            filmGrain.intensity.value = currentStress > 70
                ? (currentStress - 70) / 30f * 0.8f
                : 0f;
    }

    private void SetAlpha(Image img, float alpha)
    {
        if (img == null) return;

        Color c = img.color;
        c.a = Mathf.Clamp01(alpha);
        img.color = c;
    }
}