using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceTutorial : MonoBehaviour
{

    public List<Hint> hints = new List<Hint>();

    void Start()
    {
        for(int i = 0; i < hints.Count; i++)
        {
            hints[i].CloseHint();
        }
    }

    public void Open()
    {
        GUIAudioManager.SetTutorialVolume(0.5f);

        Invoke("Open1", 5);
        Invoke("Open2", 10);
        Invoke("Open3", 15);

    }

    public void FinishTutorial()
    {
        CancelInvoke("Open1");
        CancelInvoke("Open2");
        CancelInvoke("Open3");


        GUIAudioManager.SetTutorialVolume(1);
    }

    private void OnEnable()
    {

    }

    public void Close(int i)
    {
        hints[i].CloseHint();
    }

    public void Open(int i)
    {
        hints[i].OpenHint();
    }


    private void Open1()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Cues/Hint2", hints[0].transform.position);
        hints[0].OpenHint();
    }

    private void Open2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Cues/Hint2", hints[1].transform.position);
        hints[1].OpenHint();
        hints[0].HideHint();
    }

    private void Open3()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Cues/Hint2", hints[2].transform.position);
        hints[2].OpenHint();
        hints[1].HideHint();
    }
}
