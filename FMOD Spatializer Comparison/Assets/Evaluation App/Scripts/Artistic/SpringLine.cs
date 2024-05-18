using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringLine : MonoBehaviour
{
    public LineRenderer springLineRenderer;

    public float radius = 0.1f;
    public int numTurns = 4;
    public int numVerts = 256;

    public Transform bottomTransform;
    public Transform topTransform;

    // Start is called before the first frame update
    void Start()
    {
        springLineRenderer.positionCount = numVerts;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 top = topTransform.position;
        Vector3 bottom = bottomTransform.position;

        for(int i=0; i< numVerts; i++)
        {
            float t = i / (float)(numVerts-1);
            Vector3 vertPos = Vector3.zero;
            vertPos = Vector3.Lerp(bottom, top, t) + new Vector3(Mathf.Sin(t * Mathf.PI * 2 * numTurns), 0, Mathf.Cos(t*Mathf.PI*2*numTurns)) * radius;

            springLineRenderer.SetPosition(i,vertPos);
        }

    }
}
