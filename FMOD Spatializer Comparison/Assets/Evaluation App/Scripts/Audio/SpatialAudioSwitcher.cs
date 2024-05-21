using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudioSwitcher : MonoBehaviour
{
    public List<GameObject> emitters;

    public void SetSpatializer(int index)
    {
        for(int i=0; i<emitters.Count; i++)
        {
            emitters[i].SetActive(i == index);
        }
    }

}
