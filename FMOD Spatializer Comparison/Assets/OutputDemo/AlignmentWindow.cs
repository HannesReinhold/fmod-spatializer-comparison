using TMPro;
using UnityEngine;

public class AlignmentWindow : MonoBehaviour
{
    public TextMeshProUGUI textRenderer;

    public void SetText(string text)
    {
        textRenderer.text = text;
    }
}
