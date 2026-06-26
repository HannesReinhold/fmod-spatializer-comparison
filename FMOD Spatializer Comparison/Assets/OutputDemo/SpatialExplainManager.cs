using UnityEngine;
using System.Collections;
using FMODUnity;
using System;
using System.Numerics;
using FMOD.Studio;

public class SpatialExplainManager : MonoBehaviour
{
public OutputDemoManager demoManager;

    public PopupObject headVisualizer;
    public PopupObject monoSourceVisualizer;
    public PopupObject stereoSourceLeftVisualizer;
    public PopupObject stereoSourceRightVisualizer;
    public PopupObject spatialSourceVisualizer;

    public ParticleSystem monoSourceParticles;
    public ParticleSystem stereoSourceLeftParticles;
    public ParticleSystem stereoSourceRightParticles;
    public ParticleSystem spatialSourceParticles;
    public ParticleSystem spatialSourceParticles360;

    public float stereoSourcesDistance = 0.5f;

    public AudioPathManager pathManager;

    public PopupObject occlusionWall;
    public PopupObject roomGeometry;
    public RaytracingVisualizer raytracingVisualizer;

    public StudioEventEmitter ambienceEmitter;

    private float currentStereoDistance = 0;
    private float targetStereoDistance=0;

    public int timeOffset=0;

    public EventReference hintEvent;

    public Transform leftEar;
    public Transform rightEar;

    bool  itdOpen=false;
    bool ildOpen=false;

    public AudioPhenomena spatialPhenomena;

    float stereopanning=0.0f;

    bool reverb=false;




    public void ShowHead()
    {
        Debug.Log("Show Head");
        headVisualizer.Open();
    }

    public void HideHead()
    {
        headVisualizer.Close();
    }

    public void ShowMonoSource()
    {
        Debug.Log("Show Mono Source");
        monoSourceVisualizer.Open();
    }

    public void HideMonoSource()
    {
        Debug.Log("Close Mono Source");
        monoSourceVisualizer.Close();
    }

    public void ShowLeftStereoSource()
    {
        Debug.Log("Show Left Stereo Source");
        stereoSourceLeftVisualizer.Open();
    }

    public void HideLeftStereoSource()
    {
        Debug.Log("Hide Left Stereo Source");
        stereoSourceLeftVisualizer.Close();
    }

    public void ShowRightStereoSource()
    {
        Debug.Log("Show Right Stereo Source");
        stereoSourceRightVisualizer.Open();
    }

    public void HideRightStereoSource()
    {
        Debug.Log("Hide Right Stereo Source");
        stereoSourceRightVisualizer.Close();
    }

    public void ShowSpatialSource()
    {
        Debug.Log("Show Spatial Source");
        spatialSourceVisualizer.Open();
    }

    public void HideSpatialSource()
    {
        Debug.Log("Hide Spatial Source");
        spatialSourceVisualizer.Close();
    }

    public void StartMonoSourcePlaying()
    {
        Debug.Log("Play Mono Source");
        monoSourceParticles.Play();
    }

    public void StopMonoSourcePlaying()
    {
        Debug.Log("Stop Mono Source");
        monoSourceParticles.Stop();
    }

    public void StartStereoLeftSourcePlaying()
    {
        Debug.Log("Start Left Stereo Source");
        stereoSourceLeftParticles.Play();
    }

    public void StopStereoLeftSourcePlaying()
    {
        Debug.Log("Stop Left Stereo Source");
        stereoSourceLeftParticles.Stop();
    }

    public void StartStereoRightSourcePlaying()
    {
        Debug.Log("Start Right Stereo Source");
        stereoSourceRightParticles.Play();
    }

    public void StopStereoRightSourcePlaying()
    {
        Debug.Log("Stop Right Stereo Source");
        stereoSourceRightParticles.Stop();
    }

    public void StartSpatialSourceDirectionalPlaying()
    {
        Debug.Log("Start Spatial Source");
        spatialSourceParticles.Play();
    }

    public void StartSpatialSource360Playing()
    {
        Debug.Log("Start Spatial Source");
        spatialSourceParticles360.Play();
    }

    public void StopSpatialSourceDirectionalPlaying()
    {
        Debug.Log("Stop Spatial Source");
        spatialSourceParticles.Stop();
    }

    public void StopSpatialSource360Playing()
    {
        Debug.Log("Stop Spatial Source");
        spatialSourceParticles360.Stop();
    }

    public void SplitStereoSources()
    {
        Debug.Log("Split Stereo Source");
        targetStereoDistance = stereoSourcesDistance;
    }

