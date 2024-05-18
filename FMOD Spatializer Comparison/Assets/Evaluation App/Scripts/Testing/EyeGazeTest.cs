using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeGazeTest : MonoBehaviour
{
    public OVREyeGaze gazeLeft;
    public OVREyeGaze gazeRight;

    public Transform headTransform;
    public Transform gazeIndicator;

    public float speed = 5;


    private LineRenderer lineRenderer;

    private Vector3 smoothedDirection;


    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentDirection = (gazeLeft.transform.forward+ gazeLeft.transform.forward).normalized;
        smoothedDirection = Vector3.Lerp(smoothedDirection, currentDirection, Time.deltaTime*speed);

        gazeIndicator.position = headTransform.position + smoothedDirection * 5;

        lineRenderer.SetPosition(0,  headTransform.position);
        lineRenderer.SetPosition(1, gazeIndicator.position);
    }
}
