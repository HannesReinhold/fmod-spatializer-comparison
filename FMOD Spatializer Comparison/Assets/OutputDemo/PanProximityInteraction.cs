using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerProximityInteraction : MonoBehaviour
{
    public List<ParticleSystem> smokeParticles;
    //public List<>


    public void TurnOnPan()
    {
        for (int i = 0; i < showerParticles.Count; i++)
        {
            smokeParticles[i].Play();
        }
    }

    public void TurnOffPan()
    {
        for (int i = 0; i < showerParticles.Count; i++)
        {
            smokeParticles[i].Stop();
        }
    }
}
