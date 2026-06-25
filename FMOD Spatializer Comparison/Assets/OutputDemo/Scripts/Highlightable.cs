using UnityEngine;

public class Highlightable : MonoBehaviour
{
    public Renderer renderer;

    public bool isHighlighted=false;
    public float flashingFreq=1;

    public float alphaMult=0;

    void Start()
    {
        
    }


    void UpdateAlpha(float a)
    {
        alphaMult=a;

    }

    void Update()
    {
        if(!isHighlighted) 
        {
            renderer.materials[1].SetFloat("_Alpha",0);
            return;
        }
        renderer.materials[1].SetFloat("_Alpha",(Mathf.Sin(Time.time*flashingFreq)*0.5f+0.5f)*alphaMult*0.5f);
    }

    public void SetHighlight(bool h)
    {
        isHighlighted=h;
        if(h)
            LeanTween.value(0, 1, 1).setOnUpdate(UpdateAlpha);
        else
            LeanTween.value(1, 0, 1).setOnUpdate(UpdateAlpha);
    }
}
