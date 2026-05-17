using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class PanelChatScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Se debe de cargar de forma dinamica la informacion correspondiente por codigo
    public string nombreChat;
    [SerializeField] private TMP_Text nombreText;
    public static event Action<string> OnPanelActivo;
    private void Awake()
    {
        nombreText.text = nombreChat;
    }
    public void InicializarPanel()
    {
        if (nombreText != null) nombreText.text = nombreChat;
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
