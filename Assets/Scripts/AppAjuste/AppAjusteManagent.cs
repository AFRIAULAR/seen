using UnityEngine;
using UnityEngine.UI;

public class AppAjusteManagent : MonoBehaviour
{
    [Header("Referencias del Sistema")]
    [SerializeField] private AudioManagent audioManager;

    [Header("Sliders de la UI")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderAmbience;

    [Header("Navegación de Paneles")]
    [Tooltip("El GameObject padre que contiene a los 3 paneles como hijos directos")]
    [SerializeField] private Transform contenedorPaneles;
    void Start()
    {
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManagent>();
        }

        SincronizarSlidersConFMOD();

        if (sliderMaster != null)
            sliderMaster.onValueChanged.AddListener(OnMasterSliderChanged);

        if (sliderMusic != null)
            sliderMusic.onValueChanged.AddListener(OnMusicSliderChanged);

        if (sliderSFX != null)
            sliderSFX.onValueChanged.AddListener(OnSFXSliderChanged);

        if (sliderAmbience != null)
            sliderAmbience.onValueChanged.AddListener(OnAmbienceSliderChanged);

        if (contenedorPaneles != null && contenedorPaneles.childCount > 0)
        {
            // Obtenemos el primer hijo del contenedor y lo mostramos
            GameObject primerPanel = contenedorPaneles.GetChild(0).gameObject;
            CambiarPanel(primerPanel);
        }
    }

    /// <summary>
    /// Apaga todos los paneles hijos del contenedor y enciende únicamente el panel seleccionado.
    /// </summary>
    /// <param name="panelACambiar">El GameObject del panel que se quiere mostrar.</param>
    public void CambiarPanel(GameObject panelACambiar)
    {
        if (contenedorPaneles == null || panelACambiar == null) return;
        foreach (Transform hijo in contenedorPaneles)
        {
            hijo.gameObject.SetActive(false);
        }
        panelACambiar.SetActive(true);
        
        Debug.Log($"[AppAjusteManagent] Se mostró el panel: {panelACambiar.name}");
    }

    /// <summary>
    /// Pregunta a FMOD qué volumen tienen los buses y actualiza la barra visual del Slider.
    /// </summary>
    private void SincronizarSlidersConFMOD()
    {
        if (audioManager == null) return;

        if (sliderMaster != null)
        {
            FMODUnity.RuntimeManager.GetBus("bus:/").getVolume(out float volMaster);
            sliderMaster.value = volMaster;
        }

        if (sliderMusic != null)
        {
            FMODUnity.RuntimeManager.GetBus("bus:/Music").getVolume(out float volMusic);
            sliderMusic.value = volMusic;
        }

        if (sliderSFX != null)
        {
            FMODUnity.RuntimeManager.GetBus("bus:/SFX").getVolume(out float volSFX);
            sliderSFX.value = volSFX;
        }

        if (sliderAmbience != null)
        {
            FMODUnity.RuntimeManager.GetBus("bus:/Ambience").getVolume(out float volAmbience);
            sliderAmbience.value = volAmbience;
        }
    }
    private void OnMasterSliderChanged(float valor)
    {
        audioManager.CambiarVolumen(AudioManagent.TipoBus.Master, valor);
    }

    private void OnMusicSliderChanged(float valor)
    {
        audioManager.CambiarVolumen(AudioManagent.TipoBus.Music, valor);
    }

    private void OnSFXSliderChanged(float valor)
    {
        audioManager.CambiarVolumen(AudioManagent.TipoBus.SFX, valor);
    }
    private void OnAmbienceSliderChanged(float valor)
    {
        audioManager.CambiarVolumen(AudioManagent.TipoBus.Ambience, valor);
    }

    void OnDestroy()
    {
        if (sliderMaster != null) sliderMaster.onValueChanged.RemoveAllListeners();
        if (sliderMusic != null) sliderMusic.onValueChanged.RemoveAllListeners();
        if (sliderSFX != null) sliderSFX.onValueChanged.RemoveAllListeners();
        if (sliderAmbience != null) sliderAmbience.onValueChanged.RemoveAllListeners();
    }
}