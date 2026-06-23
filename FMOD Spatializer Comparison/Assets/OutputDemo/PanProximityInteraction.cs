using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanProximityInteraction : MonoBehaviour
{
    public List<ParticleSystem> smokeParticles;
    //public List<>


    public void TurnOnPan()
    {
        for (int i = 0; i < smokeParticles.Count; i++)
        {
            smokeParticles[i].Play();
        }
    }

    public void TurnOffPan()
    {
        for (int i = 0; i < smokeParticles.Count; i++)
        {
            smokeParticles[i].Stop();
        }
    }
}
