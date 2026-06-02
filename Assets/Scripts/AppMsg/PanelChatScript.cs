using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class PanelChatScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuración del Chat (Se carga por código)")]
    public string nombreChat;

    [Header("Componentes de UI del Panel")]
    [SerializeField] private TMP_Text nombreText;
    [SerializeField] private TMP_Text ultimoMensajeText;

    public static event Action<string> OnPanelActivo;

    /// <summary>
    /// Método para inicializar el panel dinámicamente desde el AppMsgManagent
    /// </summary>
    public void InicializarPanel(PersonaData persona)
    {
        if (persona == null) return;

        nombreChat = persona.nombre;
        if (nombreText != null) nombreText.text = nombreChat;

        persona.InicializarGuion();

        if (ultimoMensajeText != null)
        {
            if (persona.ObtenerLineaPorID(persona.idActual, out PersonaData.LineaDialogo linea))
            {
                string textoCorto = linea.texto;
                if (textoCorto.Length > 35) textoCorto = textoCorto.Substring(0, 35) + "...";
                ultimoMensajeText.text = textoCorto;
            }
            else
            {
                ultimoMensajeText.text = "No hay mensajes recientes.";
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"[HOVER] El mouse ENTRÓ al panel: {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"[HOVER] El mouse SALIÓ del panel: {gameObject.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPanelActivo?.Invoke(nombreChat);
    }
}