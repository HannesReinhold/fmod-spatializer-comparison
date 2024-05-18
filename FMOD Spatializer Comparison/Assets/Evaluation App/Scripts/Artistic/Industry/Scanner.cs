using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Engine"))
        {
            emitter.Play();
            Invoke("StopEmitter",2);
            Debug.Log("Scan");
        }
    }

    void StopEmitter()
    {
        emitter.Stop();
    }
}
