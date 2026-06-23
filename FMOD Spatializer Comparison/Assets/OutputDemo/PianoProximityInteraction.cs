using UnityEngine;

public class PianoProximityInteraction : MonoBehaviour
{
    public ApartmentManager apartmentManager;
    public GameObject piano;
    public Transform ovrRIg;

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
        active = true;
    }

    public void TurnOnPiano()
    {
        active = false;
    }
}
