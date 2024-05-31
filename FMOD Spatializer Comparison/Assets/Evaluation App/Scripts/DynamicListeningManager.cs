using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicListeningManager : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;
    public AppearingObject emitterVisual;

    public AppearingObject currentGuessSphere;
    public GameObject currentGuessHighlight;

    public LineRenderer distanceLine;

    public List<FMODUnity.EventReference> events;

    public List<FMODUnity.EventReference> tutorialEvents;

    public WindowManager windowManager;

    public AppearingObject grabButton;
    public AppearingObject submitButton;


    public int numRounds;

    public bool isTutorial;

    private int currentRound = 0;

    private bool alreadyGuessed = false;
    private bool roundRunning = false;

    private bool alreadyGrabbed = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentRound = 0;
        //SetupWorldCamera();
        emitterVisual.SetInvisible();
        currentGuessSphere.SetInvisible();
        currentGuessSphere.transform.position = new Vector3(0, 0, 0.25f) + Vector3.up * 1.3f;

        submitButton = GameObject.Find("B_Highlight").GetComponent<AppearingObject>();
        grabButton = GameObject.Find("Grip_Highlight").GetComponent<AppearingObject>();

        grabButton.SetInvisible();
        submitButton.SetInvisible();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!roundRunning) return;
        if (OVRInput.GetDown(OVRInput.Button.Two) && alreadyGuessed == false) PlaceGuess();


        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger)>=0.5f && !alreadyGrabbed) Grab();
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.5f && alreadyGrabbed) UnGrab();

    }

    public void SetGrabButtonHighlighting(bool highlight)
    {
        if (highlight)
        {
            GUIAudioManager.PlayMenuOpen(grabButton.transform.position);
            grabButton.FadeIn();
        }
        else grabButton.FadeOut();
    }

    public void SetSubmitButtonHighlighting(bool highlight)
    {
        if (highlight)
        {
            GUIAudioManager.PlayHint(submitButton.transform.position);
            submitButton.FadeIn();
        }
        else submitButton.FadeOut();
    }

    public void SetCurrentGuessHighlight(bool highlight)
    {
        currentGuessHighlight.SetActive(highlight);
    }

    private void SetupWorldCamera()
    {
        Canvas[] canvasList = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasList)
        {
            canvas.worldCamera = Camera.main;
            Debug.Log(Camera.main);
        }
    }

    public void SetTutorial(bool isTut)
    {
        isTutorial = isTut;
    }



    public void StartCountdown()
    {
        Invoke("StartRound",3);
        HideGuessingPoint();
    }

    public void StartRound()
    {
        emitterVisual.FadeOut();
        SpawnNewSphere();

        currentGuessSphere.transform.position = new Vector3(0,0,0.25f) + Vector3.up * 1.3f;
        currentRound++;
        alreadyGuessed = false;
        roundRunning = true;
    }

    void SpawnNewSphere()
    {
        Vector3 newPosition = new Vector3(Random.Range(-3,3), Random.Range(0, 1.5f), Random.Range(-3, 3));
        emitter.transform.position = newPosition;
        emitter.EventReference = events[(int)Random.Range(0,3)];
        emitterVisual.minY = newPosition.y - 0.05f;
        emitterVisual.maxY = newPosition.y;
        StartSound();
    }

    public void PlaceGuess()
    {
        alreadyGuessed = true;
        roundRunning = false;
        float dist = Vector3.Distance(currentGuessSphere.transform.position, emitter.transform.position);
        emitterVisual.FadeIn();
        GUIAudioManager.PlaySelect(grabButton.transform.position);
        StopSound();

        if (isTutorial)
        {
            Invoke("OpenTutorialComplete", 2);
            GUIAudioManager.SetAmbientVolume(0.5f);
            return;
        }

        // save data

        if (currentRound <= numRounds)
        {
            Invoke("HideGuessingPoint", 3);
            Invoke("HideEmitter",3);
            Invoke("StartRound", 5);
        }
        else
        {
            windowManager.NextPage();
            GUIAudioManager.SetAmbientVolume(0.5f);
        }
    }

    public void SetAmbientVolume(float vol)
    {
        GUIAudioManager.SetAmbientVolume(vol);
    }

    private void OpenTutorialComplete()
    {
        windowManager.NextPage();
    }

    void StartSound()
    {
        emitter.Play();
    }

    void StopSound()
    {
        emitter.Stop();
    }

    void HideGuessingPoint()
    {
        currentGuessSphere.FadeOut();
        Invoke("ResetGuessingPoint", 1f);
    }

    void HideEmitter()
    {
        emitterVisual.FadeOut();
    }

    void ResetGuessingPoint()
    {
        currentGuessSphere.transform.position = new Vector3(0, 0, 0.25f) + Vector3.up * 1.3f;
        Invoke("ShowGuessingPoint", 1f);
    }

    public void ShowGuessingPoint()
    {
        currentGuessSphere.FadeIn();
    }

    public void PlayAppearingSound(int pos)
    {
        switch (pos)
        {
            case 0:
                GUIAudioManager.PlayHint(currentGuessSphere.transform.position);
                break;
            case 1:
                GUIAudioManager.PlayHint(emitter.transform.position);
                break;
        }
        
    }

    private void Grab()
    {
        Vib(0,0.2f);
        alreadyGrabbed = true;
    }

    private void UnGrab()
    {
        Vib(0,0.1f);
        alreadyGrabbed = false;
    }


    public void VibrationCue()
    {
        Vib(2, 2.5f);
    }


    public void Vib(float start, float end)
    {
        Invoke("startVib", start);
        Invoke("stopVib", end);
    }

    public void startVib()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
    public void stopVib()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
