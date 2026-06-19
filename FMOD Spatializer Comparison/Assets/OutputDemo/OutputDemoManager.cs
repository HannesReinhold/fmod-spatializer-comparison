using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OutputDemoManager : MonoBehaviour
{

    public DemoState startDemoState;
    public DemoState demoState;

    public List<GameObject> chapters;

    public OVRPassthroughLayer passthroughLayer;

    [Header("Managers")]
    public AlignmentManager alignmentManager;
    public VFXManager vfxManager;


    void Start()
    {
        
    }

    void Update()
    {
        
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



    // Scene management

    public void StartDemo()
    {
        demoState = DemoState.Alignment;
        alignmentManager.StartAlignment();
    }

    public void StartIntro()
    {
        
    }

    public void StartMRExplanation()
    {
        
    }

    public void StartMonoExplanation()
    {
        
    }

    public void StartStereoExplanation()
    {
        
    }

    public void StartSpatialExplanation()
    {
        
    }

    public void StartILDExplanation()
    {
        
    }

    public void StartITDExplanation()
    {
        
    }

    public void StartHRTFExplanation()
    {
        
    }

    public void StartOcclusionExplanation()
    {
        
    }

    public void StartReverbExplanation()
    {
        
    }

    public void StartDemoApartment()
    {
        
    }

    public void StartDemoConcert()
    {
        
    }

    public void StartDemoRobots()
    {
        
    }
    public void Finish()
    {
        
    }
}


public enum DemoState
{
    Alignment,
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