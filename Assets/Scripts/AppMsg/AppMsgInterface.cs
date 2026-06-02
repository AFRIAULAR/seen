using TMPro;
using UnityEngine;

public class AppMsgInterface : MonoBehaviour
{
    [Header("Lista de Chats")]
    [SerializeField] private GameObject pantallaListaChat;
    
    [Header("Chat")]
    [SerializeField] private GameObject pantallaChat;
    [SerializeField] private TextMeshProUGUI nombreContacto;

    [Header("Referencias del Sistema Narrativo")]
    [Tooltip("Arrastra aquí el objeto de tu escena que contiene el script 'ConversationManager'.")]
    [SerializeField] private ConversationManager conversationManager;

    private void OnEnable()
    {
        PanelChatScript.OnPanelActivo += AbrirChat;
    }

    private void OnDisable()
    {
        PanelChatScript.OnPanelActivo -= AbrirChat;
    }

    public void Volver()
    {
        if (CelularManagent.celularInterface.HistorialPantallasApp.Count > 1)
        {
            CelularManagent.celularInterface.RetrocederPantalla();
        }
        else
        {
            CelularManagent.celularInterface.LimpiarHistorial();
            AbrirInicio();
        }
    }

    public void AbrirInicio()
    {
        CelularManagent.celularInterface.CambiarPantalla(pantallaListaChat);
        pantallaChat.SetActive(false);
        pantallaListaChat.SetActive(true);
    }

    public void AbrirChat(string nombre)
    {
        Debug.Log($"[CLICK DETECTADO] Cargando conversación de: {nombre}");
        CelularManagent.celularInterface.CambiarPantalla(pantallaChat);
        
        CargarChat(nombre);

        pantallaListaChat.SetActive(false);
        pantallaChat.SetActive(true);
    }

    public void CargarChat(string nombre)
    {
        nombreContacto.text = nombre;

        if (AppMsgManagent.appMsgManagent != null)
        {
            PersonaData personaSeleccionada = AppMsgManagent.appMsgManagent.BuscarPersonaPorNombre(nombre);

            if (personaSeleccionada != null && conversationManager != null)
            {
                conversationManager.AbrirChatConPersonaje(personaSeleccionada);
            }
        }
    }
}