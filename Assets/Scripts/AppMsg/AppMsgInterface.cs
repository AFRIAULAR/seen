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

    public void AbrirChat(PersonaData persona)
    {
        if (persona == null)
        {
            Debug.LogError("[AppMsgInterface] PersonaData llegó null.");
            return;
        }

        Debug.Log($"[CLICK DETECTADO] Cargando conversación de: {persona.nombre}");

        CelularManagent.celularInterface.CambiarPantalla(pantallaChat);

        nombreContacto.text = persona.nombre;

        pantallaListaChat.SetActive(false);
        pantallaChat.SetActive(true);

        if (conversationManager != null)
        {
            conversationManager.AbrirChatConPersonaje(persona);
        }
    }
}