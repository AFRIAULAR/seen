using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
    [Header("Personaje Actual (Se asigna dinámicamente)")]
    private PersonaData personaActual;

    [Header("Componentes de UI del Chat")]
    [SerializeField] private ScrollRect scrollRectChat;
    [SerializeField] private GameObject msgPrefabNPC;
    [SerializeField] private GameObject msgPrefabPlayer;
    [SerializeField] private EmotionalStateManager emotionalState;
    [SerializeField] private Transform content;
    [SerializeField] private Button[] botonesOpciones;

    public void AbrirChatConPersonaje(PersonaData nuevaPersona)
    {
        personaActual = nuevaPersona;

        foreach (Transform hijo in content)
        {
            Destroy(hijo.gameObject);
        }

        if (personaActual == null) return;

        foreach (int idPasado in personaActual.historialConversacion)
        {
            if (idPasado == -1) continue; 

            if (personaActual.ObtenerLineaPorID(idPasado, out PersonaData.LineaDialogo lineaPasada))
            {
                bool esJugadorPasado = (lineaPasada.hablante.ToUpper() == "JUGADOR" || lineaPasada.hablante.ToUpper() == "YO");
                string nombrePast = esJugadorPasado ? "Yo" : personaActual.nombre;
                
                CrearMensaje($"{nombrePast}: {lineaPasada.texto}", esJugadorPasado);
            }
        }

        ApagarTodosLosBotones();

        ForzarScrollAlFondo();

        if (personaActual.idActual != -1)
        {
            if (personaActual.historialConversacion.Contains(personaActual.idActual))
            {
                if (personaActual.ObtenerLineaPorID(personaActual.idActual, out PersonaData.LineaDialogo lineaActual))
                {
                    if (lineaActual.idSiguiente == -1 && (!string.IsNullOrEmpty(lineaActual.opt1Text) || !string.IsNullOrEmpty(lineaActual.opt2Text)))
                    {
                        ConfigurarBotonesDeDecision(lineaActual);
                        return; 
                    }
                    
                    if (lineaActual.idSiguiente == -1 && string.IsNullOrEmpty(lineaActual.opt1Text) && string.IsNullOrEmpty(lineaActual.opt2Text))
                    {
                        ApagarTodosLosBotones();
                        return;
                    }
                }
            }

            CargarLinea(personaActual.idActual);
        }
        else
        {
            Debug.Log($"[CHAT] Conversación concluida de forma absoluta.");
        }
    }

    private void CargarLinea(int id)
    {
        if (id == -1)
        {
            FinalizarConversacion();
            return;
        }

        if (personaActual == null || !personaActual.ObtenerLineaPorID(id, out PersonaData.LineaDialogo linea))
        {
            FinalizarConversacion();
            return;
        }

        personaActual.idActual = id;
        personaActual.GuardarEnHistorial(id);

        bool esJugador = (linea.hablante.ToUpper() == "JUGADOR" || linea.hablante.ToUpper() == "YO");
        string nombreMostrar = esJugador ? "Yo" : personaActual.nombre;
        
        CrearMensaje($"{nombreMostrar}: {linea.texto}", esJugador);
        ApagarTodosLosBotones();

        if (linea.idSiguiente != -1)
        {
            if (personaActual.ObtenerLineaPorID(linea.idSiguiente, out PersonaData.LineaDialogo proximaLinea))
            {
                bool proximoEsJugador = (proximaLinea.hablante.ToUpper() == "JUGADOR" || proximaLinea.hablante.ToUpper() == "YO");
                
                if (proximoEsJugador) CargarLinea(linea.idSiguiente); 
                else StartCoroutine(EsperarFlujoAutomatico(linea.idSiguiente)); 
            }
            else
            {
                CargarLinea(linea.idSiguiente);
            }
        }
        else if (!string.IsNullOrEmpty(linea.opt1Text) || !string.IsNullOrEmpty(linea.opt2Text))
        {
            ConfigurarBotonesDeDecision(linea);
        }
        else
        {
            ApagarTodosLosBotones();
            Debug.Log($"[CHAT] Llegamos al final del guion con {personaActual.nombre}. Modo lectura.");
        }
    }

    private void ConfigurarBotonesDeDecision(PersonaData.LineaDialogo linea)
    {
        ApagarTodosLosBotones();

        if (!string.IsNullOrEmpty(linea.opt1Text)) SetBoton(0, linea.opt1Text, linea.dest1, linea.mod1);
        if (!string.IsNullOrEmpty(linea.opt2Text)) SetBoton(1, linea.opt2Text, linea.dest2, linea.mod2);
        if (!string.IsNullOrEmpty(linea.opt3Text)) SetBoton(2, linea.opt3Text, linea.dest3, linea.mod3);
        if (!string.IsNullOrEmpty(linea.opt4Text)) SetBoton(3, linea.opt4Text, linea.dest4, linea.mod4);
        
        ForzarScrollAlFondo();
    }

    private void SetBoton(int indiceBoton, string textoOpcion, int destinoID, string modificadores)
    {
        if (indiceBoton >= botonesOpciones.Length || botonesOpciones[indiceBoton] == null) return;

        botonesOpciones[indiceBoton].gameObject.SetActive(true);
        botonesOpciones[indiceBoton].GetComponentInChildren<TMP_Text>().text = textoOpcion;
        
        botonesOpciones[indiceBoton].onClick.RemoveAllListeners();
        botonesOpciones[indiceBoton].onClick.AddListener(() => SeleccionarOpcion(destinoID, modificadores));
    }

    private void SeleccionarOpcion(int destinoID, string modificadores)
    {
        AplicarCambiosEmocionales(modificadores);
        ApagarTodosLosBotones();

        if (destinoID == -1)
        {
            FinalizarConversacion();
        }
        else
        {
            if (personaActual.ObtenerLineaPorID(destinoID, out PersonaData.LineaDialogo proximaLinea))
            {
                bool proximoEsJugador = (proximaLinea.hablante.ToUpper() == "JUGADOR" || proximaLinea.hablante.ToUpper() == "YO");
                
                if (proximoEsJugador)
                {
                    CargarLinea(destinoID);
                }
                else
                {
                    StartCoroutine(EsperarFlujoAutomatico(destinoID));
                }
            }
            else
            {
                CargarLinea(destinoID);
            }
        }
    }

    private void FinalizarConversacion()
    {
        personaActual.idActual = -1;
        personaActual.GuardarEnHistorial(-1); 
        ApagarTodosLosBotones();
        Debug.Log($"[CHAT] Conversación finalizada por completo.");
        ForzarScrollAlFondo();
    }

    private void AplicarCambiosEmocionales(string modTexto)
    {
        if (string.IsNullOrEmpty(modTexto) || personaActual == null) return;

        string[] valores = modTexto.Split(',');
        if (valores.Length == 3)
        {
            emotionalState.ModifyState(int.Parse(valores[0]), int.Parse(valores[1]), int.Parse(valores[2]));
        }
    }

    private IEnumerator EsperarFlujoAutomatico(int siguienteID)
    {
        yield return new WaitForSeconds(1.3f);
        CargarLinea(siguienteID);
    }

    private void ApagarTodosLosBotones()
    {
        foreach (Button btn in botonesOpciones)
        {
            if (btn != null)
            {
                btn.gameObject.SetActive(false);
                TMP_Text t = btn.GetComponentInChildren<TMP_Text>();
                if (t != null) t.text = "";
            }
        }
    }

    private void CrearMensaje(string texto, bool esJugador)
    {
        if (content == null) return;

        if (esJugador)
        {
            CrearEspacioVacio();

            if (msgPrefabPlayer != null)
            {
                GameObject nuevoMsg = Instantiate(msgPrefabPlayer, content);
                ConfigurarTexto(nuevoMsg, texto);
            }
        }
        else
        {
            if (msgPrefabNPC != null)
            {
                GameObject nuevoMsg = Instantiate(msgPrefabNPC, content);
                ConfigurarTexto(nuevoMsg, texto);
            }

            CrearEspacioVacio();
        }
        
        Canvas.ForceUpdateCanvases();
        
        ForzarScrollAlFondo();
    }

    private void ConfigurarTexto(GameObject go, string texto)
    {
        TMP_Text textoMsg = go.GetComponentInChildren<TMP_Text>();
        if (textoMsg != null)
        {
            textoMsg.text = texto;
        }
    }

    private void CrearEspacioVacio()
    {
        GameObject fantasma = new GameObject("EspacioVacioGrid", typeof(RectTransform));
        fantasma.transform.SetParent(content, false);
    }
    
    private void ForzarScrollAlFondo()
    {
        if (scrollRectChat != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(BajarScrollGarantizado());
        }
    }

    private IEnumerator BajarScrollGarantizado()
    {
        yield return new WaitForEndOfFrame();
        
        if (scrollRectChat != null)
        {
            scrollRectChat.verticalNormalizedPosition = 0f;
        }
    }
}