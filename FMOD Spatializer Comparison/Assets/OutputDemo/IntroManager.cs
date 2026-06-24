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


    private float particlesIntensity=0;

    public void StartSource(int index)
    {
        spatialReactiveSources[index].Play();
    }

    IEnumerator DelayedStartSource(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        StartSource(index);

    }

    IEnumerator DelayedStopSource(int index, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        spatialReactiveSources[index].Stop();

    }

    private void StartParticles()
    {
        particlesIntensity=0;
        randomParticles.SetIntensity(particlesIntensity);
        randomParticles.Play();
    }

    IEnumerator DelayedSetIntensityTarget(float intensity, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log(randomParticles.frequency+", "+intensity);
        SetIntensityTarget(randomParticles.frequency, intensity, duration);
    }

    void SetIntensityTarget(float alphaOld, float alphaNew, float duration)
    {
        LeanTween.value(alphaOld, alphaNew, duration).setOnUpdate(UpdateIntensity);
    }


    void UpdateIntensity(float a)
    {
        randomParticles.SetIntensity(a);

        Debug.Log("Set Intensity "+a);
    }

    public void StartIntro()
    {
        introParent.SetActive(true);
        dust.Play();
        // start with some random quiet sounds around the player
        Invoke("StartParticles",1);
        // show a particle trail that moves slowly around the player and plays some granular sounds
        StartCoroutine(DelayedStartSource(0,6));
        //increase the density of granular sounds 
        StartCoroutine(DelayedSetIntensityTarget(3,4,4));
        // show second trail with drone sound
        // play also a riser quietly
        StartCoroutine(DelayedStartSource(0,10));
        StartCoroutine(DelayedSetIntensityTarget(6,4,10));
        // show third trail with drone sound
        StartCoroutine(DelayedStartSource(1,16));
        StartCoroutine(DelayedSetIntensityTarget(10,4,15));

        // Sudden Stop
        StartCoroutine(DelayedSetIntensityTarget(0,0.1f,20));
        StartCoroutine(DelayedStopSource(0,20));
        StartCoroutine(DelayedStopSource(1,20));
        // Narrator begins
        StartCoroutine(demoManager.narratorManager.DelayedSetFacingCamera(new Vector3(0,0,1),0,22));
        StartCoroutine(demoManager.narratorManager.DelayedShow(22));
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(0,24));

        Invoke("StopIntro",30);
    }

    public void StopIntro()
    {
        introParent.SetActive(false);
        demoManager.StartMonoExplanation();
    }
}
