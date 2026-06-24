using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using ViewR.Passthrough.Scripts;

public class IntroManager : MonoBehaviour
{
    public OutputDemoManager demoManager;

    public List<ReactiveAudioSource> spatialReactiveSources;
    public AudioParticles randomParticles;
    public VisualEffect dust;

    public GameObject introParent;

    public PassthroughManager passthroughManager;

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
        introParent.SetActive(true);
        StartCoroutine(DelayedStartSource(0,1));
        StartCoroutine(DelayedStartSource(1,5));

        randomParticles.Play();
        dust.Play();


        Invoke("StopIntro",100);
    }

    public void StopIntro()
    {
        introParent.SetActive(false);
        demoManager.StartMonoExplanation();
    }
}
