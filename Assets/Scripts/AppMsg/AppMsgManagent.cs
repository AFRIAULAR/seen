using UnityEngine;

public class AppMsgManagent : MonoBehaviour
{
    public static AppMsgManagent appMsgManagent;

    [Header("Base de Datos de Personajes")]
    [Tooltip("Agregá acá todos los ScriptableObjects de tus personas desde el Inspector.")]
    [SerializeField] private PersonaData[] listaPersonas;

    [Header("Configuración de Interfaz automática")]
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private Transform contenedorLayoutGroup;

    void Awake()
    {
        if (appMsgManagent == null)
        {
            appMsgManagent = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerarPanelesDeChat();
    }

    /// <summary>
    /// Genera dinámicamente un panel en el Vertical Layout Group por cada persona en la lista.
    /// </summary>
    public void GenerarPanelesDeChat()
    {
        if (contenedorLayoutGroup == null || panelPrefab == null) return;

        foreach (Transform hijo in contenedorLayoutGroup) Destroy(hijo.gameObject);

        foreach (PersonaData persona in listaPersonas)
        {
            if (persona == null) continue;

            GameObject nuevoPanel = Instantiate(panelPrefab, contenedorLayoutGroup);
            PanelChatScript scriptPanel = nuevoPanel.GetComponent<PanelChatScript>();
            
            if (scriptPanel != null)
            {
                scriptPanel.InicializarPanel(persona);
            }
        }
    }

    /// <summary>
    /// Busca un personaje en la lista del inspector usando su nombre.
    /// </summary>
    public PersonaData BuscarPersonaPorNombre(string nombreABuscar)
    {
        foreach (PersonaData persona in listaPersonas)
        {
            if (persona != null && persona.nombre == nombreABuscar)
            {
                return persona;
            }
        }
        Debug.LogWarning($"[Mánager] No se encontró a ningún personaje con el nombre: {nombreABuscar}");
        return null;
    }
}