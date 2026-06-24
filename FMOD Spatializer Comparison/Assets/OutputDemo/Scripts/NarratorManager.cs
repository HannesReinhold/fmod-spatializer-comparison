using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class NarratorManager : MonoBehaviour
{
    EventInstance instance;

    public StudioEventEmitter  emitter;

    DSP dsp = new DSP();
    ChannelGroup channelGroup;
    private bool meteringInitialized;
    public float Loudness { get; private set; }

    public Vector3 targetPosition;

    public float angularSnapThreshold = 40;
    public float moveTime=1;
    public float targetHeight = 1.7f;
    private bool isAlreadyMoving=false;
    public float distanceToCamera;
    public VisualEffect vfx;

    public List<EventReference> voicelines;

    public bool faceCamera=false;

    void Start()
    {
        vfx.Stop();
    }

    void Update()
    {
        if (!meteringInitialized)
        {
            InitializeMetering();
        }

        if (meteringInitialized)
        {
            UpdateLoudness();
        }

        if (faceCamera)
        {
            CheckDirection();
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(pos.y, Camera.main.transform.position.y, Time.deltaTime);
            transform.position = pos;
        }
    }

    public void CheckDirection()
    {
        Vector3 camPos = Camera.main.transform.position;

        // Flatten directions onto XZ plane
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 narratorDir = transform.position - camPos;
        narratorDir.y = 0f;
        narratorDir.Normalize();

        float angle = Vector3.Angle(camForward, narratorDir);

        UnityEngine.Debug.Log("Angle: "+angle);

        if (angle > angularSnapThreshold && !isAlreadyMoving)
        {
            SetPositionTarget();
        }    
    }

    private void SetPositionTarget()
    {
        isAlreadyMoving=true;
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0.0f;
        camForward.Normalize();

        Vector3 targetPos =
            Camera.main.transform.position +
            camForward * distanceToCamera+Vector3.up*targetHeight;

        // Keep narrator's current height
        targetPos.y = transform.position.y;

        if (isAlreadyMoving)
        {
            LeanTween.cancel(gameObject);
        }

        LeanTween.move(gameObject, targetPos, moveTime)
            .setEase(LeanTweenType.easeInOutCubic).setOnComplete(() => isAlreadyMoving = false);

        // Optional: face the camera horizontally
        Vector3 lookPos = Camera.main.transform.position;
        //lookPos.y = targetHeight;

        transform.LookAt(lookPos);
    }

    public void Show()
    {
        UnityEngine.Debug.Log("Show Narrator");
        vfx.Play();
    }

    public void Hide()
    {
        UnityEngine.Debug.Log("Hide Narrator");
        vfx.Stop();
    }

    public void PlayVoiceline(int index)
    {
        UnityEngine.Debug.Log("Play Narrator Voiceline "+index);
        emitter.EventReference = voicelines[index];
        emitter.Play();
    }

    public void StopVoiceline()
    {
        UnityEngine.Debug.Log("Stop Narrator Voiceline ");
        emitter.Stop();
    }

    public void SetPosition(Vector3 pos, float duration)
    {
        UnityEngine.Debug.Log("Set Narrator Position "+pos);
        LeanTween.move(gameObject, pos, duration);
        faceCamera= false;
    }

    public IEnumerator DelayedShow(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Show();
    }

    public IEnumerator DelayedHide(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Show();
    }

    public IEnumerator DelayedPlayVoiceline(int voiceline, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlayVoiceline(voiceline);
    }

    public IEnumerator DelayedStopVoiceline(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StopVoiceline();
    }

    public IEnumerator DelayedSetPosition(Vector3 pos, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetPosition(pos, duration);
    }

    public IEnumerator DelayedSetFacingCamera(Vector3 offset, float duration, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        faceCamera= true;
        SetPositionTarget();
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

        vfx.SetFloat("Radius",0.1f+Loudness*0.05f);

        //UnityEngine.Debug.Log(Loudness);

    }
    
}
