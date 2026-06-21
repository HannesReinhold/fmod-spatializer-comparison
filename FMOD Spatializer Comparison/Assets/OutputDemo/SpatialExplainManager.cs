using UnityEngine;

public class SpatialExplainManager : MonoBehaviour
{
    public PopupObject headVisualizer;
    public PopupObject soundSourceVisualizer;


    public void ShowHead()
    {
        headVisualizer.Open();
    }

    public void HideHead()
    {
        headVisualizer.Close();
    }

    public void ShowSource()
    {
        soundSourceVisualizer.Open();
    }

    public void HideSource()
    {
        soundSourceVisualizer.Close();
    }


}
