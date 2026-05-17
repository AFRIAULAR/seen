using UnityEngine;

public class CelularInterface : MonoBehaviour
{
    [SerializeField] private GameObject pantallaInicio;
    [SerializeField] private GameObject pantallaAppMsg;
    private GameObject pantallaActual;
    public void RetrocederPantalla()
    {
        if (CelularManagent.celularInstancia.HistorialPantallasApp.Count > 1)
        {
            pantallaActual = CelularManagent.celularInstancia.HistorialPantallasApp.Pop();
            pantallaActual.SetActive(false);
            pantallaActual = CelularManagent.celularInstancia.HistorialPantallasApp.Peek();
            pantallaActual.SetActive(true);

            Debug.Log($"Retrocedió a: {pantallaActual.name}. Elementos restantes en historial: {CelularManagent.celularInstancia.HistorialPantallasApp.Count}");
        }
        else
        {
            Debug.Log("No hay más historial. Ya estás en el menú de inicio.");
            VolverInicio();
        }
    }
    public void VolverInicio()
    {
        CelularManagent.celularInstancia.LimpiarHistorial();
        CelularManagent.celularInstancia.CambiarApp(pantallaInicio);
    }
    public void AbrirAppMsg()
    {
        CelularManagent.celularInstancia.CambiarApp(pantallaAppMsg);
    }
}
