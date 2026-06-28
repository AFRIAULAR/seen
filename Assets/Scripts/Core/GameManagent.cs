using FMODUnity;
using UnityEngine;

public class GameManagent : MonoBehaviour
{
    [Header("Referencias de FMOD")]
    [SerializeField] private StudioEventEmitter emitterAmbienceBase;
    [SerializeField] private StudioEventEmitter emitterSFXAmbience;
    [SerializeField] private EventReference eventoBotonesUI;
    public static GameManagent gameInstancia;

    void Awake()
    {
        if (gameInstancia == null)
        {
            gameInstancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Audio
        if (emitterAmbienceBase != null)
        {
            emitterAmbienceBase.Play();
        }

        if (emitterSFXAmbience != null)
        {
            emitterSFXAmbience.Play();
        }
    }

    public void DetenerAmbiente()
    {
        if (emitterAmbienceBase != null) emitterAmbienceBase.Stop();
        if (emitterSFXAmbience != null) emitterSFXAmbience.Stop();
    }

    public void ReproducirHover()
    {
        DispararSonidoUI(1);
    }
    public void ReproducirClic()
    {
        DispararSonidoUI(0);
    }
    private void DispararSonidoUI(int idParametro)
    {
        if (!eventoBotonesUI.IsNull)
        {
            FMOD.Studio.EventInstance instancia = RuntimeManager.CreateInstance(eventoBotonesUI);
            
            instancia.setParameterByName("BtnUI", idParametro);
            
            instancia.start();
            
            instancia.release();
        }
    }

    private void OnDestroy()
    {
        DetenerAmbiente();
    }
}
