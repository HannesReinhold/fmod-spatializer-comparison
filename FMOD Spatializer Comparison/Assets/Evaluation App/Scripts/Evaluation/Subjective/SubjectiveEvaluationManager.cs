using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubjectiveEvaluationManager : MonoBehaviour
{

    public GameObject introduction;
    public GameObject tutorial;
    public GameObject evaluationRound;
    public GameObject finish;

    public SubjectiveEvaluationRound roundManager;
    public WindowManager windowManager;

    public SPatializerSwitchManager switchManagerTutorial;

    public TextMeshProUGUI currentSpatializerVisual;

    public List<GameObject> speakers = new List<GameObject>();

    private int numRounds;

    private int roundID = 0;
    private int partID = 0;

    public bool skipTutorial = false;

    FMOD.Studio.Bus bus;

    private int currentEmergingSpeaker = 0;

    public float speakerStartHeight = -4.66f;

    private bool roundRunning = false;
    private bool showRatingInterface=false;

    private bool interfaceOpen = false;

    private int setSpat = 0;

    public PopupWindow aspectHintWindow;
    public TextMeshProUGUI aspectHintText;
    

    private void OnEnable()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
        HideAll();

        if (GameManager.Instance.dataManager != null) StartEvalution();
        else Invoke("StartEvaluation", 0.5f);
        GUIAudioManager.SetTutorialVolume(1);


    }

    private void Start()
    {
        GUIAudioManager.SetTutorialVolume(1);
        aspectHintWindow.Close();
    }

    private void Update()
    {
        if (!roundRunning) return;
        if (OVRInput.GetDown(OVRInput.Button.Start)) ToggleInterface();

        if (OVRInput.GetDown(OVRInput.Button.One) && setSpat == 1) { setSpat = 0; ToggleSpatializer(0); };
        if (OVRInput.GetDown(OVRInput.Button.Two) && setSpat == 0) { setSpat = 1; ToggleSpatializer(1); };

        if (Input.GetMouseButtonDown(0)) ToggleInterface();

    }

    public void ToggleSpatializer(int index)
    {
        switchManagerTutorial.SetSpatializer(index == 0 ? switchManagerTutorial.spatializerA : switchManagerTutorial.spatializerB);
        currentSpatializerVisual.text = index == 0 ? "A" : "B";
        if (index == 0) VibA();
        else VibB();
    }

    public void VibA()
    {
        GameManager.Instance.Vib(0, 0.05f);
    }

    public void VibB()
    {
        VibA();
        Invoke("VibA", 0.25f);
    }

    public void SetAspectHintWindowVisibility(bool vis)
    {
        if (vis) aspectHintWindow.Open();
        else aspectHintWindow.Close();
    }

    public void SetAspectHintWindowText(string txt)
    {
        aspectHintText.text = txt;
    }

    private void ToggleInterface()
    {
        showRatingInterface = !showRatingInterface;
        if (showRatingInterface) windowManager.OpenCurrentWindow();
        else windowManager.CloseCurrentWindow();
    }

    private void OnDisable()
    {
        ResetValues();
    }


    private void HideAll()
    {
        //introduction.SetActive(false);
        //tutorial.SetActive(false);
        //evaluationRound.SetActive(false);
    }

    
    public void StartEvalution()
    {
        finish.SetActive(false);
        evaluationRound.SetActive(false);
        Debug.Log(GameManager.Instance.dataManager);
        //DisableHighlighting();
        numRounds = GameManager.Instance.dataManager.spatializerData.subjectiveData.comparisons.Count;
        Debug.Log("numRounds:"+numRounds);
        //if (!skipTutorial) introduction.SetActive(true);

            //StartRound(); 
            //roundID++;


        //GUIAudioManager.SetAmbientVolume(0);
        //GameManager.Instance.LogServerEvent("Subjective Evaluation");

    }

    public void FinishEvaluation()
    {
        Debug.Log("End Evaluation");
        evaluationRound.SetActive(false);
        finish.SetActive(true);
        GUIAudioManager.SetAmbientVolume(0.5f);
        windowManager.ResetSlow();
    }

    public void StartRound()
    {
        Debug.Log("STart Round");
        //subjectiveEvalInterface.ShowNextEvaluation(partID, roundID);
        GUIAudioManager.SetAmbientVolume(0);
        int id = GameManager.Instance.dataManager.randomOrderIndices[roundID];
        numRounds = GameManager.Instance.dataManager.spatializerData.subjectiveData.comparisons.Count;
        roundManager.UpdateInterface(GameManager.Instance.dataManager.spatializerData.subjectiveData.comparisons[id], id);
        aspectHintText.text = GameManager.Instance.dataManager.spatializerData.subjectiveData.comparisons[id].attribute;
        roundManager.StartRound(true);
        //tutorial.SetActive(false);
        evaluationRound.SetActive(true);
        roundID++;

        //GameManager.Instance.LogServerEvent("Subjective Evaluation Round");
    }

    public void SetRoundState(bool running)
    {
        roundRunning = running;
        showRatingInterface = false;
        windowManager.CloseCurrentWindow();
    }

    public void NextRound()
    {
        bool nextAspect=true;
        
        if (roundID>= numRounds) FinishEvaluation();
        else {
            int id = GameManager.Instance.dataManager.randomOrderIndices[roundID];
            roundManager.UpdateInterface(GameManager.Instance.dataManager.spatializerData.subjectiveData.comparisons[id], id);
            roundManager.StartRound(nextAspect); 
            
        }
        roundID++;
    }



    public void SaveData()
    {
        roundManager.SaveRound();
    }

    public void StartTutorial()
    {
        tutorial.SetActive(true);
        introduction.SetActive(false);
    }


    int currentSpeaker = 0;
    public void EmergeSpeakers()
    {
        
        Invoke("EmergeSpeaker",1);
        Invoke("EmergeSpeaker", 2);
        Invoke("EmergeSpeaker", 3);
        Invoke("EmergeSpeaker", 4);
        Invoke("EmergeSpeaker", 5);
        Invoke("EmergeSpeaker", 6);
        Invoke("EmergeSpeaker", 7);
        Invoke("EmergeSpeaker", 8);

        Invoke("DisableHighlighting",8);
    }

    public void DisableHighlighting()
    {
        for(int i=0; i<speakers.Count; i++)
        {
            if(speakers[i]!=null) speakers[i].GetComponentInChildren<Hint>().CloseHint();
        }
    }

    private void EmergeSpeaker()
    {
        LeanTween.moveY(speakers[currentEmergingSpeaker], 0, 1).setEaseOutCubic();
        speakers[currentEmergingSpeaker].GetComponentInChildren<Hint>().OpenHint();
        currentEmergingSpeaker++;
    }

    public void HighlightSpeaker(int index)
    {
        for(int i=0; i<6; i++)
        {
            if(index==i) speakers[i].GetComponentInChildren<SpeakerHighlighter>().ApplyHighlight();
            else speakers[i].GetComponentInChildren<SpeakerHighlighter>().RemoveHighlight();
        }
    }



    public void ResetValues()
    {
        
        currentEmergingSpeaker = 0;
        for (int i = 0; i < speakers.Count; i++)
        {
            //LeanTween.moveY(speakers[i], speakerStartHeight,0).setEaseOutCubic();
            //speakers[i].GetComponentInChildren<Hint>().CloseHint();
        }

        //tutorial.SetActive(false);
        //introduction.SetActive(false);

        roundID = 0;
        partID = 0;
    }

    public void CompleteEvaluation()
    {
        finish.SetActive(false);
        Invoke("StartDirectionGuessing",2);
        ResetValues();
    }

    private void StartDirectionGuessing()
    {
        GameManager.Instance.StartComplete();
    }

    


}
