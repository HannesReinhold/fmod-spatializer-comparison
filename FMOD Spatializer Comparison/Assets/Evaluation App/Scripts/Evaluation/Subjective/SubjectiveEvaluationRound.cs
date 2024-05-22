using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SubjectiveEvaluationRound : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI minText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI aspectText;

    public SubjectiveAudioSwitch audioSwitch;

    public ToggleGroup spatializerSwitch;
    public UnityEngine.UI.Slider ratingSlider;

    private ConcreteSubjectiveData roundData;
    public SubjectiveEvaluationManager manager;
    private WindowManager windowManager;

    public SPatializerSwitchManager spatialManager;


    public void UpdateInterface(ConcreteSubjectiveData data, int roundID)
    {
        this.roundData = data;

        nameText.text = data.attribute;
        descriptionText.text = data.description;
        aspectText.text = "This part compares the aspect: "+data.attribute;
        questionText.text = data.description;
        minText.text = ""; 
        maxText.text = "";

    }

    FMOD.Studio.Bus bus;

    private void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
        bus.setVolume(1);
        

    }

    private void Awake()
    {
        windowManager = GetComponent<WindowManager>();
    }

    public void SaveRound()
    {
        roundData.rating = (int)ratingSlider.value;
        GameManager.Instance.dataManager.currentSessionData.subjectiveData.Add(roundData);
        //audioSwitch.Stop();
        GameManager.Instance.SaveData();
    }

    public void StartRound(bool nextAspect)
    {
        //sync.SetAudioOutput(false,);

        windowManager = GetComponent<WindowManager>();
        if (nextAspect) windowManager.OpenPage(0);
        else
        {
            windowManager.OpenPage(1);
            
        }

        spatialManager.SetSpatializerPair(roundData.spatializerA, roundData.spatializerB);

        //audioSwitch.SetAll(true, roundData.speakerID, roundData.comparisonSpatializerID);
        //audioSwitch.Play(partData.fileID);

        //manager.HighlightSpeaker(roundData.speakerID);

    }

    public void UpdateRatingValue()
    {
        roundData.rating = (int)ratingSlider.value;
    }


    public void ToggleSpatializer(int index)
    {
        spatialManager.SetSpatializer(index == 0 ? spatialManager.spatializerA : spatialManager.spatializerB);
    }

    public void SetSpeaker(int id)
    {
        audioSwitch.SetSpeaker(id);
    }

}
