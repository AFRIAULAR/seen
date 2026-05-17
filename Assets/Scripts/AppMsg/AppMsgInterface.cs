
using TMPro;
using UnityEngine;
// Memo: Al trabajar con multiples paneles, algunos pueden ocupar completamente toda la pantalla y ocacionar que no se puedan interactuar con algunos elementos detras de estos, en esos caso revisar en los componente Image el atributo Raycast Target, esto permite la interaccion con cada elemento.
public class AppMsgInterface : MonoBehaviour
{
    [Header("Lista de Chats")]
    [SerializeField]private GameObject pantallaListaChat;
    [Header("Chat")]
    [SerializeField]private GameObject pantallaChat;
    [SerializeField]private TextMeshProUGUI nombreContacto;
    private void OnEnable()
    {
        PanelChatScript.OnPanelActivo += AbrirChat;
        CelularManagent.celularInstancia.CambiarPantalla(pantallaListaChat);
    }
    private void OnDisable()
    {
        PanelChatScript.OnPanelActivo -= AbrirChat;
    }
    public void Volver()
    {
        // CargarListaChat();
        
        CelularManagent.celularInstancia.LimpiarHistorial();
        CelularManagent.celularInstancia.CambiarPantalla(pantallaListaChat);
        pantallaChat.SetActive(false);
        pantallaListaChat.SetActive(true);
    }
    public void AbrirChat(string nombre)
    {
        Debug.Log($"[CLICK DETECTADO] Cargando conversación de: {nombre}");
        CelularManagent.celularInstancia.CambiarPantalla(pantallaChat);
        CargarChat(nombre);
        pantallaListaChat.SetActive(false);
        pantallaChat.SetActive(true);
    }
    public void CargarListaChat()
    {
        // Deberia cargar la lista de chat de forma dinamica, puede que este metodo deba ir al AppMsgManagent
    }
    public void CargarChat(string nombre)
    {
        nombreContacto.text = nombre;
    }
}
