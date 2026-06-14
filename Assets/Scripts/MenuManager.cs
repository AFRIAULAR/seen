using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void GoToPhone()
    {
        SceneManager.LoadScene("Joel");
    }
}