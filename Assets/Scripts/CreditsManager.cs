using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelGallery;
    [SerializeField] private GameObject panelCredits;

    [Header("Créditos")]
    [SerializeField] private RectTransform creditsContent;
    [SerializeField] private Button botonSalir;

    [Header("Configuración")]
    [SerializeField] private float velocidad = 45f;
    [SerializeField] private float posicionInicialY = -700f;
    [SerializeField] private float posicionFinalY = 900f;
    [SerializeField] private string nombreEscenaMenu = "Menu";

    [Header("UI fija")]
    [SerializeField] private GameObject interfazFija;

    private bool reproduciendo = false;

    private void Start()
    {
        if (panelCredits != null)
            panelCredits.SetActive(false);

        if (botonSalir != null)
            botonSalir.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!reproduciendo || creditsContent == null) return;

        creditsContent.anchoredPosition += Vector2.up * velocidad * Time.deltaTime;

        if (creditsContent.anchoredPosition.y >= posicionFinalY)
        {
            reproduciendo = false;

            if (botonSalir != null)
                botonSalir.gameObject.SetActive(true);
        }
    }

    public void AbrirCreditos()
{
    Debug.Log("AbrirCreditos llamado");

    if (panelCredits != null)
        panelCredits.SetActive(true);
    else
        Debug.LogError("PanelCredits no asignado");

    if (panelGallery != null)
        panelGallery.SetActive(false);

    if (botonSalir != null)
        botonSalir.gameObject.SetActive(false);

    if (creditsContent != null)
        creditsContent.anchoredPosition = new Vector2(
            creditsContent.anchoredPosition.x,
            posicionInicialY
        );
    if (interfazFija != null)
    interfazFija.SetActive(false);

    reproduciendo = true;
}

    public void SalirAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}