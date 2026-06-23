using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using FMOD;
using System;
using UnityEngine.VFX;
using System.Collections.Generic;
using System.Collections;

public class ReactiveAudioSource : MonoBehaviour
{
    EventInstance instance;

    public StudioEventEmitter  emitter;

    DSP dsp = new DSP();
    ChannelGroup channelGroup;
    private bool meteringInitialized;
    public float Loudness { get; private set; }
    public Transform visual;
    public List<VisualEffect> vfxObjects;

    public bool isPlaying=false;

    void Start()
    {
        StartCoroutine(InitializeAfterPlay());
    }

    private void Update()
    {
        if (!meteringInitialized)
        {
            InitializeMetering();
        }

        if (meteringInitialized)
        {
            UpdateLoudness();
        }
    }

    IEnumerator InitializeAfterPlay()
{
    yield return null;
    yield return null;

    instance = emitter.EventInstance;

    instance.getChannelGroup(out channelGroup);

    UnityEngine.Debug.Log(channelGroup.hasHandle());

    if(channelGroup.hasHandle())
    {
        channelGroup.getDSP(
            (int)CHANNELCONTROL_DSP_INDEX.HEAD,
            out dsp);

        dsp.setMeteringEnabled(true, true);

        meteringInitialized = true;
    }
}

    public void Play()
    {
        isPlaying=true;
        emitter.Play();
        for(int i=0; i<vfxObjects.Count; i++)
        {
            vfxObjects[i].Play();
        }
        
    }

    public void Stop()
    {
        isPlaying=false;
        emitter.Stop();
        for(int i=0; i<vfxObjects.Count; i++)
        {
            vfxObjects[i].Stop();
        }
    }

    private void InitializeMetering()
    {
        instance.getChannelGroup(out channelGroup);

        if (!channelGroup.hasHandle())
            return;

        channelGroup.getDSP(
            (int)CHANNELCONTROL_DSP_INDEX.HEAD,
            out dsp);
        dsp.setMeteringEnabled(true, true);

        meteringInitialized = true;
    }

    private void UpdateLoudness()
    {
        dsp.getMeteringInfo(
            out DSP_METERING_INFO inputMeter,
            out DSP_METERING_INFO outputMeter
        );

        float rms = 0f;

        for (int i = 0; i < inputMeter.numchannels; i++)
        {
            rms += inputMeter.rmslevel[i];
        }

        Loudness = rms / inputMeter.numchannels;

        visual.localScale = UnityEngine.Vector3.one*Loudness;

        for(int i=0; i<vfxObjects.Count; i++)
        {
            vfxObjects[i].SetFloat("TrailSize",0.1f+Loudness);
            vfxObjects[i].SetFloat("Turbulence",0.1f+Loudness*4);
        }

    }

    
}
