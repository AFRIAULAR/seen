using TMPro;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    [SerializeField] private GameObject msgPrefab;
    [SerializeField] private GameObject msgPrefabPlayer;
    [SerializeField] private Transform content;
    [SerializeField] private ReactiveUIManager reactiveUI;

    [Header("Textos de respuesta")]
    [SerializeField] private string respuestaBuena = "Sí, obvio. Contame qué pasó.";
    [SerializeField] private string respuestaNeutra = "Ahora estoy un poco ocupado, pero te leo.";
    [SerializeField] private string respuestaMala = "No puedo hacerme cargo de esto ahora.";

    private void Start()
    {
        CrearMensaje("Mamá: ¿Podés hablar? Necesito contarte algo.", false);
    }

    public void ResponderBuena()
    {
        CrearMensaje("Yo: " + respuestaBuena, true);
        reactiveUI.AddStress(5);
    }

    public void ResponderNeutra()
    {
        CrearMensaje("Yo: " + respuestaNeutra, true);
        reactiveUI.AddStress(15);
    }

    public void ResponderMala()
    {
        CrearMensaje("Yo: " + respuestaMala, true);
        reactiveUI.AddStress(30);
    }

    private void CrearMensaje(string texto, bool esJugador)
    {
        GameObject prefab = esJugador ? msgPrefabPlayer : msgPrefab;

        GameObject nuevoMsg = Instantiate(prefab, content);

        TMP_Text textoMsg = nuevoMsg.GetComponentInChildren<TMP_Text>();

        textoMsg.text = texto;
    }
}