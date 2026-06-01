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
    [Tooltip("El grosor de la base cuando está cerrado. No usar 0 para que el mouse pueda volver a entrar.")]
    [SerializeField] private float escalaMinimaOculta = 0.03f; 

    private RectTransform miRectTransform;
    private CanvasGroup miCanvasGroup;
    private Coroutine corrutinaAnimacion;

    // Respaldos exactos de tu configuración manual en el Inspector
    private Vector3 posicionOriginalTope;
    private Vector3 escalaOriginalTope;
    
    private Vector3 escalaOcultaMinima;
    private Vector3 posicionOcultaMinima;

    void Awake()
    {
        miRectTransform = GetComponent<RectTransform>();
        miCanvasGroup = GetComponent<CanvasGroup>();

        // 1. RESPALDAMOS TU CONFIGURACIÓN MANUAL INTACTA (Este es tu 100% abierto)
        posicionOriginalTope = miRectTransform.localPosition;
        escalaOriginalTope = miRectTransform.localScale;

        // 2. CALCULAMOS LOS ESTADOS DE CIERRE SIN ALTERAR TU PIVOTE
        escalaOcultaMinima = CalcularEscalaOculta();
        posicionOcultaMinima = CalcularPosicionOculta();

        // 3. CONFIGURACIÓN DE INICIO SEGURA
        miCanvasGroup.alpha = 0f;
        miCanvasGroup.blocksRaycasts = true; 
        miCanvasGroup.interactable = false; 
        
        // Inicializamos en el estado oculto calibrado
        miRectTransform.localScale = escalaOcultaMinima;
        miRectTransform.localPosition = posicionOcultaMinima;
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

    /// <summary>
    /// Calcula el sutil desfase de posición necesario para que el panel aparente 
    /// encogerse hacia la dirección correcta usando tu propio pivote como base.
    /// </summary>
    private Vector3 CalcularPosicionOculta()
    {
        // Obtenemos el tamaño real en píxeles del panel
        float ancho = miRectTransform.rect.width * escalaOriginalTope.x;
        float alto = miRectTransform.rect.height * escalaOriginalTope.y;
        
        float factorReduccionY = alto * (1f - escalaMinimaOculta);
        float factorReduccionX = ancho * (1f - escalaMinimaOculta);

        Vector3 offset = Vector3.zero;

        // Compensamos el movimiento del eje según el pivote que tú ya pusiste
        switch (direccion)
        {
            case DireccionDespliegue.Arriba:
                // Se encoge hacia su base inferior
                offset.y = -factorReduccionY * (1f - miRectTransform.pivot.y);
                break;
            case DireccionDespliegue.Abajo:
                // Se encoge hacia su tope superior
                offset.y = factorReduccionY * miRectTransform.pivot.y;
                break;
            case DireccionDespliegue.Izquierda:
                // Se encoge hacia su borde derecho
                offset.x = -factorReduccionX * (1f - miRectTransform.pivot.x);
                break;
            case DireccionDespliegue.Derecha:
                // Se encoge hacia su borde izquierdo
                offset.x = factorReduccionX * miRectTransform.pivot.x;
                break;
        }

        return posicionOriginalTope + offset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CambiarEstado(1f, escalaOriginalTope, posicionOriginalTope, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CambiarEstado(0f, escalaOcultaMinima, posicionOcultaMinima, false);
    }

    private void CambiarEstado(float alphaDestino, Vector3 escalaDestino, Vector3 posicionDestino, bool activarInteracciones)
    {
        miCanvasGroup.interactable = activarInteracciones;

        if (corrutinaAnimacion != null)
        {
            StopCoroutine(corrutinaAnimacion);
        }

        corrutinaAnimacion = StartCoroutine(AnimarAperturaSuave(alphaDestino, escalaDestino, posicionDestino));
    }

    private IEnumerator AnimarAperturaSuave(float alphaDestino, Vector3 escalaDestino, Vector3 posicionDestino)
    {
        miCanvasGroup.blocksRaycasts = true;
        float distanciaInicial = Vector3.Distance(miRectTransform.localScale, escalaDestino);

        while (Vector3.Distance(miRectTransform.localScale, escalaDestino) > 0.001f || Vector3.Distance(miRectTransform.localPosition, posicionDestino) > 0.01f)
        {
            // Transición gemela: Escala y posición se mueven en perfecta sincronía
            miRectTransform.localScale = Vector3.Lerp(miRectTransform.localScale, escalaDestino, Time.deltaTime * velocidadCambio);
            miRectTransform.localPosition = Vector3.Lerp(miRectTransform.localPosition, posicionDestino, Time.deltaTime * velocidadCambio);
            
            // Cálculo del progreso para el desvanecimiento
            float distanceActual = Vector3.Distance(miRectTransform.localScale, escalaDestino);
            float progreso = distanciaInicial > 0f ? (1f - (distanceActual / distanciaInicial)) : 1f;
            
            miCanvasGroup.alpha = (alphaDestino == 1f) ? progreso : (1f - progreso);

            yield return null;
        }

        miRectTransform.localScale = escalaDestino;
        miRectTransform.localPosition = posicionDestino;
        miCanvasGroup.alpha = alphaDestino;
    }
}