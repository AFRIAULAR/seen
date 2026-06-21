using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using FMODUnity;

public class AppMusicaManagent : MonoBehaviour
{
    [Header("Referencias de FMOD")]
    [SerializeField] private StudioEventEmitter musicaEmitter;

    [Header("Configuración del Parámetro")]
    [SerializeField] private string nombreParametro = "IndiceMusica";

    [System.Serializable]
    public struct DatosCancion
    {
        public string nombreDeLaPista;
        public int duracionSegundosTotales;
        public Sprite portadaCancion; 
    }

    [Header("Base de Datos de Canciones")]
    [SerializeField] private DatosCancion[] listaCanciones;

    [Header("Componentes de la Interfaz Visual (UI)")]
    [SerializeField] private Image componenteImagenPortada;
    [SerializeField] private TextMeshProUGUI componenteTextoNombre;
    [SerializeField] private Slider sliderProgreso;

    [Header("Botones de Control (UI)")]
    [Tooltip("Arrastrá acá el GameObject del botón de PLAY")]
    [SerializeField] private GameObject botonPlay;
    [Tooltip("Arrastrá acá el GameObject del botón de PAUSA")]
    [SerializeField] private GameObject botonPausa;

    [Header("Referencias de Sistemas")]
    [SerializeField] private EmotionalStateManager emosionalManager;

    [Header("Configuración de Alivio")]
    [Tooltip("Cuánto estrés baja por cada segundo de música reproducido")]
    [SerializeField] private float estresReducidoPorPulso = 2f;
    [Tooltip("Cada cuántos segundos se aplica el descuento de estrés (ej: 1s, 5s, 10s)")]
    [SerializeField] private float intervaloReduccionEstres = 5f;

    // Variable interna para acumular los fragmentos de tiempo de la corrutina
    private float acumuladorTiempo = 0f;

    private float indiceMaximo; 
    private int indiceActual = 0;
    private Coroutine corrutinaProgreso; 
    private bool estaReproduciendo = false;

    void Start()
    {
        if (musicaEmitter == null)
            musicaEmitter = GetComponent<StudioEventEmitter>();

        ObtenerMaximoDirecto();
        ActualizarUIYDatosPista();
        ActualizarVisualBotones();
    }

    private void ObtenerMaximoDirecto()
    {
        if (musicaEmitter == null) return;
        try
        {
            FMOD.Studio.EventDescription descripcionEvento = RuntimeManager.GetEventDescription(musicaEmitter.EventReference);
            FMOD.Studio.PARAMETER_DESCRIPTION descripcionParametro;
            descripcionEvento.getParameterDescriptionByName(nombreParametro, out descripcionParametro);
            indiceMaximo = descripcionParametro.maximum;
        }
        catch (System.Exception e) { Debug.LogError(e.Message); }
    }

    private void ActualizarUIYDatosPista()
    {
        if (listaCanciones == null || listaCanciones.Length == 0 || indiceActual >= listaCanciones.Length) return;

        DatosCancion cancionActual = listaCanciones[indiceActual];
        
        if (componenteImagenPortada != null && cancionActual.portadaCancion != null)
            componenteImagenPortada.sprite = cancionActual.portadaCancion;

        if (componenteTextoNombre != null)
        {
            int minutos = cancionActual.duracionSegundosTotales / 60;
            int segundos = cancionActual.duracionSegundosTotales % 60;
            
            componenteTextoNombre.text = $"{cancionActual.nombreDeLaPista} ({minutos}:{segundos:00})";
        }

        if (sliderProgreso != null)
        {
            sliderProgreso.minValue = 0;
            sliderProgreso.maxValue = cancionActual.duracionSegundosTotales; // <--- Asignación directa y limpia
            sliderProgreso.value = 0; 
        }
    }

    private void ActualizarVisualBotones()
    {
        if (botonPlay != null && botonPausa != null)
        {
            botonPlay.SetActive(!estaReproduciendo);
            botonPausa.SetActive(estaReproduciendo);
        }
    }

    public void ReproducirMusica()
    {
        if (musicaEmitter == null) return;

        if (estaReproduciendo) return;

        estaReproduciendo = true;
        if (MemoryManager.Instance != null)
        {
            MemoryManager.Instance.MarcarMusicaEscuchada();
        }
        musicaEmitter.EventInstance.getPaused(out bool estaPausadoEnFMOD);

        if (estaPausadoEnFMOD)
        {
            musicaEmitter.EventInstance.setPaused(false);
            Debug.Log($"[AppMusicaManagent] RESUME -> Continuando pista {indiceActual} desde donde quedó.");
        }
        else
        {
            musicaEmitter.Play();
            musicaEmitter.SetParameter(nombreParametro, indiceActual);
            Debug.Log($"[AppMusicaManagent] PLAY -> Arrancando pista {indiceActual} desde cero.");
        }
        
        ActualizarVisualBotones();

        if (corrutinaProgreso != null) StopCoroutine(corrutinaProgreso);
        corrutinaProgreso = StartCoroutine(ActualizarSliderProgreso());

        
    }

