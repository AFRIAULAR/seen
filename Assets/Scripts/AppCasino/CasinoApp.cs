using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CasinoApp : MonoBehaviour
{
    // ── PANTALLA Y NAVEGACION ─────────────────────────────────────
    [Header("Pantalla")]
    [SerializeField] private GameObject pantallaCasino;

    // ── DINERO ────────────────────────────────────────────────────
    [Header("Dinero")]
    [SerializeField] private TMP_Text textoDinero;
    [SerializeField] private TMP_Text textoApuesta;
    private int dineroJugador = 0;
    private int apuestaActual = 10;

    // ── RULETA ────────────────────────────────────────────────────
    [Header("Ruleta")]
    [SerializeField] private TMP_Text textoResultado;
    [SerializeField] private Button botonRojo;
    [SerializeField] private Button botonNegro;
    [SerializeField] private Button botonVerde;
    [SerializeField] private Button botonGirar;
    private string colorSeleccionado = "";

    // ── CODIGO SECRETO ────────────────────────────────────────────
    [Header("Codigo Secreto")]
    [SerializeField] private GameObject panelCodigoSecreto;
    [SerializeField] private TMP_InputField inputCodigo;
    [SerializeField] private TMP_Text textoMensajeCodigo;
    [SerializeField] private Button botonConfirmarCodigo;
    [SerializeField] private Button botonAbrirPanelCodigo;
    private const string CODIGO_MARIO = "MARIO2025";
    private bool dineroDesbloqueado = false;

    // ── ESTRES ────────────────────────────────────────────────────
    [Header("Sistema de Estres")]
    [SerializeField] private EmotionalStateManager emotionalStateManager;

    // ─────────────────────────────────────────────────────────────

    private void Start()
    {
        ActualizarUI();
        if (panelCodigoSecreto != null) panelCodigoSecreto.SetActive(false);

        if (botonRojo != null)   botonRojo.onClick.AddListener(() => SeleccionarColor("Rojo"));
        if (botonNegro != null)  botonNegro.onClick.AddListener(() => SeleccionarColor("Negro"));
        if (botonVerde != null)  botonVerde.onClick.AddListener(() => SeleccionarColor("Verde"));
        if (botonGirar != null)  botonGirar.onClick.AddListener(GirarRuleta);
        if (botonAbrirPanelCodigo != null) botonAbrirPanelCodigo.onClick.AddListener(AbrirPanelCodigo);
        if (botonConfirmarCodigo != null)  botonConfirmarCodigo.onClick.AddListener(ConfirmarCodigo);
    }

    // ── SELECCION DE COLOR ────────────────────────────────────────
    private void SeleccionarColor(string color)
    {
        colorSeleccionado = color;
        if (textoResultado != null)
            textoResultado.text = "Apostaste a: " + color;
    }

    // ── GIRAR RULETA ──────────────────────────────────────────────
    private void GirarRuleta()
    {
        if (string.IsNullOrEmpty(colorSeleccionado))
        {
            if (textoResultado != null) textoResultado.text = "Primero elegí un color.";
            return;
        }
        if (dineroJugador < apuestaActual)
        {
            if (textoResultado != null) textoResultado.text = "No tenes suficiente dinero.";
            return;
        }

        // Sorteo: 45% Rojo, 45% Negro, 10% Verde
        float valor = Random.Range(0f, 1f);
        string resultado;
        if (valor < 0.45f)      resultado = "Rojo";
        else if (valor < 0.90f) resultado = "Negro";
        else                    resultado = "Verde";

        if (resultado == colorSeleccionado)
        {
            int ganancia = (resultado == "Verde") ? apuestaActual * 5 : apuestaActual;
            dineroJugador += ganancia;
            if (textoResultado != null)
                textoResultado.text = "Salio " + resultado + ". GANAS $" + ganancia + "!";
        }
        else
        {
            dineroJugador -= apuestaActual;
            if (textoResultado != null)
                textoResultado.text = "Salio " + resultado + ". Perdiste $" + apuestaActual + ".";

            // +15% de estres al perder
            if (emotionalStateManager != null)
            {
                int aumento = Mathf.RoundToInt(emotionalStateManager.stress * 0.15f);
                aumento = Mathf.Max(aumento, 1); // minimo 1
                emotionalStateManager.ModifyState(aumento, 0, 0);
            }
        }

        colorSeleccionado = "";
        ActualizarUI();
    }

    // ── CODIGO SECRETO ────────────────────────────────────────────
    private void AbrirPanelCodigo()
    {
        if (panelCodigoSecreto != null) panelCodigoSecreto.SetActive(true);
        if (textoMensajeCodigo != null) textoMensajeCodigo.text = "";
    }

    private void ConfirmarCodigo()
    {
        if (inputCodigo == null) return;
        string ingresado = inputCodigo.text.Trim().ToUpper();

        if (ingresado == CODIGO_MARIO)
        {
            dineroDesbloqueado = true;
            dineroJugador += 100;
            if (textoMensajeCodigo != null) textoMensajeCodigo.text = "Codigo correcto. Recibis $100!";
            if (panelCodigoSecreto != null)
                StartCoroutine(CerrarPanelDespues(2f));
        }
        else
        {
            if (textoMensajeCodigo != null) textoMensajeCodigo.text = "Codigo incorrecto.";
        }

        ActualizarUI();
    }

    private IEnumerator CerrarPanelDespues(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        if (panelCodigoSecreto != null) panelCodigoSecreto.SetActive(false);
    }

    // ── UI ────────────────────────────────────────────────────────
    private void ActualizarUI()
    {
        if (textoDinero  != null) textoDinero.text  = "Dinero: $" + dineroJugador;
        if (textoApuesta != null) textoApuesta.text = "Apuesta: $" + apuestaActual;
    }

    // ── METODO PUBLICO PARA BILLETERA FUTURA ─────────────────────
    /// <summary>
    /// Punto de integracion para la billetera virtual.
    /// Llamar desde la app externa con la cantidad a depositar.
    /// </summary>
    public void RecibirDineroBilletera(int cantidad)
    {
        if (!dineroDesbloqueado) return;
        dineroJugador += cantidad;
        ActualizarUI();
    }
}
