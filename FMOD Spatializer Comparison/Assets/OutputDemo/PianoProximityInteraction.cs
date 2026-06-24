using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PianoProximityInteraction : MonoBehaviour
{
    public ApartmentManager apartmentManager;
    public GameObject piano;
    public Transform ovrRIg;
    public StudioEventEmitter emitter;

    public PopupWindow standingIndicator;

    public List<Highlightable> highlightable;

    private bool complete=false;
    private bool active=false;

    public float distTreshold = 1;

    void Update()
    {
        if(!active) return;
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(ovrRIg.position.x, ovrRIg.position.z));
        Debug.Log("Dist to Piano: "+dist);
        if (dist < distTreshold && !complete)
        {
            Debug.Log("Piano Complete");
            TurnOnPiano();
            apartmentManager.OnPianoComplete();
        }
    }

    public void SpawnPiano()
    {
        for(int i=0; i<highlightable.Count; i++)
        {
            highlightable[i].SetHighlight(true);
        }
        active = true;
        standingIndicator.Open();
    }

    public void TurnOnPiano()
    {
         for(int i=0; i<highlightable.Count; i++)
        {
            highlightable[i].SetHighlight(false);
        }
        standingIndicator.Close();
        emitter.Play();
        active = false;
    }
}
