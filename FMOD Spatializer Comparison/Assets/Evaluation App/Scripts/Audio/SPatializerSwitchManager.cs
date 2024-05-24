using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPatializerSwitchManager : MonoBehaviour
{
    public List<SpatialAudioSwitcher> switchers;

    public List<SpatializedEvents> spatializedEvents;

    public List<SpatialAudioSwitcher> engineBlocks;

    public int spatializerA;
    public int spatializerB;
    public int currentSpatializer;


    private void Start()
    {
        StopAll();
    }

    public void StopAll()
    {
        foreach(SpatialAudioSwitcher switcher in switchers)
        {
            switcher.ResetAll();
        }
    }

    public void PlayAll()
    {
        foreach (SpatialAudioSwitcher switcher in switchers)
        {
            switcher.SetSpatializer(currentSpatializer);
        }
    }

    public void SetSpatializerPair(int a, int b)
    {
        spatializerA = a;
        spatializerB = b;
        SetSpatializer(a);
    }


    public void SetSpatializer(int index)
    {
        currentSpatializer = index;

        foreach (SpatialAudioSwitcher s in switchers)
        {
            s.SetSpatializer(index);
        }
        foreach(SpatialAudioSwitcher s in engineBlocks)
        {
            if(s!=null) s.SetSpatializer(index);
        }
        
    }

    public List<FMODUnity.EventReference> GetEvent(int i)
    {
        return spatializedEvents[i].eventRefs;
    }

    public void SetIndustryVolume(float vol)
    {
    }
}
