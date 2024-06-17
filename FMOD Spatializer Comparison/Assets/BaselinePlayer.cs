using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaselinePlayer : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;
    private bool canActivate = false;

    private Vector3 lastPos=Vector3.zero;

    int n = 0;


    void OnEnable()
    {

        canActivate = true;
    }

    public void Play()
    {
        Debug.Log(n);
        if (n >= 10)
        {
            Stop();
            return;
        }
        if (GameManager.Instance.isAssistant && !GameManager.Instance.IsVR && canActivate)
            emitter.Play();
        n++;
        
    }

    public void Stop()
    {
        emitter.Stop();
    }


    void OnDisable()
    {
        emitter.Stop();
    }

    void Update()
    {
        if(transform.position != lastPos)
        {
            lastPos = transform.position;
            Stop();
            Invoke("Play", 5);
            
        }
    }
}
