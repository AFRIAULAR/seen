using UnityEngine;

public class AppMsgManagent : MonoBehaviour
{
    public static AppMsgManagent appMsgManagent;
    void Awake()
    {
        // Configuramos la instancia correctamente apuntando a este objeto de la escena
        if (appMsgManagent == null)
        {
            appMsgManagent = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
