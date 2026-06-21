using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiaryManager : MonoBehaviour
{
    [Header("Estado emocional")]
    [SerializeField] private EmotionalStateManager emotionalState;

    [Header("UI")]
    [SerializeField] private TMP_Text textoEntradaSeleccionada;
    [SerializeField] private TMP_Text textoFeedback;
    [SerializeField] private Button botonReflexionar;

    private int selectedOption = -1;
    private bool[] reflexionesUsadas = new bool[3];

    private void Start()
    {
        selectedOption = -1;

        if (textoEntradaSeleccionada != null)
            textoEntradaSeleccionada.text = "Seleccioná una entrada.";

        if (textoFeedback != null)
            textoFeedback.text = "";

        if (botonReflexionar != null)
            botonReflexionar.interactable = false;
    }

    public void SelectText(int option)
    {
        selectedOption = option;

        if (reflexionesUsadas[option])
        {
            if (textoFeedback != null)
                textoFeedback.text = "Ya reflexionaste sobre esta entrada.";

            if (botonReflexionar != null)
                botonReflexionar.interactable = false;

            return;
        }

        if (textoEntradaSeleccionada != null)
            textoEntradaSeleccionada.text = ObtenerTextoEntrada(option);

        if (textoFeedback != null)
            textoFeedback.text = "Podés reflexionar sobre esto.";

        if (botonReflexionar != null)
            botonReflexionar.interactable = true;
    }

    public void Reflexionar()
    {
        if (selectedOption == -1)
        {
            if (textoFeedback != null)
                textoFeedback.text = "Seleccioná una entrada del diario primero.";
            return;
        }

        if (reflexionesUsadas[selectedOption])
        {
            if (textoFeedback != null)
                textoFeedback.text = "Esta reflexión ya fue usada.";
            return;
        }

        switch (selectedOption)
        {
            case 0:
                // "No puedo con todo"
                emotionalState.ModifyState(-10, 0, +5);
                MostrarFeedback("Respiraste un poco. Baja el estrés, pero todavía dudás de vos.");
                break;

            case 1:
                // "Tengo que estar para todos"
                emotionalState.ModifyState(+10, +10, -10);
                MostrarFeedback("Pensar en los demás te da validación, pero te carga más presión.");
                break;

            case 2:
                // "Hoy necesito elegirme"
                emotionalState.ModifyState(+5, -10, +15);
                MostrarFeedback("Elegirte te fortalece, aunque te aleja un poco de la aprobación externa.");
                break;
        }

        reflexionesUsadas[selectedOption] = true;

        if (botonReflexionar != null)
            botonReflexionar.interactable = false;

        if (MemoryManager.Instance != null)
            {
                MemoryManager.Instance.MarcarDiarioUsado();
            }
    }

    private string ObtenerTextoEntrada(int option)
    {
        switch (option)
        {
            case 0:
                return "No puedo con todo.";

            case 1:
                return "Tengo que estar para todos.";

            case 2:
                return "Hoy necesito elegirme.";

            default:
                return "";
        }
    }

    private void MostrarFeedback(string mensaje)
    {
        if (textoFeedback != null)
            textoFeedback.text = mensaje;
    }
}