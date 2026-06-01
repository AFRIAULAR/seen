using UnityEngine;
using UnityEngine.UI;

public class ReactiveUIManager : MonoBehaviour
{
    [SerializeField] private Image stressOverlay;
    [SerializeField] private Image blurOverlay;

    public void SetStress(int stress)
    {
        stress = Mathf.Clamp(stress, 0, 100);
        UpdateVisualStress(stress);
    }

    private void UpdateVisualStress(int stress)
    {
        float alpha = stress / 100f * 0.8f;

        Color color = stressOverlay.color;
        color.a = alpha;
        stressOverlay.color = color;

        Color blurColor = blurOverlay.color;
        blurColor.a = stress / 100f * 0.35f;
        blurOverlay.color = blurColor;
    }
}