    public void MergeStereoSources()
    {
        Debug.Log("Merge Stereo Sources");
        targetStereoDistance = 0;
    }

    private void UpdateStereoSources()
    {
        UnityEngine.Vector3 pos = headVisualizer.transform.parent.position;
        UnityEngine.Vector3 posLeft = pos - headVisualizer.transform.parent.right * currentStereoDistance - headVisualizer.transform.parent.up * 0.193f;
        UnityEngine.Vector3 posRight = pos + headVisualizer.transform.parent.right * currentStereoDistance - headVisualizer.transform.parent.up * 0.193f;
        //posLeft.y = h;
        //posRight.y = h;
        stereoSourceLeftVisualizer.transform.parent.position = posLeft;
        stereoSourceRightVisualizer.transform.parent.position = posRight;

        currentStereoDistance = Mathf.Lerp(currentStereoDistance, targetStereoDistance, Time.deltaTime);
    }

    public void SetPanning(float panning, float duration)
    {
      LeanTween.value(stereopanning, panning, duration).setOnUpdate(UpdatePanning);
      
    }


    void UpdatePanning(float a)
    {
        stereopanning=a;
        demoManager.narratorManager.SetVoiceParameter("StereoPan",a);
    }

    IEnumerator DelayedSetPanning(float tilt, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        SetPanning(tilt, duration);

    }
    IEnumerator DelayedSetRoomSIze(float size, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        LeanTween.scale(roomGeometry.transform.parent.gameObject, UnityEngine.Vector3.one+new UnityEngine.Vector3(1,0.2f,1)*size,duration);

    }

    private float reverbAmount=0;
    public void SetReverb(float reverb, float duration)
    {
      LeanTween.value(reverbAmount, reverb, duration).setOnUpdate(UpdateReverb);
      
    }


    void UpdateReverb(float a)
    {
        reverbAmount=a;
        demoManager.narratorManager.SetVoiceParameter("RoomReverbMix",a);
    }

    IEnumerator DelayedSetReverb(float tilt, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        SetReverb(tilt, duration);

    }

    private void SetHeadTilt(UnityEngine.Vector3 tilt, float duration)
    {
        Debug.Log("Set Head Tilt");
        LeanTween.rotate(headVisualizer.transform.parent.gameObject, tilt, duration).setEaseInOutSine();
    }


    IEnumerator DelayedHeadTilt(UnityEngine.Vector3 tilt, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        SetHeadTilt(tilt, duration);

    }

    private void SetSpatializedSourcePos(UnityEngine.Vector3 pos, float duration)
    {
        UnityEngine.Vector3 newPos = headVisualizer.transform.parent.position + headVisualizer.transform.parent.up;
        Debug.Log("Move SOurce to "+newPos);
        LeanTween.moveLocal(spatialSourceVisualizer.transform.parent.gameObject, pos, duration).setEaseInOutSine();
    }


    IEnumerator DelayedSpatializedSourcePos(UnityEngine.Vector3 pos, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        SetSpatializedSourcePos(pos, duration);

    }

    public void ShowSoundShadow()
    {
        spatialSourceParticles.GetComponent<SoundHeadTrigger>().lifetimeInsideTrigger = 0.27f;
    }

    public void HideSoundShadow()
    {
        spatialSourceParticles.GetComponent<SoundHeadTrigger>().lifetimeInsideTrigger = 1;
    }

    private void SetAudioPath(int pathIndex)
    {
        Debug.Log("Set Path to"+pathIndex);
        pathManager.SetTargetIndex(pathIndex);
    }


    IEnumerator DelayedAudioPath(int pathIndex, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetAudioPath(pathIndex);
    }

    private void SetOcclusionWallOpen(bool open)
    {
        Debug.Log("Set Occ Open");
        if(open)
            occlusionWall.Open();
        else 
            occlusionWall.Close();
    }


    IEnumerator DelayedOcclusionWallOpen(bool open, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetOcclusionWallOpen(open);
    }

    private void SetRoomGeometryOpen(bool open)
    {
        Debug.Log("Set Room Open");
        if (open)
            roomGeometry.Open();
        else
            roomGeometry.Close();
    }


    IEnumerator DelayedRoomGeometryOpen(bool open, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetRoomGeometryOpen(open);
    }

    private void SetRaysVisible(bool visible)
    {
        Debug.Log("Set Room Open");
        if (visible)
            raytracingVisualizer.ShowAllRays();
        else
            raytracingVisualizer.HideAllRays();
    }


    IEnumerator DelayedRaysVisible(bool open, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetRaysVisible(open);
    }

