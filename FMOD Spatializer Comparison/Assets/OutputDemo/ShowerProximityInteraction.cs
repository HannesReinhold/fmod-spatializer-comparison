using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ShowerProximityInteraction : MonoBehaviour
{
    public List<ParticleSystem> showerParticles;
    public ApartmentManager apartmentManager;

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
        //Debug.Log("Dist to Shower: "+dist);
        if (dist < distTreshold && !complete)
        {
            Debug.Log("Shower Complete");
            TurnOffShower();
            apartmentManager.OnShowerComplete();
        }
    }


    public void TurnOnShower()
    {
        highlightable.SetHighlight(true);
        emitter.Play();
        for (int i = 0; i < showerParticles.Count; i++)
        {
            showerParticles[i].Play();
        }
        active = true;
        standingIndicator.Open();
    }

    public void TurnOffShower()
    {
        highlightable.SetHighlight(false);
        emitter.Stop();
        active = false;
        for (int i = 0; i < showerParticles.Count; i++)
        {
            showerParticles[i].Stop();
        }
        standingIndicator.Close();
    }
}
