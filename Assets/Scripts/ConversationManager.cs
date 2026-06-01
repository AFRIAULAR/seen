using TMPro;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private GameObject msgPrefab;
    [SerializeField] private GameObject msgPrefabPlayer;
    [SerializeField] private Transform content;
    [SerializeField] private EmotionalStateManager emotionalState;

    [Header("Textos de respuesta")]
    [SerializeField] private string Complaciente = "Sí, obvio. Contame qué pasó.";
    [SerializeField] private string PonerLimite = "Ahora estoy un poco ocupado, pero te leo.";
    [SerializeField] private string Evasiva = "No puedo hacerme cargo de esto ahora.";

    private void Start()
    {
        CrearMensaje("Mamá: ¿Podés hablar? Necesito contarte algo.", false);
    }

    public void RespComplaciente()
    {
        CrearMensaje("Yo: " + Complaciente, true);
        emotionalState.ModifyState(15, 15, -10);
    }

    public void RespLimite()
    {
        CrearMensaje("Yo: " + PonerLimite, true);
        emotionalState.ModifyState(5, 5, 5);
    }

    public void RespEvasiva()
    {
        CrearMensaje("Yo: " + Evasiva, true);
        emotionalState.ModifyState(-5, -15, 15);
    }

    private void CrearMensaje(string texto, bool esJugador)
    {
        GameObject prefab = esJugador ? msgPrefabPlayer : msgPrefab;

        GameObject nuevoMsg = Instantiate(prefab, content);

        TMP_Text textoMsg = nuevoMsg.GetComponentInChildren<TMP_Text>();

        textoMsg.text = texto;
    }
}