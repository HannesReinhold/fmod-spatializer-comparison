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
        Invoke("StartPanEvent",2);
        apartmentObject.gameObject.SetActive(true);
        dust.Play();
        Invoke("PlayAmbience",2);
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
    }

    public void OnShowerComplete()
    {
        Invoke("StartPianoEvent",2);
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
    }
}
