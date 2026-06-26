using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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

    public List<EventReference> staticSfx;



    private float particlesIntensity=0;

    public void StartAllStaticSFX()
    {
        for(int i=0; i<staticSfx.Count; i++)
        {
            FMODUnity.RuntimeManager.PlayOneShot(staticSfx[i], Camera.main.transform.position+Random.onUnitSphere*Random.Range(1,3));
        }
    }

    public void StartAllDynamicSFX()
    {
        for(int i=0; i<spatialReactiveSources.Count; i++)
        {
            spatialReactiveSources[i].PlaySound();
        }
    }

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

        //Debug.Log("Set Intensity "+a);
    }

    public void StartIntro()
    {
        Invoke("StartAllStaticSFX",1);
        Invoke("StartAllDynamicSFX",1);

        introParent.SetActive(true);
        dust.Play();
        // start with some random quiet sounds around the player
        Invoke("StartParticles",1);
        // show a particle trail that moves slowly around the player and plays some granular sounds
        StartCoroutine(DelayedStartSource(0,3));
        //increase the density of granular sounds 
        StartCoroutine(DelayedSetIntensityTarget(3,4,4));
        // show second trail with drone sound
        // play also a riser quietly
        StartCoroutine(DelayedStartSource(0,0));
        StartCoroutine(DelayedSetIntensityTarget(6,4,10));
        // show third trail with drone sound
        StartCoroutine(DelayedStartSource(1,0));
        StartCoroutine(DelayedStartSource(2,0));
        StartCoroutine(DelayedStartSource(3,0));
        StartCoroutine(DelayedStartSource(4,0));
        StartCoroutine(DelayedStartSource(5,0));
        StartCoroutine(DelayedStartSource(6,0));
        StartCoroutine(DelayedSetIntensityTarget(10,4,15));
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(0,15));

        // Sudden Stop
        StartCoroutine(DelayedSetIntensityTarget(0,0.1f,43));
        StartCoroutine(DelayedStopSource(0,43));
        StartCoroutine(DelayedStopSource(1,43));
        StartCoroutine(DelayedStopSource(2,43));
        StartCoroutine(DelayedStopSource(3,43));
        StartCoroutine(DelayedStopSource(4,43));
        StartCoroutine(DelayedStopSource(5,43));
        StartCoroutine(DelayedStopSource(6,43));

        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(1,40));//41
        // Narrator shows
        //StartCoroutine(demoManager.narratorManager.DelayedSetFacingCamera(new Vector3(0,0,1),1,43));
        //StartCoroutine(demoManager.narratorManager.DelayedShow(44));

        Invoke("StopIntro",45);
    }

    public void StopIntro()
    {
        introParent.SetActive(false);
        demoManager.StartMonoExplanation();
    }
}
