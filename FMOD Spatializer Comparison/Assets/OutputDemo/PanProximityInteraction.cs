using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanProximityInteraction : MonoBehaviour
{
    public ApartmentManager apartmentManager;
    public List<ParticleSystem> smokeParticles;
    
    public Transform ovrRIg;

    public float distTreshold = 1;
    private bool complete=false;
    private bool active=false;

    void Update()
    {
        if(!active) return;
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(ovrRIg.position.x, ovrRIg.position.z));
        Debug.Log("Dist to Pan: "+dist);
        if (dist < distTreshold && !complete)
        {
            Debug.Log("Pan Complete");
            TurnOffPan();
            apartmentManager.OnPanComplete();
        }
    }


    public void TurnOnPan()
    {
        active = true;
        for (int i = 0; i < smokeParticles.Count; i++)
        {
            smokeParticles[i].Play();
        }
    }

    public void TurnOffPan()
    {
        active=false;
        for (int i = 0; i < smokeParticles.Count; i++)
        {
            smokeParticles[i].Stop();
        }
    }
}
