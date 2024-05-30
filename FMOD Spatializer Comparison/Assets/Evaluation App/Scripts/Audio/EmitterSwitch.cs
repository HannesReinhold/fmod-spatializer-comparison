using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterSwitch : MonoBehaviour
{
    public List<FMODUnity.EventReference> events;
    public FMODUnity.StudioEventEmitter emitter;


    private void Awake()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    public void SetSpatializer(int index)
    {
        emitter.Stop();
        FMOD.Studio.EventInstance inst = emitter.EventInstance;
        inst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
        emitter.EventReference = events[index];
        
        emitter.Play();
    }


}
