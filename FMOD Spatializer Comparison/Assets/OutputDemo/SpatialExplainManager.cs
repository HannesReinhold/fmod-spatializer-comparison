using UnityEngine;
using System.Collections;

public class SpatialExplainManager : MonoBehaviour
{
    public PopupObject headVisualizer;
    public PopupObject monoSourceVisualizer;
    public PopupObject stereoSourceLeftVisualizer;
    public PopupObject stereoSourceRightVisualizer;
    public PopupObject spatialSourceVisualizer;

    public ParticleSystem monoSourceParticles;
    public ParticleSystem stereoSourceLeftParticles;
    public ParticleSystem stereoSourceRightParticles;
    public ParticleSystem spatialSourceParticles;

    public float stereoSourcesDistance = 0.5f;

    public AudioPathManager pathManager;

    public PopupObject occlusionWall;
    public PopupObject roomGeometry;
    public RaytracingVisualizer raytracingVisualizer;

    private float currentStereoDistance = 0;
    private float targetStereoDistance=0;




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

    public void StartSpatialSourcePlaying()
    {
        Debug.Log("Start Spatial Source");
        spatialSourceParticles.Play();
    }

    public void StopSpatialSourcePlaying()
    {
        Debug.Log("Stop Spatial Source");
        spatialSourceParticles.Stop();
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
        LeanTween.rotate(headVisualizer.transform.parent.gameObject, tilt, duration);
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
        LeanTween.moveLocal(spatialSourceVisualizer.transform.parent.gameObject, pos, duration);
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



    public void StartExplainingProcedure()
    {
        // mono
        // spawn head
        //Invoke("ShowHead",1);
        //Spawn source
        //Invoke("ShowMonoSource",2); // 3
        // play delayed 
        //Invoke("StartMonoSourcePlaying",3); // 4
        // after some time stop mono
        //Invoke("StopMonoSourcePlaying",4); // 10
        // hide it
        //Invoke("HideMonoSource", 5); //11


        // stereo

        //Invoke("ShowLeftStereoSource",5);
        //Invoke("ShowRightStereoSource",5);
        //Invoke("SplitStereoSources",5.5f);
        // start playing
        //Invoke("StartStereoLeftSourcePlaying", 6); // 13
        //Invoke("StartStereoRightSourcePlaying", 6);
        // moving and tilting head // 17
        //StartCoroutine(DelayedHeadTilt(new Vector3(0,180,50),1,7));
        //StartCoroutine(DelayedHeadTilt(new Vector3(0, 180, -50), 1, 8));
        //StartCoroutine(DelayedHeadTilt(new Vector3(0, 180, 0), 1, 9));
        // start playing
        //Invoke("StopStereoLeftSourcePlaying", 10); // 25
        //Invoke("StopStereoRightSourcePlaying", 10);
        // merge
        //Invoke("MergeStereoSources",11);
        // hide
        //Invoke("HideLeftStereoSource", 12); // 26
        //Invoke("HideRightStereoSource", 12);

        // itd
        // show source in front
        //Invoke("ShowSpatialSource", 13); // 28
        //StartCoroutine(DelayedSpatializedSourcePos(Vector3.zero,0,13));
        // play
        //Invoke("StartSpatialSourcePlaying", 14); // 29
        //move slowly to left
        //StartCoroutine(DelayedSpatializedSourcePos(new Vector3(-1,0.2f,0), 1, 14));
        // move slowly to right
        //StartCoroutine(DelayedSpatializedSourcePos(new Vector3(1, 0.2f, 0), 1, 15));
        // move to center
        //StartCoroutine(DelayedSpatializedSourcePos(new Vector3(0, 0, 0), 1, 15));

        //ild
        // show sound shadow
        //Invoke("ShowSoundShadow", 16);
        // move left
        //StartCoroutine(DelayedSpatializedSourcePos(new Vector3(-1, 0.2f, 0), 1, 17));
        // move right
        //StartCoroutine(DelayedSpatializedSourcePos(new Vector3(0, 0, 0), 1, 18));
        // hide shadow
        //Invoke("ShowSoundShadow", 19);
        //hrtf

        // show 3d field around head

        // move source around the head
        Invoke("ShowHead", 1);
        Invoke("ShowSpatialSource", 2);
        StartCoroutine(DelayedAudioPath(5, 2));

        //occlusion
        Invoke("HideHead", 3);
        // spawn wall in front of player
        StartCoroutine(DelayedOcclusionWallOpen(true, 4));
        // move source around wall
        StartCoroutine(DelayedAudioPath(6, 5));
        // close Wall
        StartCoroutine(DelayedOcclusionWallOpen(false, 6));

        //reverb

        // show room geometry
        StartCoroutine(DelayedRoomGeometryOpen(true, 7));
        // begin with reflection visualization
        StartCoroutine(DelayedRaysVisible(true, 8));
        // hide rays
        StartCoroutine(DelayedRaysVisible(false, 9));
        //move source around room
        StartCoroutine(DelayedAudioPath(6, 10));
        //hide
        StartCoroutine(DelayedRoomGeometryOpen(false, 11));
        Invoke("StopSpatialSourcePlaying", 12); // 29
        Invoke("HideSpatialSource", 12); // 29

    }

    void Update()
    {
        UpdateStereoSources();
    }


}
