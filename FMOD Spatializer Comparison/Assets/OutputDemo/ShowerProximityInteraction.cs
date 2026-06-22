using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerProximityInteraction : MonoBehaviour
{
    public List<ParticleSystem> showerParticles;
    //public List<>


    public void TurnOnShower()
    {
        for (int i = 0; i < showerParticles.Count; i++)
        {
            showerParticles[i].Play();
        }
    }

    public void TurnOffShower()
    {
        for (int i = 0; i < showerParticles.Count; i++)
        {
            showerParticles[i].Stop();
        }
    }
}
