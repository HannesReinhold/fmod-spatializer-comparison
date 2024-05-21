using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPatializerSwitchManager : MonoBehaviour
{
    public List<SpatialAudioSwitcher> switchers;

    public List<SpatializedEvents> spatializedEvents;

    public int spatializerA;
    public int spatializerB;
    public int currentSpatializer;

    public void SetSpatializerPair(int a, int b)
    {
        spatializerA = a;
        spatializerB = b;
        SetSpatializer(a);
    }


    public void SetSpatializer(int index)
    {
        foreach(SpatialAudioSwitcher s in switchers)
        {
            s.SetSpatializer(index);
        }
        currentSpatializer = index;
    }

    public List<FMODUnity.EventReference> GetEvent(int i)
    {
        return spatializedEvents[i].eventRefs;
    }
}
