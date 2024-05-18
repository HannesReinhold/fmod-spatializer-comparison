using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectiveEvaluationManager : MonoBehaviour
{

    public GameObject introduction;
    public GameObject tutorial;
    public GameObject evaluationRound;
    public GameObject finish;

    public SubjectiveEvaluationRound roundManager;
    public WindowManager windowManager;
    //public SubjectiveEvaluationInterface1 subjectiveEvalInterface;

    public List<GameObject> speakers = new List<GameObject>();

    private int numParts;

    private int roundID = 0;
    private int partID = 0;

    public bool skipTutorial = false;

    FMOD.Studio.Bus bus;

    private int currentEmergingSpeaker = 0;

    public float speakerStartHeight = -4.66f;

    private void OnEnable()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
        HideAll();
        StartEvalution();
        
    }

    private void OnDisable()
    {
        ResetValues();
    }


    private void HideAll()
    {
        introduction.SetActive(false);
        tutorial.SetActive(false);
        evaluationRound.SetActive(false);
    }

    
    public void StartEvalution()
    {
        finish.SetActive(false);
        evaluationRound.SetActive(false);
        Debug.Log(GameManager.Instance.dataManager);
        DisableHighlighting();
        numParts = GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts.Count;
        if (!skipTutorial) introduction.SetActive(true);
        else { 
            StartRound(); 
            roundID++;
        }

        //GUIAudioManager.SetAmbientVolume(0);
        GameManager.Instance.LogServerEvent("Subjective Evaluation");

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
        numParts = GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts.Count;
        roundManager.UpdateInterface(GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts[partID], roundID);
        roundManager.StartRound(true);
        tutorial.SetActive(false);
        evaluationRound.SetActive(true);
        roundID++;

        GameManager.Instance.LogServerEvent("Subjective Evaluation Round");
    }

    public void NextRound()
    {
        bool nextAspect=false;
        if (roundID >= 4)
        {
            roundID = 0;
            partID++;
            nextAspect = true;
        }

        if (partID>=numParts) FinishEvaluation();
        else {
            roundManager.UpdateInterface(GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts[partID], roundID);
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
            LeanTween.moveY(speakers[i], speakerStartHeight,0).setEaseOutCubic();
            speakers[i].GetComponentInChildren<Hint>().CloseHint();
        }

        tutorial.SetActive(false);
        introduction.SetActive(false);

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
        GameManager.Instance.StartDirectionGuessing();
    }
}
