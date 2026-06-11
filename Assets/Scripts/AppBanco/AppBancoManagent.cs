using TMPro;
using UnityEngine;

public class AppBancoManagent : MonoBehaviour
{
    public static AppBancoManagent bancoDatosInstancia;
    [SerializeField] private float saldoActual = 0;
    [SerializeField] private string alias = "Alias";
    [SerializeField] private string cVU = "0000000000000000000000"; // <-- 22 digitos
    [SerializeField] private TextMeshProUGUI lAlias;
    [SerializeField] private TextMeshProUGUI lCVU;
    [SerializeField] private TextMeshProUGUI lSaldo;
    void Awake()
    {
        if (bancoDatosInstancia == null)
        {
            bancoDatosInstancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        ActualizarDatos();
    }
    public void ActualizarDatos()
    {
        lAlias.text = "Alias: " + alias;
        lCVU.text = "CVU: " + cVU;
        lSaldo.text = "Dinero: " + saldoActual;
    }
    public bool IngresarDinero(float cantidadIngresado)
    {
        saldoActual += cantidadIngresado;
        ActualizarDatos();
        return true;
    }
    public bool EnviarDineroCVU(float cantidadEnviada)
    {
        if((saldoActual - cantidadEnviada) < 0) // Mensajes...
            return false;
        saldoActual -= cantidadEnviada;
        ActualizarDatos();
        return true;
    }
    public void Rendimiento(float porcentaje)
    {
        saldoActual += (saldoActual*porcentaje)/100;
        ActualizarDatos();
    }
}
