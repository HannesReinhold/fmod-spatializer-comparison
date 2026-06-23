using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public List<ReactiveAudioSource> spatialReactiveSources;

    public void StartSource(int index)
    {
        spatialReactiveSources[index].Play();
        Debug.Log("Play Reactive AUdio SOurce");
    }

    IEnumerator DelayedStartSource(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        StartSource(index);

    }

    void Start()
    {
        StartCoroutine(DelayedStartSource(0,1));
        StartCoroutine(DelayedStartSource(1,5));
    }
}
