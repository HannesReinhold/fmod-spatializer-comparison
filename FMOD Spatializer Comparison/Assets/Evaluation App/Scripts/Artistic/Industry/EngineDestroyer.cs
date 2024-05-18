using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit2");
        if (other.tag == "Engine")
        {
            Destroy(other.gameObject);
        }
    }
}
