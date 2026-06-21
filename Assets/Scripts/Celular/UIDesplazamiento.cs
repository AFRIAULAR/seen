using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class UIDesplazamiento : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum DireccionDespliegue { Arriba, Abajo, Izquierda, Derecha }

    [Header("Configuración de Dirección")]
    [SerializeField] private DireccionDespliegue direccion = DireccionDespliegue.Arriba;

    [Header("Configuración de Animación")]
    [SerializeField] private float velocidadCambio = 15f;

    [Header("Garantía de Colisión")]
    [Range(0.01f, 0.1f)]
    [SerializeField] private float escalaMinimaOculta = 0.03f; 

    private RectTransform miRectTransform;
    private CanvasGroup miCanvasGroup;
    private Coroutine corrutinaAnimacion;

    private Vector3 escalaOriginalTope;
    private Vector3 escalaOcultaMinima;

    void Awake()
    {
        miRectTransform = GetComponent<RectTransform>();
        miCanvasGroup = GetComponent<CanvasGroup>();

        // Forzamos el pivote correcto según la dirección elegida ANTES de guardar la escala
        AjustarPivoteSegunDireccion();

        escalaOriginalTope = miRectTransform.localScale;
        escalaOcultaMinima = CalcularEscalaOculta();

        // Configuración inicial (Oculto)
        miCanvasGroup.alpha = 0f;
        miCanvasGroup.blocksRaycasts = true; 
        miCanvasGroup.interactable = false; 
        
        miRectTransform.localScale = escalaOcultaMinima;
    }

    private void AjustarPivoteSegunDireccion()
    {
        // Modificar el pivote por código puede mover el elemento si no se hace con cuidado.
        // Lo ideal es que configures el pivote a mano en el inspector, pero este switch lo asegura:
        switch (direccion)
        {
            case DireccionDespliegue.Arriba:    miRectTransform.pivot = new Vector2(0.5f, 0f);  break;
            case DireccionDespliegue.Abajo:     miRectTransform.pivot = new Vector2(0.5f, 1f);  break;
            case DireccionDespliegue.Izquierda: miRectTransform.pivot = new Vector2(1f, 0.5f); break;
            case DireccionDespliegue.Derecha:   miRectTransform.pivot = new Vector2(0f, 0.5f); break;
        }
    }

    private Vector3 CalcularEscalaOculta()
    {
        switch (direccion)
        {
            case DireccionDespliegue.Arriba:
            case DireccionDespliegue.Abajo:
                return new Vector3(escalaOriginalTope.x, escalaOriginalTope.y * escalaMinimaOculta, escalaOriginalTope.z);
                
            case DireccionDespliegue.Izquierda:
            case DireccionDespliegue.Derecha:
                return new Vector3(escalaOriginalTope.x * escalaMinimaOculta, escalaOriginalTope.y, escalaOriginalTope.z);
                
            default:
                return escalaOriginalTope;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CambiarEstado(1f, escalaOriginalTope, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CambiarEstado(0f, escalaOcultaMinima, false);
    }

    private void CambiarEstado(float alphaDestino, Vector3 escalaDestino, bool activarInteracciones)
    {
        miCanvasGroup.interactable = activarInteracciones;

        if (corrutinaAnimacion != null)
        {
            StopCoroutine(corrutinaAnimacion);
        }

        corrutinaAnimacion = StartCoroutine(AnimarAperturaSuave(alphaDestino, escalaDestino));
    }

    private IEnumerator AnimarAperturaSuave(float alphaDestino, Vector3 escalaDestino)
    {
        miCanvasGroup.blocksRaycasts = true;
        float distanciaInicial = Vector3.Distance(miRectTransform.localScale, escalaDestino);

        while (Vector3.Distance(miRectTransform.localScale, escalaDestino) > 0.001f)
        {
            miRectTransform.localScale = Vector3.Lerp(miRectTransform.localScale, escalaDestino, Time.deltaTime * velocidadCambio);
            
            float distanceActual = Vector3.Distance(miRectTransform.localScale, escalaDestino);
            float progreso = distanciaInicial > 0f ? (1f - (distanceActual / distanciaInicial)) : 1f;
            
            miCanvasGroup.alpha = (alphaDestino == 1f) ? progreso : (1f - progreso);

            yield return null;
        }

        miRectTransform.localScale = escalaDestino;
        miCanvasGroup.alpha = alphaDestino;
    }
}