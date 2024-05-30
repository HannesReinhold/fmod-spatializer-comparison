using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingObject : MonoBehaviour
{

    public bool allowMove = true;

    public float fadeTime = 1;

    public float maxAlpha = 1;
    public float minAlpha = 0;

    public float minY = -0.1f;
    public float maxY = 0;

    private void Awake()
    {
        //
        //FadeOut();
    }


    public void FadeIn()
    {
        LeanTween.alpha(gameObject, maxAlpha, fadeTime);
        if(allowMove) LeanTween.moveLocalY(gameObject, maxY, fadeTime);
    }

    public void FadeOut()
    {
        LeanTween.alpha(gameObject, minAlpha, fadeTime);
        if (allowMove) LeanTween.moveLocalY(gameObject, minY, fadeTime);
    }

    public void SetVisible()
    {
        LeanTween.alpha(gameObject, maxAlpha, 0);
        if (allowMove) LeanTween.moveLocalY(gameObject, maxY, 0);
    }

    public void SetInvisible()
    {
        LeanTween.alpha(gameObject, minAlpha, 0);
        if (allowMove) LeanTween.moveLocalY(gameObject, minY, 0);
    }


}
