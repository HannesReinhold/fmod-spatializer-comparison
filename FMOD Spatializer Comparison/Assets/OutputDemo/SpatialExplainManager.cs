using UnityEngine;
using System.Collections;

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

    private float currentStereoDistance = 0;
    private float targetStereoDistance=0;

    public int timeOffset=0;




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
        Vector3 pos = headVisualizer.transform.parent.position;
        Vector3 posLeft = pos - headVisualizer.transform.parent.right * currentStereoDistance - headVisualizer.transform.parent.up * 0.193f;
        Vector3 posRight = pos + headVisualizer.transform.parent.right * currentStereoDistance - headVisualizer.transform.parent.up * 0.193f;
        //posLeft.y = h;
        //posRight.y = h;
        stereoSourceLeftVisualizer.transform.parent.position = posLeft;
        stereoSourceRightVisualizer.transform.parent.position = posRight;

        currentStereoDistance = Mathf.Lerp(currentStereoDistance, targetStereoDistance, Time.deltaTime);
    }

    private void SetHeadTilt(Vector3 tilt, float duration)
    {
        Debug.Log("Set Head Tilt");
        LeanTween.rotate(headVisualizer.transform.parent.gameObject, tilt, duration).setEaseInOutSine();
    }


    IEnumerator DelayedHeadTilt(Vector3 tilt, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        SetHeadTilt(tilt, duration);

    }

    private void SetSpatializedSourcePos(Vector3 pos, float duration)
    {
        Vector3 newPos = headVisualizer.transform.parent.position + headVisualizer.transform.parent.up;
        Debug.Log("Move SOurce to "+newPos);
        LeanTween.moveLocal(spatialSourceVisualizer.transform.parent.gameObject, pos, duration).setEaseInOutSine();
    }


    IEnumerator DelayedSpatializedSourcePos(Vector3 pos, float duration, float delayTime)
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
    }



    public void StartExplainingProcedure()
    {
        // mono
        // spawn head
        Invoke("ShowHead",1);
        //Spawn source
        Invoke("ShowMonoSource",3); // 3
        // play delayed 
        Invoke("StartMonoSourcePlaying",4); // 4
        // after some time stop mono
        Invoke("StopMonoSourcePlaying",10); // 10
        // hide it
        Invoke("HideMonoSource", 11); //11


        // stereo

        Invoke("ShowLeftStereoSource",11);
        Invoke("ShowRightStereoSource",11);
        Invoke("SplitStereoSources",12);
        // start playing
        Invoke("StartStereoLeftSourcePlaying", 13); // 13
        Invoke("StartStereoRightSourcePlaying", 13);
        // moving and tilting head // 17
        StartCoroutine(DelayedHeadTilt(new Vector3(0,180,50),2,17));
        StartCoroutine(DelayedHeadTilt(new Vector3(0, 180, -50), 4, 20));
        StartCoroutine(DelayedHeadTilt(new Vector3(0, 180, 0), 2, 25));
        // start playing
        Invoke("StopStereoLeftSourcePlaying", 28); // 25
        Invoke("StopStereoRightSourcePlaying", 28);
        // merge
        Invoke("MergeStereoSources",29);
        // hide
        Invoke("HideLeftStereoSource", 30); // 26
        Invoke("HideRightStereoSource", 30);

        // itd
        // show source in front
        Invoke("ShowSpatialSource", 32); // 28
        StartCoroutine(DelayedSpatializedSourcePos(new Vector3(-0.5f,0.2f,0),0,32));
        // play
        Invoke("StartSpatialSourceDirectionalPlaying", 33); // 29
        //move slowly to left
        StartCoroutine(DelayedSpatializedSourcePos(new Vector3(-1,0.2f,0), 2, 34));
        // move slowly to right
        StartCoroutine(DelayedSpatializedSourcePos(new Vector3(1, 0.2f, 0), 4, 35));
        // move to center
        StartCoroutine(DelayedSpatializedSourcePos(new Vector3(0, 0, 0), 2, 44));

        //ild
        // show sound shadow
        Invoke("ShowSoundShadow", 47);
        // move left
        StartCoroutine(DelayedSpatializedSourcePos(new Vector3(-1, 0.2f, 0), 2, 48));
        // move right
        StartCoroutine(DelayedSpatializedSourcePos(new Vector3(0, 0, 0), 4, 52));
        // hide shadow
        Invoke("HideSoundShadow", 57);

        //hrtf

        // show 3d field around head

        // move source around the head
        StartCoroutine(DelayedAudioPath(5, 60));

        //occlusion
        Invoke("HideHead", 70);
        Invoke("StopSpatialSourceDirectionalPlaying", 70);
        Invoke("StartSpatialSource360Playing", 71);
        // spawn wall in front of player
        StartCoroutine(DelayedOcclusionWallOpen(true, 72));
        // move source around wall
        StartCoroutine(DelayedAudioPath(6, 73));
        // close Wall
        StartCoroutine(DelayedOcclusionWallOpen(false, 80));

        //reverb

        // show room geometry
        StartCoroutine(DelayedRoomGeometryOpen(true, 82));
        // begin with reflection visualization
        StartCoroutine(DelayedRaysVisible(true, 84));
        // hide rays
        StartCoroutine(DelayedRaysVisible(false, 90));
        //move source around room
        StartCoroutine(DelayedAudioPath(6, 92));
        //hide
        Invoke("StopSpatialSource360Playing", 98); // 29
        Invoke("HideSpatialSource", 99); // 29
        StartCoroutine(DelayedRoomGeometryOpen(false, 100));

        Invoke("StartNextScenario",101);
        

    }

    void Update()
    {
        UpdateStereoSources();
    }


}
