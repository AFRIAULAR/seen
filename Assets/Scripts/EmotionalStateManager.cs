using UnityEngine;
using TMPro;

public class EmotionalStateManager : MonoBehaviour
{
    [SerializeField] private ReactiveUIManager reactiveUI;

    [SerializeField] private TMP_Text stressText;
    [SerializeField] private TMP_Text validationText;
    [SerializeField] private TMP_Text identityText;

    [Header("Estados emocionales")]
    [Range(0, 100)] public int stress = 20;
    [Range(0, 100)] public int validation = 50;
    [Range(0, 100)] public int identity = 50;


    private void Start()
    {
        UpdateHUD();
        reactiveUI.SetStress(stress);
    }
    public void ModifyState(int stressChange, int validationChange, int identityChange)
    {
        stress = Mathf.Clamp(stress + stressChange, 0, 100);
        validation = Mathf.Clamp(validation + validationChange, 0, 100);
        identity = Mathf.Clamp(identity + identityChange, 0, 100);

        Debug.Log(
        $"Stress: {stress} | Validation: {validation} | Identity: {identity}");

        reactiveUI.SetStress(stress);

        UpdateHUD();
    }

    private void UpdateHUD()
    {
        stressText.text = stress + "%";
        validationText.text = validation + "%";
        identityText.text = identity + "%";
    }
}