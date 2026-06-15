using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ReactiveUIManager : MonoBehaviour
{
    [Header("UI Overlays")]
    [SerializeField] private Image validationDarkOverlay;
    [SerializeField] private Image blurOverlay;
    [SerializeField] private Image glitchOverlay;

    [Header("Post Processing")]
    [SerializeField] private Volume volume;

    private Vignette vignette;
    private ChromaticAberration chromaticAberration;
    private FilmGrain filmGrain;

    private int currentIdentity;

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
        // Identidad baja = glitch
        if (currentIdentity < 30)
        {
            float glitchAlpha = Mathf.PingPong(Time.time * 10f, 1f) * 0.45f;
            SetAlpha(glitchOverlay, glitchAlpha);
        }
        else
        {
            SetAlpha(glitchOverlay, 0f);
        }
    }

    public void SetEmotionalState(int stress, int validation, int identity)
    {
        stress = Mathf.Clamp(stress, 0, 100);
        validation = Mathf.Clamp(validation, 0, 100);
        identity = Mathf.Clamp(identity, 0, 100);

        currentIdentity = identity;

        // 🔥 Estrés: vignette + distorsión de color
        if (vignette != null)
            vignette.intensity.value = stress / 100f * 0.55f;

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = stress > 50
                ? (stress - 50) / 50f * 0.6f
                : 0f;

        if (filmGrain != null)
            filmGrain.intensity.value = stress > 60
                ? (stress - 60) / 40f * 0.7f
                : 0f;

        // ⭐ Validación baja: oscurecimiento
        float validationDarkness = validation < 50
            ? (50 - validation) / 50f * 0.65f
            : 0f;

        SetAlpha(validationDarkOverlay, validationDarkness);

        // 🪞 Identidad baja: blur leve
        float blurAlpha = identity < 50
            ? (50 - identity) / 50f * 0.35f
            : 0f;

        SetAlpha(blurOverlay, blurAlpha);
    }

    private void SetAlpha(Image img, float alpha)
    {
        if (img == null) return;

        Color c = img.color;
        c.a = Mathf.Clamp01(alpha);
        img.color = c;
    }
}