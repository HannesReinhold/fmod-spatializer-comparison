using UnityEngine;

public class ApartmentManager : MonoBehaviour
{
    public OutputDemoManager demoManager;
    public PopupObject apartmentObject;

    public PanProximityInteraction panInteraction;
    public ShowerProximityInteraction showerInteraction;
    public PianoProximityInteraction pianoInteraction;

    public void StartApartment()
    {
        Invoke("StartPanEvent",2);
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
