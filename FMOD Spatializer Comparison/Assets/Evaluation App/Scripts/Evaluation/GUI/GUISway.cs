using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISway : MonoBehaviour
{
    public Transform cameraTransform;

    public Vector3 offset;

    public float sway;

    public float maxDifference;


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newForward = Vector3.Lerp(transform.forward, cameraTransform.forward, Time.deltaTime * sway);

        Vector3 dif = cameraTransform.forward - newForward;
        dif = Vector3.ClampMagnitude(dif, maxDifference);
        newForward = cameraTransform.forward - dif;
        transform.position = cameraTransform.position + newForward * offset.z;


        transform.forward = newForward;



    }
}
