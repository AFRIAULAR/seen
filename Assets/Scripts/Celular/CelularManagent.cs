using UnityEngine;

public class CelularManagent : MonoBehaviour
{
    public static CelularManagent celularInstancia;
    public static CelularInterface celularInterface;

    void Awake()
    {
        if (celularInstancia == null)
        {
            celularInstancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
