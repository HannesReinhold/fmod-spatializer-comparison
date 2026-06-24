using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using FMODUnity;

public class AudioParticles : MonoBehaviour
{
    public List<AudioParticle> particlePool;

    public List<EventReference> audioEvents;

    public float maxDistance=2;
    public float minDistance= 1;

    private float timer=0;
    public float minTimer=0.3f;
    public float maxTimer=2;

    public float frequency=0.1f;

    public bool playParticles=false;

    public Gradient colors;


    public void StartSequence()
    {
        
    }

    void Update()
    {
        if(!playParticles) return;
        timer-= Time.deltaTime * frequency;
        if (timer <= 0)
        {
            PlayAvailableParticle();
            timer = Random.Range(minTimer, maxTimer);
        }
    }

    public void Play()
    {
        playParticles=true;
    }

    public void Stop()
    {
        playParticles=false;
    }

    public void PlayAvailableParticle()
    {
        for(int i=0; i<particlePool.Count; i++)
        {
            if (particlePool[i].ready)
            {
                Vector3 dir = Random.onUnitSphere;
                float dist = Random.Range(minDistance,maxDistance);
                Vector3 pos = dir*dist;
                particlePool[i].Blink(pos, audioEvents[Random.Range(0,audioEvents.Count)], colors.Evaluate(Random.Range(0.0f,1.0f)));
                break;
            }
        }
    }

    public void SetIntensity(float intensity)
    {
        frequency = intensity;
    }
}
