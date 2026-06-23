using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public List<ReactiveAudioSource> spatialReactiveSources;
    public AudioParticles randomParticles;

    public void StartSource(int index)
    {
        spatialReactiveSources[index].Play();
    }

    IEnumerator DelayedStartSource(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        StartSource(index);

    }

    public void StartIntro()
    {
        StartCoroutine(DelayedStartSource(0,1));
        StartCoroutine(DelayedStartSource(1,5));

        randomParticles.Play();
    }
}
