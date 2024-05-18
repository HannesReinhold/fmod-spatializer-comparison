using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSphereEditor : MonoBehaviour
{
    [Range(1,25)] public int gridResolution = 8;

    [Range(0, 25)] public float Radius = 1f;
    [Range(0, 1)] public float Width = 0.1f;
    [Range(0, 1)] public float Alpha = 1f;


    public float targetRadius;
    public float targetWidth;
    public float targetAlpha;
    public float animationTime = 1;

    private float currentAnimationTime = 1;

    public GameObject circlePrefab;
    public Transform parent;

    private float lastRadius;
    private float lastWidth;
    private float lastAlpha;

    private float currentRadius;
    private float currentWidth;
    private float currentAlpha;

    private float currentAnimationFrame;


    private List<LineRenderer> sphereLines = new List<LineRenderer>();



    // Start is called before the first frame update
    void Start()
    {
        CreateWireSphere();
        //transform.position = new Vector3(1.896f , -1.5f, -3.231f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnValidate()
    {
        //SetSphere(Radius, Width, Alpha);
    }

    public void SetSphere(float r, float w, float a)
    {
        lastRadius = currentRadius;
        lastWidth = currentWidth;
        lastAlpha = currentAlpha;

        targetRadius = r;
        targetWidth = w;
        targetAlpha = a;

        currentAnimationTime = animationTime;
        currentAnimationFrame = 0;

        //StopCoroutine(UpdateShpere());
        StartCoroutine(UpdateShpere());
    }

    public void SetSphere(float r, float w, float a, float time)
    {
        
        SetSphere(r,w,a);
        currentAnimationTime = time;

        //StopCoroutine(UpdateShpere());
        StartCoroutine(UpdateShpere());
    }

    public void SetSphereInstant(float r, float w, float a)
    {
        currentAlpha = a;
        currentRadius = r;
        currentWidth = w;
        SetWireSphere(64, gridResolution);
    }



    IEnumerator UpdateShpere()
    {
        Debug.Log("Starting");

        while(currentAnimationFrame < 1f)
        {
            currentAnimationFrame += Time.deltaTime/ currentAnimationTime;

            currentRadius = Mathf.Lerp(lastRadius, targetRadius, currentAnimationFrame);
            currentWidth = Mathf.Lerp(lastWidth, targetWidth, currentAnimationFrame);
            currentAlpha = Mathf.Lerp(lastAlpha, targetAlpha, currentAnimationFrame);

            SetWireSphere(64, gridResolution);

            yield return null;
        }

        yield return new WaitForSeconds(currentAnimationTime);

        Debug.Log("Finished");
    }

    void CreateWireSphere()
    {
        
        for (int i = 0; i < gridResolution * 2 - 1; i++)
        {
            GameObject circle = Instantiate(circlePrefab);
            circle.transform.position = Vector3.zero;
            circle.transform.parent = transform;
            transform.parent = parent;
            
            
            LineRenderer line = circle.GetComponent<LineRenderer>();
            sphereLines.Add(line);
        }
        transform.localPosition = Vector3.zero;

        SetSphereInstant(0, 0, 0);
    }


    void SetWireSphere(int numSegments, int gridN)
    {


        int i = 0;

        if (gridN < 2)
        {
            gridN = 2;
        }

        int doubleSegments = gridN * 2;

        // Draw meridians

        float meridianStep = 180.0f / gridN;

        for (i = 0; i < gridN; i++)
        {
            sphereLines[i].positionCount = numSegments + 1;
            sphereLines[i].SetPositions(WireSphereDrawer.CreateCircle(Vector3.zero, Quaternion.Euler(0, meridianStep * i, 0), currentRadius, numSegments, Color.red));
            //DrawCircle(position, orientation * Quaternion.Euler(0, meridianStep * i, 0), radius, doubleSegments, color);
        }

        // Draw parallels

        Vector3 verticalOffset = Vector3.zero;
        float parallelAngleStep = Mathf.PI / (gridN);
        float stepRadius = 0.0f;
        float stepAngle = 0.0f;

        for (i = gridN; i < gridN * 2 - 1; i++)
        {
            stepAngle = parallelAngleStep * (i - gridN + 1);
            verticalOffset = (Vector3.up) * Mathf.Cos(stepAngle) * currentRadius;
            stepRadius = Mathf.Sin(stepAngle) * currentRadius;

            sphereLines[i].positionCount = numSegments + 1;
            sphereLines[i].SetPositions(WireSphereDrawer.CreateCircle(verticalOffset, Quaternion.Euler(90.0f, 0, 0), stepRadius, numSegments, Color.red));
        }

        SetSphereAlpha();
    }

    void SetSphereAlpha()
    {
        for (int i = 0; i < sphereLines.Count; i++)
        {
            sphereLines[i].material.SetColor("_Color", new Color(0.8f, 0.8f, 0.8f, currentAlpha));
            sphereLines[i].widthMultiplier = Mathf.Pow(currentWidth * 4, 2);
        }
    }
}
