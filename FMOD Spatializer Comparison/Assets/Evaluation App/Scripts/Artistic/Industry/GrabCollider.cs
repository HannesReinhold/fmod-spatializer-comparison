using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCollider : MonoBehaviour
{

    public ConveyorBelt controller;


    bool inRange = false;

    bool trigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) >= 0.5f&& inRange && !trigger) Grab();
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) < 0.5f && trigger) trigger=false;

    }

    void Grab()
    {
        trigger = true;
        controller.Grab();
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }
}
