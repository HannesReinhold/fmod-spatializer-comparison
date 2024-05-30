using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudioSwitcher : MonoBehaviour
{
    public List<GameObject> emitters;

    private void Awake()
    {
        ResetAll();
    }

    public void SetSpatializer(int index)
    {
        for(int i=0; i<emitters.Count; i++)
        {
            if (i == index)
            {
                FMOD.Studio.EventInstance inst = emitters[i].GetComponent<FMODUnity.StudioEventEmitter>().EventInstance;
                inst.start();
            }
            else
            {
                FMOD.Studio.EventInstance inst = emitters[i].GetComponent<FMODUnity.StudioEventEmitter>().EventInstance;
                inst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            emitters[i].SetActive(i == index);
        }
    }

    public void ResetAll()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].SetActive(false);
        }
    }


    public void StopAll()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].SetActive(true);
            FMOD.Studio.EventInstance inst = emitters[i].GetComponent<FMODUnity.StudioEventEmitter>().EventInstance;
            inst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            emitters[i].GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            emitters[i].SetActive(false);
        }
    }

}
