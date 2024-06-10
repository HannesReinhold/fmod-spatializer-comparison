using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicListeningManager : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;
    public AppearingObject emitterVisual;

    public AppearingObject currentGuessSphere;
    public GameObject currentGuessHighlight;

    public LineRenderer distanceLine;

    public List<FMODUnity.EventReference> events;

    public List<FMODUnity.EventReference> tutorialEvents;

    [SerializeField] public AudioEvent[] eventRefs;

    public WindowManager windowManager;

    public AppearingObject grabButton;
    public AppearingObject submitButton;


    public int numRounds;

    public int maxRounds = 5;

    public bool isTutorial;
    public bool isTest = false;

    private int currentRound = 0;
    private int currentSpatializer = 0;
    private int currentStimuli = 0;

    private bool alreadyGuessed = false;
    private bool roundRunning = false;

    private bool alreadyGrabbed = false;

    public Vector3 dimensions;

    private Vector3 lastPosition = new Vector3();

    private float startTime = 0;

    // Start is called before the first frame update

    private ConcretePositionGuessingData roundData;

    private List<Vector3> errorList = new List<Vector3>();

    public List<TextMeshProUGUI> scoreText;

    public bool showTarget = false;



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

        //GameManager.Instance.LogServerEvent("Position introduction");
    }

    // Update is called once per frame
    void Update()
    {
        if (!roundRunning) return;
        if (OVRInput.GetDown(OVRInput.Button.Two) && alreadyGuessed == false) PlaceGuess();
        if (Input.GetKeyDown(KeyCode.Return) && alreadyGuessed == false) PlaceGuess();


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
        emitterVisual.FadeOut();
        Invoke("StartRound",3);
        HideGuessingPoint();
    }

    public void StartRound()
    {
        emitterVisual.FadeOut();
        SpawnNewSphere();

        currentGuessSphere.transform.position = new Vector3(0,0,0.55f) + Vector3.up * 1.3f;
        currentRound++;
        alreadyGuessed = false;
        roundRunning = true;

        roundData = new ConcretePositionGuessingData();

        startTime = Time.time;

        //GameManager.Instance.LogServerEvent("Position round: " + currentRound);

        //GameManager.Instance.rayObject.SetActive(false);
    }

    void SpawnNewSphere()
    {
        Vector3 newPosition = GenerateNewPosition();
        lastPosition = newPosition;
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
        if(showTarget) emitterVisual.FadeIn();
        GUIAudioManager.PlaySelect(grabButton.transform.position);
        StopSound();

        if (GameManager.Instance != null) GameManager.Instance.Vib(0,0.2f);

        if (isTutorial)
        {
            Invoke("OpenTutorialComplete", 2);
            GUIAudioManager.SetAmbientVolume(0.5f);
            return;
        }

        Vector3 actual = emitter.transform.position;
        Vector3 guess = currentGuessSphere.transform.position;

        // save data
        roundData.roundID = currentRound;
        roundData.spatializerID = currentSpatializer;
        roundData.stimuliID = currentStimuli;
        roundData.actualPosition = emitter.transform.position;
        roundData.guessedPosition = currentGuessSphere.transform.position;
        roundData.totalDifference = dist;
        roundData.horizontalDifference = Vector2.Distance(new Vector2(actual.x, actual.z),  new Vector2(guess.x, guess.z));
        roundData.verticalDifference = Mathf.Abs(actual.y- guess.y);
        roundData.timeToGuess = Time.time - startTime;
        if(GameManager.Instance!=null) GameManager.Instance.dataManager.currentSessionData.positionGuessData.Add(roundData);
        if (GameManager.Instance != null) GameManager.Instance.SaveData();


        if (currentRound <= numRounds && currentRound <= maxRounds)
        {
            Invoke("HideGuessingPoint", 3);
            Invoke("HideEmitter",3);
            Invoke("StartRound", 5);

            errorList.Add(new Vector3(dist, Vector2.Distance(new Vector2(actual.x, actual.z), new Vector2(guess.x, guess.z)), Mathf.Abs(actual.y - guess.y)));
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.rayObject.SetActive(true);
            windowManager.NextPage();
            GUIAudioManager.SetAmbientVolume(0.5f);
            emitterVisual.SetInvisible();
            currentGuessSphere.SetInvisible();

            if (isTest)
            {
                Vector3 avrError = new Vector3();

                foreach(Vector3 v in errorList)
                {
                    avrError += v;
                }
                avrError /= (float)numRounds;
                Debug.Log(avrError);
                scoreText[0].text = "Total: " + avrError.x;
                scoreText[1].text = "Hor: : " + avrError.y;
                scoreText[2].text = "Vert: " + avrError.z;
            }
            
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
        PlayAudioCue();
    }

    void StopSound()
    {
        emitter.Stop();
    }

    private void PlayAudioCue()
    {
        currentStimuli = Random.Range(0,3);
        currentSpatializer = Random.Range(0, 3);
        Debug.Log("Playing spatializer " + currentSpatializer + " at " + emitter.transform.position);
        emitter.Stop();
        switch (currentSpatializer)
        {
            //case 0: FMODUnity.RuntimeManager.PlayOneShot(events[cueID].spatializedEvents[0], target.transform.position); break;
            //case 1: FMODUnity.RuntimeManager.PlayOneShot(events[cueID].spatializedEvents[1], target.transform.position); break;
            //case 2: FMODUnity.RuntimeManager.PlayOneShot(events[cueID].spatializedEvents[2], target.transform.position); break; break;
            case 0:
                emitter.ChangeEvent(eventRefs[currentStimuli].spatializedEvents[0]);
                emitter.Play();
                break;
            case 1:
                emitter.ChangeEvent(eventRefs[currentStimuli].spatializedEvents[1]);
                emitter.Play();
                break;
            case 2:
                emitter.ChangeEvent(eventRefs[currentStimuli].spatializedEvents[2]);
                emitter.Play();
                break;
        }
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

    public Vector3 GenerateNewPosition()
    {
        Vector3 newPosition = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0.5f, 2f), Random.Range(-3.5f, 3.5f));
        int i = 0;
        while (Vector3.Distance(newPosition, lastPosition) < 1 && i<10)
        {
            newPosition = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(0.5f, 2f), Random.Range(-3.5f, 3.5f));
            i++;
        }


        return newPosition;
    }

    public void SetTargetVisibility(bool vsi)
    {
        showTarget = vsi;
    }

}
