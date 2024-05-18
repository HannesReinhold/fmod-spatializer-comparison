using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSwitchTest : MonoBehaviour
{
    [Range(0, 2)] public int selectedSpatializer;
    [Range(0, 1)] public float globalVolume = 1;
    public bool start = false;
    public bool stop = false;

    public List<GameObject> audioGroups = new List<GameObject>();

    public Slider slider;

    public Slider progressBar;
    public Slider timeSetSlider;
    public FMODUnity.StudioEventEmitter progressEmitter;
    private FMOD.Studio.EventInstance progressEventInstance;



    // Start is called before the first frame update
    void Start()
    {
        SetBus(selectedSpatializer);

        foreach (GameObject g in audioGroups)
        {
            g.SetActive(true);
        }

        progressEventInstance = progressEmitter.EventInstance;

    }

    // Update is called once per frame
    void Update()
    {
        selectedSpatializer = Mathf.RoundToInt(slider.value);
        SetBus(selectedSpatializer);


        int timeInMs1 = 0;
        progressEmitter.EventDescription.getLength(out timeInMs1);

        if (start)
        {

            
            start = false;
            foreach(GameObject g in audioGroups)
            {
                
                
                FMODUnity.StudioEventEmitter[] emitters = g.GetComponentsInChildren<FMODUnity.StudioEventEmitter>();
                for(int i=0; i<emitters.Length; i++)
                {
                    FMOD.Studio.EventInstance eventInstance = emitters[i].EventInstance;
                    eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    eventInstance.setTimelinePosition(Mathf.RoundToInt(timeSetSlider.value*timeInMs1));
                    eventInstance.start();
                }
                
            }
        }

        if (stop)
        {
            stop = false;
            foreach (GameObject g in audioGroups)
            {
                FMODUnity.StudioEventEmitter[] emitters = g.GetComponentsInChildren<FMODUnity.StudioEventEmitter>();
                for (int j = 0; j < emitters.Length; j++)
                {
                    FMOD.Studio.EventInstance eventInstance = emitters[j].EventInstance;
                    eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    eventInstance.setTimelinePosition(Mathf.RoundToInt(0));
                }
            }
        }

        int timeInMs2 = 0;
        progressEmitter.EventDescription.getLength(out timeInMs2);
        progressEventInstance = progressEmitter.EventInstance;
        int playbackTime;
        progressEventInstance.getTimelinePosition(out playbackTime);
        if(timeInMs2!=0)
            progressBar.value = Mathf.Lerp(0,1000, (float)playbackTime / (float)timeInMs2);


    }

    public void OnStart()
    {
        start = true;
    }

    public void OnStop()
    {
        stop = true;
    }

    private void EnableAll()
    {
        start = true;
    }

    private void SetBus(int i)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("OculusVolume", i == 0 ? 1 : 0.01f);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("ResonanceVolume", i == 1 ? 1 : 0.01f);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SteamVolume", i == 2 ? 1 : 0.01f);

        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("GlobalVolume", globalVolume);
    }
}
