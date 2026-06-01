using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    [SerializeField] private EmotionalStateManager emotionalState;

    private int selectedOption = -1;

    public void SelectText(int option)
    {
        selectedOption = option;
        Debug.Log("Texto seleccionado: " + option);
    }

    public void Reflexionar()
    {
        if (selectedOption == -1)
        {
            Debug.Log("Seleccioná una entrada del diario primero.");
            return;
        }

        switch (selectedOption)
        {
            case 0:
                // Pensamiento: "No puedo con todo"
                emotionalState.ModifyState(-10, 0, +5);
                break;

            case 1:
                // Pensamiento: "Tengo que estar para todos"
                emotionalState.ModifyState(+10, +10, -10);
                break;

            case 2:
                // Pensamiento: "Hoy necesito elegirme"
                emotionalState.ModifyState(+5, -10, +15);
                break;
        }

        Debug.Log("Reflexión aplicada.");
    }
}