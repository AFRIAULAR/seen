using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// VERSIÓN MODIFICADA de EmotionalStateManager.
/// Cambios respecto al original:
///   - Ahora tiene una referencia a MusicaApp
///   - Cuando el estrés llega al 80% o más, muestra el pop-up de música
///   - Solo muestra el pop-up UNA VEZ por "episodio" (no spam)
/// 
public class EmotionalStateManager : MonoBehaviour
{
    [SerializeField] private ReactiveUIManager reactiveUI;

    [SerializeField] private TMP_Text stressText;
    [SerializeField] private TMP_Text validationText;
    [SerializeField] private TMP_Text identityText;

    [Header("Estados emocionales")]
    [Range(0, 100)] public int stress = 20;
    [Range(0, 100)] public int validation = 50;
    [Range(0, 100)] public int identity = 50;

    [Header("Pop-up de Estrés Alto")]
    [Tooltip("El panel del pop-up que avisa sobre el estrés alto")]
    [SerializeField] private GameObject popUpEstres;
    [Tooltip("Botón 'OK' o 'Aceptar' dentro del pop-up")]
    [SerializeField] private Button botonAceptarPopUp;
    [Tooltip("Botón del ícono de música (para activar/desactivar el click)")]
    [SerializeField] private Button botonIconoMusica;
    private bool popUpMostrado = false;
    private const int UMBRAL_ESTRES = 80;

    private void Start()
    {
        UpdateHUD();
       // reactiveUI.SetStress(stress);
        reactiveUI.SetEmotionalState(stress, validation, identity);
        
        if (botonIconoMusica != null) botonIconoMusica.interactable = false;
        if (botonAceptarPopUp != null)
            botonAceptarPopUp.onClick.AddListener(AlAceptarPopUp);
    }

    public void ModifyState(int stressChange, int validationChange, int identityChange)
    {
        stress = Mathf.Clamp(stress + stressChange, 0, 100);
        validation = Mathf.Clamp(validation + validationChange, 0, 100);
        identity = Mathf.Clamp(identity + identityChange, 0, 100);

        Debug.Log(
        $"Stress: {stress} | Validation: {validation} | Identity: {identity}");

        //reactiveUI.SetStress(stress);
        reactiveUI.SetEmotionalState(stress, validation, identity);

        // Verificar si el estrés superó el umbral
        VerificarEstresAlto();

        UpdateHUD();
    }

    // Método que revisa si hay que mostrar el pop-up
    private void VerificarEstresAlto()
    {
        if (stress >= UMBRAL_ESTRES && !popUpMostrado)
        {
            popUpMostrado = true;
            popUpEstres.SetActive(true);
            Debug.Log("[EmotionalStateManager] Estrés superó el 80%, mostrando pop-up de música.");
        }

        // OPTIMIZACIÓN: Solo reseteamos el pop-up si el estrés bajó considerablemente (ej: al 60%)
        // Esto evita que si oscila entre 79% y 80% te salte el cartel en la cara a cada rato.
        if (stress < 60)
        {
            popUpMostrado = false;
        }
    }

    private void AlAceptarPopUp()
    {
        if (popUpEstres != null) popUpEstres.SetActive(false);

        if (botonIconoMusica != null) botonIconoMusica.interactable = true;
        Debug.Log("[MusicaApp] App de música desbloqueada.");
    }

    private void UpdateHUD()
    {
        stressText.text = stress + "%";
        validationText.text = validation + "%";
        identityText.text = identity + "%";
    }
}