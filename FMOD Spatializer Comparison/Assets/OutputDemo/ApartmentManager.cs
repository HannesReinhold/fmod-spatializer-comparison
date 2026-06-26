using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class ApartmentManager : MonoBehaviour
{
    public OutputDemoManager demoManager;
    public PopupObject apartmentObject;

    public PanProximityInteraction panInteraction;
    public ShowerProximityInteraction showerInteraction;
    public PianoProximityInteraction pianoInteraction;

    public List<StudioEventEmitter> ambienceEmitters;

    public VisualEffect dust;

    public Transform appartmentParent;
    public Transform outsideParent;


    void Awake()
    {
        foreach (Transform childTransform in this.transform)
        {
            LeanTween.alpha(childTransform.gameObject,0,0);
        }
    }
    public void OpenAppartment()
    {
        foreach (Transform childTransform in this.transform)
        {
            LeanTween.alpha(childTransform.gameObject,1,1);
        }
    }

    public void CloseAppartment()
    {
        foreach (Transform childTransform in this.transform)
        {
            LeanTween.alpha(childTransform.gameObject,0,1);
        }
    }

    public void StartApartment()
    {
        apartmentObject.gameObject.SetActive(true);
        OpenAppartment();

        dust.Play();
        Invoke("PlayAmbience",1);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(7, 3)); // comment on appartment
        Invoke("StartPanEvent", 6);
    }

    public void PlayAmbience()
    {
        for(int i=0; i<ambienceEmitters.Count; i++)
        {
            ambienceEmitters[i].Play();
        }
    }

    public void StopAmbience()
    {
        for(int i=0; i<ambienceEmitters.Count; i++)
        {
            ambienceEmitters[i].Stop();
        }
    }
    public void OnPanComplete()
    {
        Invoke("StartShowerEvent",2);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(9, 1)); // eggs
    }

    public void OnShowerComplete()
    {
        Invoke("StartPianoEvent",2);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(10, 1)); // shower complete
    }

    public void OnPianoComplete()
    {
        CloseAppartment();
        apartmentObject.gameObject.SetActive(false);
        demoManager.StartDemoConcert();
        dust.Stop();
    }

    public void StartPanEvent()
    {
        Debug.Log("STart Pan Event");
        panInteraction.TurnOnPan();
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(8, 2)); // eggs
    }

    public void StartShowerEvent()
    {
        Debug.Log("STart Shower Event");
        showerInteraction.TurnOnShower();
    }

    public void StartPianoEvent()
    {
        Debug.Log("STart Piano Event");
        pianoInteraction.SpawnPiano();
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(11, 4)); // eggs
    }

    
}