    public void CambiarCancion(int paso)
    {
        if (musicaEmitter == null) return;

        bool deberiaContinuarSonando = estaReproduciendo;

        DetenerMusica();

        indiceActual += paso;

        if (indiceActual > indiceMaximo) 
        {
            indiceActual = 0; 
        }
        else if (indiceActual < 0)
        {
            indiceActual = (int)indiceMaximo; 
        }

        ActualizarUIYDatosPista();

        if (deberiaContinuarSonando)
        {
            ReproducirMusica();
        }
        else
        {
            musicaEmitter.SetParameter(nombreParametro, indiceActual);
            ActualizarVisualBotones();
        }
    }

    public void DetenerMusica()
    {
        if (musicaEmitter == null) return;

        estaReproduciendo = false;

        musicaEmitter.Stop(); 

        if (corrutinaProgreso != null) 
        {
            StopCoroutine(corrutinaProgreso); 
            corrutinaProgreso = null; 
        }

        if (sliderProgreso != null) 
        {
            sliderProgreso.value = 0;
        }
        ActualizarVisualBotones();

        Debug.Log("[AppMusicaManagent] STOP: Música cortada y botones reseteados a estado inicial.");
    }

    public void BotonDetenerMusica()
    {
        DetenerMusica();
        ActualizarVisualBotones();
    }

    public void PausarMusica()
    {
        if (musicaEmitter == null) return;

        if (estaReproduciendo)
        {
            estaReproduciendo = false;

            musicaEmitter.EventInstance.setPaused(true);

            if (corrutinaProgreso != null)
            {
                StopCoroutine(corrutinaProgreso);
                corrutinaProgreso = null;
            }

            ActualizarVisualBotones();

            Debug.Log("[AppMusicaManagent] Música PAUSADA (Slider congelado).");
        }
    }

    private IEnumerator ActualizarSliderProgreso()
    {
        if (sliderProgreso == null) yield break;

        while (estaReproduciendo)
        {   
            musicaEmitter.EventInstance.getTimelinePosition(out int posicionMilisegundos);
            float posicionSegundos = posicionMilisegundos / 1000f;
            
            sliderProgreso.value = posicionSegundos;

            if (sliderProgreso.value >= sliderProgreso.maxValue)
            {
                Debug.Log("[AppMusicaManagent] Fin de la canción detectado por el Slider.");
                
                DetenerMusica(); 
                yield break;
            }

            if (emosionalManager != null)
            {
                acumuladorTiempo += 0.1f;

                if (acumuladorTiempo >= intervaloReduccionEstres)
                {
                    emosionalManager.ModifyState(-Mathf.RoundToInt(estresReducidoPorPulso), 0, 0);
                    
                    acumuladorTiempo = 0f;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
        corrutinaProgreso = null;
    }

    public void AlEmpezarAArrastrarSlider()
    {
        if (corrutinaProgreso != null)
        {
            StopCoroutine(corrutinaProgreso);
            corrutinaProgreso = null;
        }
    }

    public void AlModificarTiempoSlider()
    {
        if (musicaEmitter == null || sliderProgreso == null) return;

        int nuevoTiempoMilisegundos = Mathf.RoundToInt(sliderProgreso.value * 1000f);

        if (!musicaEmitter.IsPlaying())
        {
            musicaEmitter.Play(); 
            musicaEmitter.SetParameter(nombreParametro, indiceActual);
        }

        musicaEmitter.EventInstance.setTimelinePosition(nuevoTiempoMilisegundos);

        FMODUnity.RuntimeManager.StudioSystem.update();

        if (estaReproduciendo)
        {
            musicaEmitter.EventInstance.setPaused(false);

            if (corrutinaProgreso != null) StopCoroutine(corrutinaProgreso);
            corrutinaProgreso = StartCoroutine(ActualizarSliderProgreso());
            
            Debug.Log($"[AppMusicaManagent] Reproduciendo desde el segundo exacto: {sliderProgreso.value}s");
        }
        else
        {
            musicaEmitter.EventInstance.setPaused(true);
            Debug.Log($"[AppMusicaManagent] Reproductor en pausa. Aguja preparada en el segundo: {sliderProgreso.value}s");
        }
    }
}