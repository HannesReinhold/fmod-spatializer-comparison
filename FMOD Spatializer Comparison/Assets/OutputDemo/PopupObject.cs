using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using System.Linq;

public class PopupObject : MonoBehaviour
{
    public bool moveLocal;
    public bool openOnEnable = true;
    public bool closeOnComplete = true;
    public bool allowRot = true;
    public bool openOnEnableOnlyOnce=true;

    public float openDuration = 0.25f;
    public float closeDuration = 0.3f;

    public float openDelay = 0;
    public float closeDelay = 0;

    public float minScale = 0.9f;
    public float maxScale = 1f;

    public float minY = -0.1f;
    public float maxY = 0;

    public float minAlpha = 0f;
    public float maxAlpha = 1f;

    public float tiltDeg = 10;

    public float lineMin = 0;
    public float lineMax = 0.01f;

    public bool lineAlphaIndependent = false;

    public Collider collider;

    public List<MeshRenderer> alphaObjects = new List<MeshRenderer>();
    public List<MeshRenderer> alphaObjects2 = new List<MeshRenderer>();

    public List<Renderer> alphaObjects3 = new List<Renderer>();

    private int numOpened = 0;

    public Vector3 scaleMult = Vector3.one;



    private void Awake()
    {
    }

    private void Start()
    {
        LeanTween.moveLocalY(gameObject, minY, 0);
        LeanTween.scale(gameObject, Vector3.Scale(Vector3.one * minScale * 1f, scaleMult), 0);
        //LeanTween.alpha(gameObject, minAlpha, 0);
        SetObjectsAlpha(minAlpha, minAlpha, 0.1f);
        if (collider != null) collider.enabled = false;
    }

    private void OnEnable()
    {
        if (numOpened > 0 && openOnEnableOnlyOnce) return;
        numOpened++;

        LeanTween.moveLocalY(gameObject, minY, 0);
        LeanTween.scale(gameObject, Vector3.Scale(Vector3.one * minScale * 1f,scaleMult), 0);
        //LeanTween.alpha(gameObject, minAlpha, 0);
        SetObjectsAlpha(minAlpha, minAlpha, 0.1f);
        if(allowRot) LeanTween.rotateX(gameObject, 0, 0);
        if (collider != null) collider.enabled = true;
        if (openOnEnable) Open();
    }



    private void Update()
    {

    }

    public void CancelInvokes()
    {
        CancelInvoke();
    }

    public void SetOpen()
    {
        LeanTween.scale(gameObject, Vector3.Scale(Vector3.one * maxScale * 1f,scaleMult), 0.1f).setEaseOutCubic();
        LeanTween.moveLocalY(gameObject, maxY, 0.1f).setEaseOutCubic();
        //LeanTween.alpha(gameObject, maxAlpha, openDuration).setEaseOutCubic();
        SetObjectsAlpha(minAlpha, maxAlpha, 0.1f);
        if (allowRot) LeanTween.rotateX(gameObject, tiltDeg, 0.1f).setEaseOutCubic();
        Invoke("EnableCollider", 0.1f * 0.75f);
    }

    private void OpenWindow()
    {
        LeanTween.scale(gameObject, Vector3.Scale(Vector3.one * maxScale * 1f, scaleMult), openDuration).setEaseOutCubic();
        LeanTween.moveLocalY(gameObject, maxY, openDuration).setEaseOutCubic();
        //LeanTween.alpha(gameObject, maxAlpha, openDuration).setEaseOutCubic();
        SetObjectsAlpha(minAlpha, maxAlpha, openDuration);
        if (allowRot) LeanTween.rotateX(gameObject, tiltDeg, openDuration).setEaseOutCubic();
        Invoke("EnableCollider", openDuration * 0.75f);

    }

    private void CloseWindow()
    {
        if (closeOnComplete) LeanTween.moveLocalY(gameObject, minY, closeDuration).setEaseOutCubic().setOnComplete(Disable);
        else LeanTween.moveLocalY(gameObject, minY, closeDuration).setEaseOutCubic();
        LeanTween.scale(gameObject, Vector3.Scale(Vector3.one * minScale * 1f,scaleMult), closeDuration).setEaseOutCubic();
        //LeanTween.alpha(gameObject, minAlpha, closeDuration).setEaseOutCubic();
        SetObjectsAlpha(maxAlpha, minAlpha, closeDuration);
        if (allowRot) LeanTween.rotateX(gameObject, -tiltDeg, closeDuration).setEaseOutCubic();
        Invoke("EnableCollider", closeDuration * 0.25f);
    }

    void SetObjectsAlpha(float alphaOld, float alphaNew, float duration)
    {
        LeanTween.value(alphaOld, alphaNew, duration).setOnUpdate(UpdateAlpha);
    }


    void UpdateAlpha(float a)
    {
        foreach (MeshRenderer o in alphaObjects)
        {
            if (o == null) continue;
            o.material.SetFloat("_Alpha", lineAlphaIndependent ? a*0.1f:a);
        }

        foreach (MeshRenderer o in alphaObjects2)
        {
            if (o == null) continue;
            UnityEngine.Color c = o.material.color;
            c.a = a;
            o.material.color = c;
        }

        foreach (MeshRenderer o in alphaObjects3)
        {
            if (o == null) continue;
            o.material.SetFloat("_Alpha", lineAlphaIndependent ? a*0.1f:a);
        }

    }

    public void Open()
    {
        Invoke("OpenWindow", openDelay);
    }

    public void Close()
    {
        Invoke("CloseWindow", closeDelay);
    }

    public void OpenTimed(float delay)
    {
        Invoke("OpenWindow", delay);
    }

    public void CloseTimed(float delay)
    {
        Invoke("CloseWindow", delay);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void EnableCollider()
    {
        if (collider != null) collider.enabled = true;
    }

    private void DisableCollider()
    {
        if (collider != null) collider.enabled = false;
    }
}


