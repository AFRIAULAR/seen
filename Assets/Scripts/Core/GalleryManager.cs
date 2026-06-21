using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GalleryManager : MonoBehaviour
{
    public static GalleryManager Instance;

    [Header("Botón Galería")]
    [SerializeField] private Button botonGaleria;

    [Header("Popup desbloqueo")]
    [SerializeField] private GameObject popupGaleria;
    [SerializeField] private TMP_Text textoPopup;

    [Header("App Galería")]
    [SerializeField] private GameObject pantallaGaleria;
    [SerializeField] private Image imagenPolaroid;
    [SerializeField] private TMP_Text tituloFinal;
    [SerializeField] private TMP_Text descripcionFinal;

    [Header("Animación final")]
    [SerializeField] private CanvasGroup grupoPolaroid;
    [SerializeField] private CanvasGroup grupoTitulo;
    [SerializeField] private CanvasGroup grupoDescripcion;
    [SerializeField] private CanvasGroup grupoBotonCreditos;
    //[SerializeField] private CanvasGroup grupoTextoRecuerdo;
    //[SerializeField] private TMP_Text textoRecuerdo;

    [Header("Polaroids")]
    [SerializeField] private Sprite polaroidEquilibrio;
    [SerializeField] private Sprite polaroidElegirse;
    [SerializeField] private Sprite polaroidVacioAceptado;
    [SerializeField] private Sprite polaroidColapso;

    [Header("Estado emocional")]
    [SerializeField] private EmotionalStateManager emotionalState;

    private bool galeriaDesbloqueada = false;
    private Coroutine secuenciaActual;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (botonGaleria != null)
            botonGaleria.interactable = false;

        if (popupGaleria != null)
            popupGaleria.SetActive(false);

        if (pantallaGaleria != null)
            pantallaGaleria.SetActive(false);

        PrepararElementosFinal();
    }

    public void DesbloquearGaleria()
    {
        if (galeriaDesbloqueada) return;

        galeriaDesbloqueada = true;

        if (botonGaleria != null)
            botonGaleria.interactable = true;

        if (textoPopup != null)
            textoPopup.text = "Has recuperado tus recuerdos.\nAhora puedes acceder a la galería.";

        if (popupGaleria != null)
            popupGaleria.SetActive(true);

        Debug.Log("Galería desbloqueada desde GalleryManager");
    }

    public void CerrarPopup()
    {
        if (popupGaleria != null)
            popupGaleria.SetActive(false);
    }

    public void AbrirGaleria()
    {
        if (!galeriaDesbloqueada)
        {
            Debug.Log("La galería todavía está bloqueada.");
            return;
        }

        if (pantallaGaleria != null)
            pantallaGaleria.SetActive(true);

        MostrarFinal();

        if (secuenciaActual != null)
            StopCoroutine(secuenciaActual);

        secuenciaActual = StartCoroutine(SecuenciaFinal());
    }

    private void MostrarFinal()
    {
        if (emotionalState == null)
        {
            Debug.LogError("GalleryManager no tiene asignado EmotionalStateManager.");
            return;
        }

        int stress = emotionalState.stress;
        int validation = emotionalState.validation;
        int identity = emotionalState.identity;

        if (stress >= 75)
        {
            AplicarFinal(
                polaroidColapso,
                "Colapso",
                "Intentaste responder a todo, estar para todos y no decepcionar a nadie. Cuando finalmente levantaste la vista de la pantalla, ya no quedaba energía para vos mismo."
            );
        }
        else if (identity >= 70 && validation <= 45)
        {
            AplicarFinal(
                polaroidElegirse,
                "Se eligió a sí mismo",
                "Por primera vez dejaste de buscar tu valor en la mirada de los demás. Las dudas seguían ahí, pero ya no definían quién eras."
            );
        }
        else if (validation >= 70 && identity <= 45)
        {
            AplicarFinal(
                polaroidVacioAceptado,
                "Vacío pero aceptado",
                "Conseguiste la aprobación que buscabas. Las risas, los mensajes y las invitaciones estaban ahí, pero ninguna logró llenar el vacío."
            );
        }
        else
        {
            AplicarFinal(
                polaroidEquilibrio,
                "Equilibrio",
                "Aprendiste a escuchar a los demás sin dejar de escucharte a vos mismo. Las respuestas no aparecieron de golpe, pero encontraste un lugar donde podías estar en paz con ellas."
            );
        }

        Debug.Log($"Final calculado con Stress:{stress} Validation:{validation} Identity:{identity}");
    }

    private void AplicarFinal(Sprite imagen, string titulo, string descripcion)
    {
        if (imagenPolaroid != null)
            imagenPolaroid.sprite = imagen;

        if (tituloFinal != null)
            tituloFinal.text = titulo;

        if (descripcionFinal != null)
            descripcionFinal.text = descripcion;
    }

    private void PrepararElementosFinal()
    {
        SetAlpha(grupoPolaroid, 0);
        SetAlpha(grupoTitulo, 0);
        SetAlpha(grupoDescripcion, 0);
        SetAlpha(grupoBotonCreditos, 0);
       // SetAlpha(grupoTextoRecuerdo, 0);

        if (imagenPolaroid != null)
            imagenPolaroid.transform.localScale = Vector3.one * 0.85f;

        // if (textoRecuerdo != null)
        //     textoRecuerdo.text = "Recuerdo recuperado...";
    }

    private IEnumerator SecuenciaFinal()
    {
        PrepararElementosFinal();

        yield return new WaitForSeconds(0.4f);

       // yield return FadeCanvasGroup(grupoTextoRecuerdo, 0, 1, 0.8f);
       // yield return new WaitForSeconds(1f);
       // yield return FadeCanvasGroup(grupoTextoRecuerdo, 1, 0, 0.5f);

        yield return FadePolaroidConZoom();

        yield return new WaitForSeconds(0.3f);
        yield return FadeCanvasGroup(grupoTitulo, 0, 1, 0.7f);

        yield return new WaitForSeconds(0.2f);
        yield return FadeCanvasGroup(grupoDescripcion, 0, 1, 0.9f);

        yield return new WaitForSeconds(0.2f);
        yield return FadeCanvasGroup(grupoBotonCreditos, 0, 1, 0.6f);
    }

    private IEnumerator FadePolaroidConZoom()
    {
        float duracion = 1.4f;
        float tiempo = 0f;

        Vector3 escalaInicial = Vector3.one * 0.85f;
        Vector3 escalaFinal = Vector3.one;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            SetAlpha(grupoPolaroid, Mathf.Lerp(0, 1, t));

            if (imagenPolaroid != null)
                imagenPolaroid.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);

            yield return null;
        }

        SetAlpha(grupoPolaroid, 1);

        if (imagenPolaroid != null)
            imagenPolaroid.transform.localScale = escalaFinal;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup grupo, float desde, float hasta, float duracion)
    {
        if (grupo == null) yield break;

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            grupo.alpha = Mathf.Lerp(desde, hasta, t);

            yield return null;
        }

        grupo.alpha = hasta;
    }

    private void SetAlpha(CanvasGroup grupo, float alpha)
    {
        if (grupo != null)
            grupo.alpha = alpha;
    }
}