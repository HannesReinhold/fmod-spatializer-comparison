using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObject : MonoBehaviour
{

    public bool allowMove = true;
    public bool allowScale = false;
    public bool allowInitialState = false;
    public bool isVisible = true;

    public float fadeTime = 1;

    public float maxAlpha = 1;
    public float minAlpha = 0;

    public float minY = -0.1f;
    public float maxY = 0;

    public float minScale = 1;
    public float maxScale = 1;

   

    private void Awake()
    {
        //
        //FadeOut();
        if (!allowInitialState) return;
        
        
    }

    private void OnEnable()
    {
        if (!allowInitialState) return;
        SetInvisible();

        if (isVisible)
        {
            FadeIn();
        }
    }


    public void FadeIn()
    {
        LeanTween.alpha(gameObject, maxAlpha, fadeTime);
        if(allowMove) LeanTween.moveLocalY(gameObject, maxY, fadeTime);
        if(allowScale) LeanTween.scale(gameObject, Vector3.one*maxScale, fadeTime);
    }

    public void FadeOut()
    {
        LeanTween.alpha(gameObject, minAlpha, fadeTime);
        if (allowMove) LeanTween.moveLocalY(gameObject, minY, fadeTime);
        if (allowScale) LeanTween.scale(gameObject, Vector3.one * minScale, fadeTime);
    }

    public void SetVisible()
    {
        LeanTween.alpha(gameObject, maxAlpha, 0);
        if (allowMove) LeanTween.moveLocalY(gameObject, maxY, 0);
        if (allowScale) LeanTween.scale(gameObject, Vector3.one * maxScale, 0);
    }

    public void SetInvisible()
    {
        LeanTween.alpha(gameObject, minAlpha, 0);
        if (allowMove) LeanTween.moveLocalY(gameObject, minY, 0);
        if (allowScale) LeanTween.scale(gameObject, Vector3.one * minScale, 0);
    }


}
