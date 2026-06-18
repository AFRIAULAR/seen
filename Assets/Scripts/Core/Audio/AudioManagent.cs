using UnityEngine;

public class AudioManagent : MonoBehaviour
{
    // Definimos los tipos de canales disponibles en tu juego
    public enum TipoBus
    {
        Master,
        Music,
        SFX,
        Ambience,
        UI
    }

    [Header("Rutas de FMOD Studio")]
    [SerializeField] private string rutaMaster = "bus:/";
    [SerializeField] private string rutaMusic = "bus:/Music";
    [SerializeField] private string rutaSFX = "bus:/SFX";
    [SerializeField] private string rutaAmbience = "bus:/Ambience";
    [SerializeField] private string rutaUI = "bus:/UI";

    // Variables nativas de FMOD
    private FMOD.Studio.Bus fmodMaster;
    private FMOD.Studio.Bus fmodMusic;
    private FMOD.Studio.Bus fmodSFX;
    private FMOD.Studio.Bus fmodAmbience;
    private FMOD.Studio.Bus fmodUI;

    void Start()
    {
        // Inicializamos todos los buses al arrancar
        fmodMaster = FMODUnity.RuntimeManager.GetBus(rutaMaster);
        fmodMusic = FMODUnity.RuntimeManager.GetBus(rutaMusic);
        fmodSFX = FMODUnity.RuntimeManager.GetBus(rutaSFX);
        fmodAmbience = FMODUnity.RuntimeManager.GetBus(rutaAmbience);
        fmodUI = FMODUnity.RuntimeManager.GetBus(rutaUI);
    }

    /// <summary>
    /// Cambia el volumen de CUALQUIER bus pasándole el tipo de canal.
    /// </summary>
    public void CambiarVolumen(TipoBus canal, float volumen)
    {
        volumen = Mathf.Clamp01(volumen);
        FMOD.Studio.Bus busObjetivo = ObtenerBus(canal);

        if (busObjetivo.isValid())
        {
            busObjetivo.setVolume(volumen);
            Debug.Log($"[AudioManagent] Volumen de {canal} cambiado a {volumen}");
        }
    }

    /// <summary>
    /// Alterna el estado de Mute de CUALQUIER bus pasándole el tipo de canal.
    /// </summary>
    public void AlternarMute(TipoBus canal, bool estaMuteado)
    {
        FMOD.Studio.Bus busObjetivo = ObtenerBus(canal);

        if (busObjetivo.isValid())
        {
            busObjetivo.setMute(estaMuteado);
            Debug.Log($"[AudioManagent] Mute de {canal} fijado en {estaMuteado}");
        }
    }

    /// <summary>
    /// Método auxiliar interno que traduce el "Enum" al bus de FMOD correspondiente.
    /// </summary>
    private FMOD.Studio.Bus ObtenerBus(TipoBus canal)
    {
        switch (canal)
        {
            case TipoBus.Master:   return fmodMaster;
            case TipoBus.Music:    return fmodMusic;
            case TipoBus.SFX:      return fmodSFX;
            case TipoBus.Ambience: return fmodAmbience;
            case TipoBus.UI:       return fmodUI;
            default:               return default;
        }
    }

    /// <summary>
    /// Envía los estados emocionales directamente a los parámetros globales de FMOD.
    /// FMOD se encargará de aplicar automáticamente los efectos al canal de música.
    /// </summary>
    public void ActualizarParametrosEmocionales(int stress, int validation, int identity)
    {
        // Aseguramos que los valores vayan de 0 a 100 (o adaptalo si usas rango 0 a 1)
        float f_stress = Mathf.Clamp(stress, 0, 100);
        float f_validation = Mathf.Clamp(validation, 0, 100);
        float f_identity = Mathf.Clamp(identity, 0, 100);

        // Mandamos los valores de forma global a FMOD Studio
        // IMPORTANTE: Los strings deben coincidir EXACTAMENTE con FMOD
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Estres", f_stress);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Validacion", f_validation);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Identidad", f_identity);
    }
}