    private void StartNextScenario()
    {
        demoManager.StartDemoApartment();
        ambienceEmitter.Stop();
    }



    public void StartExplainingProcedure()
    {
        ambienceEmitter.Play();
        FMODUnity.RuntimeManager.PlayOneShot(hintEvent, headVisualizer.transform.position);
        demoManager.narratorManager.SetPosition(headVisualizer.transform.position,0.5f);
        StartCoroutine(demoManager.narratorManager.DelayedHide(1));
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(2, 2)); // explain mono
        // mono
        // spawn head
        Invoke("ShowHead",1); 
        //Spawn source
        Invoke("ShowMonoSource",3); // 3
        // play delayed 
        Invoke("StartMonoSourcePlaying",4); // 4
        // after some time stop mono
        Invoke("StopMonoSourcePlaying",17); // 10
        // hide it
        Invoke("HideMonoSource", 14); //11


        // stereo start at 20
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(3, 15)); // explain stereo
        StartCoroutine(DelayedSetPanning(0.5f,1,15));
        Invoke("ShowLeftStereoSource",15);
        Invoke("ShowRightStereoSource",15);
        Invoke("SplitStereoSources",15+1);
        // start playing
        Invoke("StartStereoLeftSourcePlaying", 15+2); // 13
        Invoke("StartStereoRightSourcePlaying", 15+2);
        StartCoroutine(DelayedSetPanning(0.2f,1,15+2));
        StartCoroutine(DelayedSetPanning(0.8f,1,15+3));
        // moving and tilting head // 17
        StartCoroutine(DelayedHeadTilt(new UnityEngine.Vector3(0,180,50),2,15+7));
        StartCoroutine(DelayedHeadTilt(new UnityEngine.Vector3(0, 180, -50), 4, 15+9));
        StartCoroutine(DelayedHeadTilt(new UnityEngine.Vector3(0, 180, 0), 2, 15+13));
        StartCoroutine(DelayedSetPanning(0.5f,1,15+14));
        // start playing
        Invoke("StopStereoLeftSourcePlaying", 15+15); // 25
        Invoke("StopStereoRightSourcePlaying", 15+15);
        // merge
        Invoke("MergeStereoSources",15+16);
        // hide
        Invoke("HideLeftStereoSource", 15+17); // 26
        Invoke("HideRightStereoSource", 15+17);

        // itd start at 38
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(4, 34)); // explain itd
        Invoke("ShowITD",34+1);
        // show source in front
        Invoke("ShowSpatialSource", 34+1); // 28
        StartCoroutine(DelayedSpatializedSourcePos(new UnityEngine.Vector3(-0.5f,0.7f,0),1,34+1));
        // play
        Invoke("StartSpatialSourceDirectionalPlaying", 34+2); // 29
        //move slowly to left
        StartCoroutine(DelayedSpatializedSourcePos(new UnityEngine.Vector3(-1,0.4f,0), 2, 34+6));
        // move slowly to right
        StartCoroutine(DelayedSpatializedSourcePos(new UnityEngine.Vector3(1, 0.4f, 0), 4, 34+12));
        // move to center
        StartCoroutine(DelayedSpatializedSourcePos(new UnityEngine.Vector3(0, 0.4f, 0), 2, 34+16));
        Invoke("HideITD",34+19);

        //ild
        Invoke("SHowILD",53);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(5, 53)); // explain ild
        // show sound shadow
        Invoke("ShowSoundShadow", 53+1);
        // move left
        StartCoroutine(DelayedSpatializedSourcePos(new UnityEngine.Vector3(-1, 0.4f, 0), 2, 53+2));
        // move right
        StartCoroutine(DelayedSpatializedSourcePos(new UnityEngine.Vector3(1, 0.4f, 0), 6, 53+10));
        // hide shadow
        Invoke("HideSoundShadow", 53+15);
        Invoke("HideILD",53+16);

        //hrtf
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(6, 70)); // explain hrtf
        StartCoroutine(demoManager.narratorManager.DelayedSetFollowTarget(spatialSourceVisualizer.transform.parent, 70));
        // show 3d field around head

        // move source around the head
        StartCoroutine(DelayedAudioPath(5, 71));

        //occlusion
        /*
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(6, 70)); // explain occlusion
        Invoke("HideHead", 70);
        Invoke("StopSpatialSourceDirectionalPlaying", 70);
        Invoke("StartSpatialSource360Playing", 71);
        // spawn wall in front of player
        StartCoroutine(DelayedOcclusionWallOpen(true, 72));
        // move source around wall
        StartCoroutine(DelayedAudioPath(6, 73));
        // close Wall
        StartCoroutine(DelayedOcclusionWallOpen(false, 80));
        */

        //reverb

        // show room geometry
        Invoke("HideHead", 96);
        Invoke("StopSpatialSourceDirectionalPlaying", 96);
        Invoke("StartSpatialSource360Playing", 97);
        StartCoroutine(demoManager.narratorManager.DelayedPlayVoiceline(7, 97)); // explain reverb
        StartCoroutine(DelayedRoomGeometryOpen(true, 97+2));
        Invoke("ShowReverb", 97+5);
        StartCoroutine(DelayedSetReverb(1.0f,1,97+8));
        // begin with reflection visualization
        StartCoroutine(DelayedRaysVisible(true, 97+7));
        StartCoroutine(DelayedSetReverb(0.2f,1,97+11));
        // hide rays
        StartCoroutine(DelayedRaysVisible(false, 97+13));
        //move source around room
        //StartCoroutine(DelayedAudioPath(6, 107));
        //hide
        Invoke("StopSpatialSource360Playing", 97+15); // 29
        Invoke("HideSpatialSource", 97+16); // 29
        //StartCoroutine(DelayedRoomGeometryOpen(false, 97+18));
        StartCoroutine(DelayedSetRoomSIze(4,1,97+17));
        StartCoroutine(DelayedSetRoomSIze(1,1,97+19.5f));
        Invoke("HideReverb", 97+17);
        StartCoroutine(DelayedSetReverb(0,1,97+20));
        StartCoroutine(DelayedRoomGeometryOpen(false, 97+21));
        StartCoroutine(demoManager.narratorManager.DelayedSetFollowTarget(null, 117));
        StartCoroutine(demoManager.narratorManager.DelayedSetFacingCamera(new UnityEngine.Vector3(0,0,1),0,117));
        Invoke("HideHead",117);

        Invoke("StartNextScenario",118);
    
    }

    void Update()
    {
        UpdateStereoSources();

        if(spatialSourceVisualizer.transform.parent != null)
        {
            UnityEngine.Vector3 source = spatialSourceVisualizer.transform.parent.transform.position;
            UnityEngine.Vector3 earLeft = headVisualizer.transform.position-headVisualizer.transform.right;
            UnityEngine.Vector3 earRight = headVisualizer.transform.position+headVisualizer.transform.right;
            float distLeft = UnityEngine.Vector3.Distance(source,earLeft);
            float distRight = UnityEngine.Vector3.Distance(source,earRight);
        }

        if (ildOpen)
        {
            UnityEngine.Vector3 source = spatialSourceVisualizer.transform.parent.transform.position;
            UnityEngine.Vector3 earLeft = headVisualizer.transform.position-headVisualizer.transform.right*0.1f;
            UnityEngine.Vector3 earRight = headVisualizer.transform.position+headVisualizer.transform.right*0.1f;
            float distLeft = UnityEngine.Vector3.Distance(source,earLeft);
            float distRight = UnityEngine.Vector3.Distance(source,earRight);
            float diff = (distLeft-distRight)/0.2f;
            //FMOD.RESULT result = demoManager.narratorManager.instance.setParameterByName("ITD_Direction", -1);
            demoManager.narratorManager.SetVoiceParameter("ITD_Direction",diff);
        }

        if (itdOpen){
            UnityEngine.Vector3 source = spatialSourceVisualizer.transform.parent.transform.position;
            UnityEngine.Vector3 earLeft = headVisualizer.transform.position-headVisualizer.transform.right*0.1f;
            UnityEngine.Vector3 earRight = headVisualizer.transform.position+headVisualizer.transform.right*0.1f;
            float distLeft = UnityEngine.Vector3.Distance(source,earLeft);
            float distRight = UnityEngine.Vector3.Distance(source,earRight);
            float diff = (distLeft-distRight)/0.2f;
            //FMOD.RESULT result = demoManager.narratorManager.instance.setParameterByName("ITD_Direction", -1);
            demoManager.narratorManager.SetVoiceParameter("ITD_Direction",diff);
        }

        //Debug.Log(Time.time);

    }

    void ShowITD()
    {
        itdOpen=true;

        
    }

    void SHowILD()
    {
        ildOpen=true;

    }

    void HideITD()
    {
        itdOpen=true;

    }

    void HideILD()
    {
        ildOpen=true;

    }

    void ShowReverb()
    {
        reverb=true;

    }

    void HideReverb()
    {
        reverb=true;

    }


}
