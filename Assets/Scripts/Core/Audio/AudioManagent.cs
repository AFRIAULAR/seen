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
}