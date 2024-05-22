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

}
