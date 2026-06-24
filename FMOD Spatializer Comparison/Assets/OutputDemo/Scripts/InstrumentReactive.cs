using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using FMOD;
using System;
using UnityEngine.VFX;
using System.Collections.Generic;
using System.Collections;
using PathCreation;
using PathCreation.Examples;

public class InstrumentReactive : MonoBehaviour
{
    EventInstance instance;

    public StudioEventEmitter  emitter;

    DSP dsp = new DSP();
    ChannelGroup channelGroup;
    private bool meteringInitialized;
    public float Loudness { get; private set; }
    public Transform visual;
    public List<VisualEffect> vfxObjects;

    public List<ParticleSystem> particleSystems;

    public bool isPlaying=false;

    public float scalingStrength=0.01f;

    void Awake()
    {
        for(int i=0; i<vfxObjects.Count; i++)
        {
            vfxObjects[i].Stop();
        }
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
        UnityEngine.Debug.Log("INIT "+meteringInitialized);
    }
}

    public void Play()
    {
        UnityEngine.Debug.Log("Start Playing Reactive Source");
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
            UnityEngine.Debug.Log("INIT "+meteringInitialized);
        }
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

        visual.localScale = UnityEngine.Vector3.one*(1+Loudness*scalingStrength);

        for(int i=0; i<vfxObjects.Count; i++)
        {
            vfxObjects[i].SetFloat("TrailSize",0.1f+Loudness);
            vfxObjects[i].SetFloat("Turbulence",0.1f+Loudness*4);
        }

        for(int i=0; i<particleSystems.Count; i++)
        {
            // do burst
        }


    }

    
}
