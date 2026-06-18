using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Botón Entrar")]
    public CanvasGroup enterButtonGroup;
    public RectTransform enterButton;
    public float pulseSpeed = 2f;
    public float minAlpha = 0.45f;
    public float maxAlpha = 1f;

    [Header("Logo Glitch")]
    public RectTransform logo;
    public float glitchDelay = 6f;
    public float glitchIntensity = 6f;

    [Header("Paneles")]
    public GameObject panelCreditos;

    private Vector2 logoOriginalPosition;

    private void Start()
    {
        if (panelCreditos != null)
            panelCreditos.SetActive(false);

        if (logo != null)
        {
            logoOriginalPosition = logo.anchoredPosition;
            StartCoroutine(LogoGlitchLoop());
        }
    }

    private void Update()
    {
        AnimateEnterButton();
    }

    private void AnimateEnterButton()
    {
        if (enterButtonGroup == null) return;

        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        enterButtonGroup.alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        if (enterButton != null)
        {
            float scale = Mathf.Lerp(0.98f, 1.02f, t);
            enterButton.localScale = Vector3.one * scale;
        }
    }

    private IEnumerator LogoGlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(glitchDelay);

            for (int i = 0; i < 6; i++)
            {
                logo.anchoredPosition = logoOriginalPosition + Random.insideUnitCircle * glitchIntensity;
                yield return new WaitForSeconds(0.03f);
            }

            logo.anchoredPosition = logoOriginalPosition;
        }
    }

    public void MostrarCreditos()
    {
        panelCreditos.SetActive(true);
    }

    public void OcultarCreditos()
    {
        panelCreditos.SetActive(false);
    }

    public void GoToPhone()
    {
        SceneManager.LoadScene("Joel 2");
    }
}