using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineDestroyer : MonoBehaviour
{
    public ConveyorBelt belt;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit2");
        if (other.tag == "Engine")
        {
            belt.RemoveEngine(other.gameObject);
        }
    }
}
