using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaytracingVisualizer : MonoBehaviour
{
    public Transform sourceTransform;
    public Transform parent;

    public List<GameObject> LineRendererPrefab;
    public GameObject hemispherePrefab;

    public int numRaysDirect;
    public int numRaysReflections;
    public int numReflections;


    public List<LineRenderer> lines;
    public List<GameObject> reflectionVisuals;

    public List<Vector3> currentRays;
    public List<GameObject> currentHits;

    public float maxDistance = 10;
    public Gradient colorByDistance;

    private int currentReflectionOrder = 0;

    void Start()
    {
        HideAllRays();
    }

    void Update()
    {
        
    }

    public void ShowAllRays()
    {
        for(int i=0; i<lines.Count; i++)
        {
            lines[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < reflectionVisuals.Count; i++)
        {
            reflectionVisuals[i].gameObject.SetActive(true);
        }
    }

    public void HideAllRays()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < reflectionVisuals.Count; i++)
        {
            reflectionVisuals[i].gameObject.SetActive(false);
        }
    }



    public void ResetRays()
    {
        for(int i = 0; i < lines.Count; i++) 
        {
            DestroyImmediate(lines[i].gameObject);
        }
        lines.Clear();

        for (int i = 0; i < reflectionVisuals.Count; i++)
        {
            DestroyImmediate(reflectionVisuals[i]);
        }
        reflectionVisuals.Clear();

        currentHits.Clear();
        currentRays.Clear();
        currentReflectionOrder = 0;
    }

    public void StartRaycasting()
    {
        ResetRays();
        List<Vector3> directions = GenerateDirections(numRaysDirect);

        for (int i=0; i<numRaysDirect; i++)
        {
            LineRenderer line = Instantiate(LineRendererPrefab[0], parent).GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, sourceTransform.position);
            lines.Add(line);

            RaycastHit hit;
            if (Physics.Raycast(sourceTransform.position, directions[i], out hit, Mathf.Infinity))
            {
                GameObject reflectionVisual = Instantiate(hemispherePrefab, parent);
                reflectionVisual.transform.position = hit.point;
                reflectionVisual.transform.up = hit.normal;
                float dist = hit.distance/maxDistance;
                line.GetComponent<LineRenderer>().startColor = colorByDistance.Evaluate(0);
                line.GetComponent<LineRenderer>().endColor = colorByDistance.Evaluate(dist);

                reflectionVisuals.Add(reflectionVisual);
                currentHits.Add(reflectionVisual);
                currentRays.Add(hit.point - sourceTransform.position);
            }
            line.SetPosition(1, hit.point);
        }
        for(int i=0; i<numReflections; i++)
        {
            StartReflection();
        }
        currentReflectionOrder++;
    }

    public void StartReflection()
    {

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;
        List<GameObject> tempHits = new List<GameObject>();
        List<Vector3> tempRays = new List<Vector3>();

        for (int i = 0; i < currentHits.Count; i++)
        {
            Vector3 reflectedDirecion = Vector3.Reflect(currentRays[i], currentHits[i].transform.up).normalized;
            float rayLength = currentRays[i].magnitude;


            LineRenderer line = Instantiate(LineRendererPrefab[currentReflectionOrder], parent).GetComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, currentHits[i].transform.position);
            lines.Add(line);

            RaycastHit hit;
            if (Physics.Raycast(currentHits[i].transform.position, reflectedDirecion, out hit, Mathf.Infinity))
            {
                GameObject reflectionVisual = Instantiate(hemispherePrefab,parent);
                reflectionVisual.transform.position = hit.point;
                reflectionVisual.transform.up = hit.normal;
                float dist = rayLength + hit.distance;
                line.GetComponent<LineRenderer>().startColor = colorByDistance.Evaluate(rayLength/ maxDistance);
                line.GetComponent<LineRenderer>().endColor = colorByDistance.Evaluate((rayLength + hit.distance)/maxDistance);
                reflectionVisuals.Add(reflectionVisual);
                tempHits.Add(reflectionVisual);
                tempRays.Add(Vector3.Normalize(hit.point - currentHits[i].transform.position)*dist);
            }
            line.SetPosition(1, hit.point);
        }
        currentHits.Clear();
        currentRays.Clear();
        for(int i=0; i< tempHits.Count; i++) 
        { 
            currentHits.Add(tempHits[i]);
        }

        for (int i = 0; i < tempRays.Count; i++)
        {
            currentRays.Add(tempRays[i]);
        }
        currentReflectionOrder++;
    }


    List<Vector3> GenerateDirections(int numDirections)
    {
        List<Vector3>  directions = new List<Vector3>();

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numDirections; i++)
        {
            float t = (float)i / numDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            directions.Add(new Vector3(x, z, y));
           
        }
        return directions;
    }
}
