using UnityEngine;
using UnityEngine.UI;

public class CheatScript : MonoBehaviour
{
    [Header("Referencias del Sistema")]
    [SerializeField] private EmotionalStateManager emotionalManager;

    [Header("Paneles de la Interfaz")]
    [SerializeField] private GameObject panelSlidersDebug; 
    [SerializeField] private GameObject panelAnteriorJuego; 

    [Header("Sliders de Prueba (0 a 100)")]
    [SerializeField] private Slider sliderStress;
    [SerializeField] private Slider sliderValidation;
    [SerializeField] private Slider sliderIdentity;

    private bool isUpdatingSliders = false;

    void Start()
    {
        ConfigureSlider(sliderStress);
        ConfigureSlider(sliderValidation);
        ConfigureSlider(sliderIdentity);

        if (sliderStress != null) sliderStress.onValueChanged.AddListener(OnStressChanged);
        if (sliderValidation != null) sliderValidation.onValueChanged.AddListener(OnValidationChanged);
        if (sliderIdentity != null) sliderIdentity.onValueChanged.AddListener(OnIdentityChanged);

        if (panelSlidersDebug != null) panelSlidersDebug.SetActive(false);
        if (panelAnteriorJuego != null) panelAnteriorJuego.SetActive(true);
    }

    private void ConfigureSlider(Slider slider)
    {
        if (slider == null) return;
        slider.minValue = 0f;
        slider.maxValue = 100f;
        slider.wholeNumbers = true;
    }

    private void OnStressChanged(float value)
    {
        if (isUpdatingSliders || emotionalManager == null) return;
        
        isUpdatingSliders = true;
        emotionalManager.OverrideState(Mathf.RoundToInt(value), emotionalManager.validation, emotionalManager.identity);
        isUpdatingSliders = false;
    }

    private void OnValidationChanged(float value)
    {
        if (isUpdatingSliders || emotionalManager == null) return;
        
        isUpdatingSliders = true;
        // Pisamos la validación
        emotionalManager.OverrideState(emotionalManager.stress, Mathf.RoundToInt(value), emotionalManager.identity);
        isUpdatingSliders = false;
    }

    private void OnIdentityChanged(float value)
    {
        if (isUpdatingSliders || emotionalManager == null) return;
        
        isUpdatingSliders = true;
        // Pisamos la identidad
        emotionalManager.OverrideState(emotionalManager.stress, emotionalManager.validation, Mathf.RoundToInt(value));
        isUpdatingSliders = false;
    }

    public void SincronizarSlidersConJuego()
    {
        if (emotionalManager == null) return;

        isUpdatingSliders = true;

        if (sliderStress != null) sliderStress.value = emotionalManager.stress;
        if (sliderValidation != null) sliderValidation.value = emotionalManager.validation;
        if (sliderIdentity != null) sliderIdentity.value = emotionalManager.identity;

        isUpdatingSliders = false;
    }

    public void AlternarPanelDebug()
    {
        if (panelSlidersDebug == null || panelAnteriorJuego == null) return;

        bool debugEstaActivo = panelSlidersDebug.activeSelf;

        panelSlidersDebug.SetActive(!debugEstaActivo);
        panelAnteriorJuego.SetActive(debugEstaActivo);

        if (panelSlidersDebug.activeSelf)
        {
            SincronizarSlidersConJuego();
        }
    }

    private void OnDestroy()
    {
        if (sliderStress != null) sliderStress.onValueChanged.RemoveAllListeners();
        if (sliderValidation != null) sliderValidation.onValueChanged.RemoveAllListeners();
        if (sliderIdentity != null) sliderIdentity.onValueChanged.RemoveAllListeners();
    }
}