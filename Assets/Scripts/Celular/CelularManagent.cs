using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CelularManagent : MonoBehaviour
{
    public static CelularManagent celularInstancia;
    [SerializeField] private GameObject appActual;
    private Stack<GameObject> historialPantallasApp = new Stack<GameObject>();
    public GameObject AppActual { get => appActual;}
    public Stack<GameObject> HistorialPantallasApp { get => historialPantallasApp; set => historialPantallasApp = value; }

    void Awake()
    {
        if (celularInstancia == null)
        {
            celularInstancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        celularInstancia = this;
        AppActual.SetActive(true);
    }
    public void CambiarPantalla(GameObject pantallaActual)
    {
        HistorialPantallasApp.Push(pantallaActual);
        Debug.Log($"Avanzó a: {pantallaActual.name}. Guardada en historial: {HistorialPantallasApp.Peek().name}");
    }
    public void CambiarApp(GameObject appAbierto)
    {
        // Esto es un puntero automatico ???
        appActual.SetActive(false);
        appActual = appAbierto;
        appActual.SetActive(true);
    }
    public void LimpiarHistorial()
    {
        HistorialPantallasApp.Clear();
        Debug.Log("Historial limpiado por completo.");
    }
}
