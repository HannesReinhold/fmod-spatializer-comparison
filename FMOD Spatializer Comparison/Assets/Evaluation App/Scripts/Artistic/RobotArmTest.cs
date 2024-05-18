using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmTest : MonoBehaviour
{

    [Range(0,360)] public float rotation = 0;
    [Range(0, 25)] public float speed = 1;
    [Range(1, 100)] public float acceleration = 10;

    public Vector2 rotationRange;

    public Transform anchorTransform;

    private float currentRotation;



    private float seed;


    // Start is called before the first frame update
    void Start()
    {
        seed = Random.value * 100;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = (sigmoid(Mathf.PerlinNoise1D(seed+Time.time*0.5f)*2-1))*2-1;

        currentRotation += speed;
        if (currentRotation >= 360 && rotationRange.x>=0) currentRotation = 0;
        if (currentRotation < 0 && rotationRange.x >= 0) currentRotation = 359;
        if (currentRotation < rotationRange.x) currentRotation = rotationRange.x;
        if (currentRotation >= rotationRange.y) currentRotation = rotationRange.y;

        anchorTransform.localEulerAngles = new Vector3(0, currentRotation,0);

    }

    private float sigmoid(float x)
    {
        return 1f / (1+Mathf.Exp(-x*acceleration));
    }
}
