using UnityEngine;
using UnityEngine.VFX;

public class ApartmentManager : MonoBehaviour
{
    public OutputDemoManager demoManager;
    public PopupObject apartmentObject;

    public PanProximityInteraction panInteraction;
    public ShowerProximityInteraction showerInteraction;
    public PianoProximityInteraction pianoInteraction;

    public VisualEffect dust;

    public void StartApartment()
    {
        Invoke("StartPanEvent",2);
        dust.Play();
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
