using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Paneles y Contenedores")]
    [SerializeField] private GameObject panelGallery;
    [SerializeField] private GameObject panelCredits;
    [Tooltip("El contenedor o máscara transparente que limita la vista de los créditos.")]
    [SerializeField] private RectTransform viewportContenedor; 

    [Header("Créditos")]
    [SerializeField] private RectTransform creditsContent;
    [SerializeField] private Button botonSalir;

    [Header("Configuración")]
    [SerializeField] private float velocidad = 45f;
    [SerializeField] private string nombreEscenaMenu = "Menu";

    [Header("UI fija")]
    [SerializeField] private GameObject interfazFija;

    [SerializeField] private AppMusicaManagent appmusica;

    private float posicionInicialY;
    private float posicionFinalY;
    private Coroutine corrutinaCreditos;

    private void Start()
    {
        if (panelCredits != null)
            panelCredits.SetActive(false);

        if (botonSalir != null)
            botonSalir.gameObject.SetActive(false);
    }

    public void AbrirCreditos()
    {
        GameManagent.gameInstancia.DetenerAmbiente();
        Debug.Log("AbrirCreditos llamado con Corrutina y cálculo adaptativo.");

        if (panelCredits != null) panelCredits.SetActive(true);
        if (panelGallery != null) panelGallery.SetActive(false);
        if (interfazFija != null) interfazFija.SetActive(false);
        if (botonSalir != null) botonSalir.gameObject.SetActive(false);

        if (viewportContenedor != null && creditsContent != null)
        {
            Canvas.ForceUpdateCanvases();

            float altoViewport = viewportContenedor.rect.height;
            float altoTexto = creditsContent.rect.height;

            posicionInicialY = -(altoViewport / 2f) - (altoTexto / 2f);
            posicionFinalY = (altoViewport / 2f) + (altoTexto / 2f);

            creditsContent.anchoredPosition = new Vector2(creditsContent.anchoredPosition.x, posicionInicialY);
            
            if (corrutinaCreditos != null)
            {
                StopCoroutine(corrutinaCreditos);
            }

            corrutinaCreditos = StartCoroutine(MoverCreditos());
        }
        else
        {
            Debug.LogError("[CreditsManager] Falta asignar el Viewport o el CreditsContent en el Inspector.");
        }
    }

    private IEnumerator MoverCreditos()
    {
        while (creditsContent.anchoredPosition.y < posicionFinalY)
        {
            creditsContent.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

            yield return null;
        }

        if (botonSalir != null)
            botonSalir.gameObject.SetActive(true);

        corrutinaCreditos = null;
    }

    public void SalirAlMenu()
    {
        appmusica.DetenerMusica(); 
        if (corrutinaCreditos != null)
        {
            StopCoroutine(corrutinaCreditos);
        }

        SceneManager.LoadScene(nombreEscenaMenu);
    }
}