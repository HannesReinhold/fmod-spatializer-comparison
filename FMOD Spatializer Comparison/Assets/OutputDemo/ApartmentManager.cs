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

    public void StartApartment()
    {
        apartmentObject.gameObject.SetActive(true);
        dust.Play();
        Invoke("PlayAmbience",1);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(8, 3)); // comment on appartment
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
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(10, 1)); // eggs
    }

    public void OnShowerComplete()
    {
        Invoke("StartPianoEvent",2);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(11, 1)); // shower complete
    }

    public void OnPianoComplete()
    {
        apartmentObject.Close();
        apartmentObject.gameObject.SetActive(false);
        demoManager.StartDemoConcert();
        dust.Stop();
    }

    public void StartPanEvent()
    {
        Debug.Log("STart Pan Event");
        panInteraction.TurnOnPan();
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(9, 2)); // eggs
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
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(12, 4)); // eggs
    }
}
