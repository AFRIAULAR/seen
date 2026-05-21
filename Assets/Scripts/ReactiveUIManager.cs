using UnityEngine;
using UnityEngine.UI;

public class ReactiveUIManager : MonoBehaviour
{
    [SerializeField] private Image stressOverlay;
    [SerializeField] private Image blurOverlay;

    private int stress = 0;

    public void AddStress(int amount)
    {
        stress += amount;
        stress = Mathf.Clamp(stress, 0, 100);

        UpdateVisualStress();
    }

    private void UpdateVisualStress()
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