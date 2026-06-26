using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OutputDemoManager : MonoBehaviour
{

    public DemoState startDemoState;
    public DemoState demoState;

    public List<GameObject> chapters;

    public OVRPassthroughLayer passthroughLayer;

    public PopupWindow centerIndicator;
    private float positionTimer=0;

    [Header("Managers")]
    public AlignmentManager alignmentManager;
    public VFXManager vfxManager;
    public SpatialExplainManager spatialExplainManager;
    public ApartmentManager apartmentManager;
    public IntroManager introManager;
    public NarratorManager narratorManager;
    public OrchestraManager orchestraManager;



    void Start()
    {
        StartMonoExplanation();
    }

    void Update()
    {
        switch (demoState)
        {
            case DemoState.Alignment:
            alignmentManager.UpdateAlignment();
                break;
            case DemoState.Positioning:
                UpdatePositioning();
                break;
        }
    }

    // Updates
    private void UpdatePositioning()
    {
        float dist = Vector3.Distance(Camera.main.transform.position, centerIndicator.transform.position);
        Debug.Log("Dist: "+dist);
        if (dist <2)
        {
            positionTimer+= Time.deltaTime;
            if(positionTimer> 2) StopPositioning();
        }
        else
        {
            positionTimer=0;
        }
    }

    // effects

    public void PassthroughFadeOut(float time)
    {
        LeanTween.value( gameObject, 1, 0, time ).setOnUpdate( (float val) => { passthroughLayer.textureOpacity = val; } );
    }

    public void PassthroughFadeIn(float time)
    {
        LeanTween.value( gameObject, 0, 1, time ).setOnUpdate( (float val) => { passthroughLayer.textureOpacity = val; } );
    }

    public void ShowCenterIndicator()
    {
        centerIndicator.Open();
    }

    public void HideCenterIndicator()
    {
        centerIndicator.Close();
    }



    // Scene management

    public void StartAligning()
    {
        Debug.Log("Start Aligning");
        demoState = DemoState.Alignment;
    }

    public void StopAligning()
    {
        Debug.Log("Stop Aligning");
        demoState = DemoState.Positioning;
        Invoke("StartPositioning",1);
    }

    public void StartPositioning()
    {
        Debug.Log("Start Positioning");
        demoState = DemoState.Positioning;
        ShowCenterIndicator();
    }

    public void StopPositioning()
    {
        Debug.Log("Stop Positioning");
        demoState = DemoState.Intro;
        //HideCenterIndicator();
        StartIntro();
    }

    public void StartDemo()
    {
        Debug.Log("Start Demo");
        demoState = DemoState.Alignment;
        alignmentManager.StartAlignment();
    }

    public void StartIntro()
    {
        Debug.Log("Start Intro");
        introManager.StartIntro();
    }

    public void StartMRExplanation()
    {
        Debug.Log("Start MR Explanation");
    }

    public void StartMonoExplanation()
    {
        Debug.Log("Start Explanation");
        spatialExplainManager.StartExplainingProcedure();
    }

    public void StartStereoExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartSpatialExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartILDExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartITDExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartHRTFExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartOcclusionExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartReverbExplanation()
    {
        Debug.Log("Start Explanation");
    }

    public void StartDemoApartment()
    {
        Debug.Log("Start Apartment");
        apartmentManager.StartApartment();
        demoState = DemoState.DemoApartment;
    }

    public void StartDemoConcert()
    {
        Debug.Log("Start Concert");
        orchestraManager.StartOrchestra();
        demoState = DemoState.DemoConcert;
    }

    public void StartDemoRobots()
    {
        Debug.Log("Start Robots");
    }
    public void Finish()
    {
        Debug.Log("Finish");
    }
}


public enum DemoState
{
    Alignment,
    Positioning,
    Intro,
    ExplainMR,
    ExplainMono,
    ExplainStereo,
    ExplainILD,
    ExplainITD,
    ExplainHRTF,
    ExplainOcclusion,
    ExplainReverb,
    DemoApartment,
    DemoConcert,
    StartDemoRobots,
    Finish
}