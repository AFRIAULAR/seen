using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.UI;
public class PanelChatScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuración del Chat")]
    [SerializeField] private PersonaData personaData;
    [SerializeField] private Image fotoImage;

    public string nombreChat;

    [Header("Componentes de UI del Panel")]
    [SerializeField] private TMP_Text nombreText;
    [SerializeField] private TMP_Text ultimoMensajeText;

    public static event Action<PersonaData> OnPanelActivo;

    public void InicializarPanel(PersonaData persona)
    {
        if (persona == null) return;

        personaData = persona;

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

        if (fotoImage != null && persona.fotoPerfil != null)
            {
                fotoImage.sprite = persona.fotoPerfil;
            }
    }

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (personaData == null)
        {
            Debug.LogError($"[PanelChatScript] No hay PersonaData asignada en {gameObject.name}");
            return;
        }

        OnPanelActivo?.Invoke(personaData);
    }
}