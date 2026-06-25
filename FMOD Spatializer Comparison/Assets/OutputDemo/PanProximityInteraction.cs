using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PanProximityInteraction : MonoBehaviour
{
    public ApartmentManager apartmentManager;
    public List<ParticleSystem> smokeParticles;

    public StudioEventEmitter emitter;
    
    public Transform ovrRIg;

    public PopupWindow standingIndicator;
    public Highlightable highlightable;

    public float distTreshold = 1;
    private bool complete=false;
    private bool active=false;

    void Update()
    {
        if(!active) return;
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(ovrRIg.position.x, ovrRIg.position.z));
        //Debug.Log("Dist to Pan: "+dist);
        if (dist < distTreshold && !complete)
        {
            Debug.Log("Pan Complete");
            TurnOffPan();
            apartmentManager.OnPanComplete();
        }
    }


    public void TurnOnPan()
    {
        highlightable.SetHighlight(true);
        standingIndicator.Open();
        emitter.Play();
        active = true;
        for (int i = 0; i < smokeParticles.Count; i++)
        {
            smokeParticles[i].Play();
        }
    }

    public void TurnOffPan()
    {
        highlightable.SetHighlight(false);
        standingIndicator.Close();
        emitter.Stop();
        active=false;
        for (int i = 0; i < smokeParticles.Count; i++)
        {
            smokeParticles[i].Stop();
        }
    }
}
