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
    public TextMeshProUGUI aspectDescription;

    public SubjectiveAudioSwitch audioSwitch;

    public ToggleGroup spatializerSwitch;
    public UnityEngine.UI.Toggle t1;
    public UnityEngine.UI.Toggle t2;
    public TextMeshProUGUI currentSpatializerVisual;
    public UnityEngine.UI.Slider ratingSlider;

    private ConcreteSubjectiveData roundData;
    public SubjectiveEvaluationManager manager;
    private WindowManager windowManager;

    public SPatializerSwitchManager spatialManager;

    private int setSpat = 0;

    private bool roundRunning = false;
    private bool showRatingInterface = false;

    public TextMeshProUGUI aspect;



    public void UpdateInterface(ConcreteSubjectiveData data, int roundID)
    {
        this.roundData = data;

        nameText.text = data.attribute;
        descriptionText.text = data.description + "\n\n <color=#7FD6FC>Tip:</color>         Use the slider below to enter your choice.<color=#7FD6FC>Tip:</color>         Use the Radio buttons to switch between the 2 audio sources.";
        aspectText.text = "Aspect for this round:  <color=#7FD6FC>" + data.attribute+ "</color>";
        aspectDescription.text = data.description;
        //questionText.text = "<color=#7FD6FC>Attribute:</color> "+data.description + "\n\n <color=#7FD6FC>Tip:</color>         Use the slider below to enter your choice.\n<color=#7FD6FC>Tip:</color>         Use the Radio buttons to switch between the 2 audio sources.";
        minText.text = ""; 
        maxText.text = "";
        aspect.text = data.attribute;

    }

    FMOD.Studio.Bus bus;

    private void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
        bus.setVolume(1);
        
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && setSpat == 1) { setSpat = 0; ToggleSpatializer(0); };
        if (OVRInput.GetDown(OVRInput.Button.Two) && setSpat == 0) { setSpat = 1;  ToggleSpatializer(1); };


        if (!roundRunning) return;
        if (OVRInput.GetDown(OVRInput.Button.Start)) ToggleInterface();

        if (Input.GetMouseButtonDown(0)) ToggleInterface();
    }

    private void ToggleInterface()
    {
        showRatingInterface = !showRatingInterface;
        if (showRatingInterface) windowManager.OpenCurrentWindow();
        else windowManager.CloseCurrentWindow();
    }

    private void OpenInterface()
    {
        showRatingInterface = true;
        if (showRatingInterface) windowManager.OpenCurrentWindow();
        else windowManager.CloseCurrentWindow();
    }

    public void SetRoundState(bool running)
    {
        roundRunning = running;
        showRatingInterface = false;
        windowManager.CloseCurrentWindow();
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
        setSpat = 0;
        //t1.isOn = true;
        //t2.isOn = false;
        spatialManager.SetSpatializer(0);

        GameManager.Instance.OpenSpatializerSwitchWindow();

        Invoke("OpenInterface",30);
    }

    public void UpdateRatingValue()
    {
        roundData.rating = (int)ratingSlider.value;
    }


    public void ToggleSpatializer(int index)
    {
        spatialManager.SetSpatializer(index == 0 ? spatialManager.spatializerA : spatialManager.spatializerB);
        currentSpatializerVisual.text = index == 0 ? "A" : "B";

        if (index == 0) VibA();
        else VibB();
    }
    public void ToggleSpatializerButton(int index)
    {
        if (index == 0) t1.isOn = true;
        else t2.isOn = true;

        
    }


    public void SetSpeaker(int id)
    {
        audioSwitch.SetSpeaker(id);
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

}
