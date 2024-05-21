using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmJointAudio : MonoBehaviour
{
    [Header("Attributes")]
    public bool isLooping = true;
    public float servoFrequencyMultiplier = 1;

    [Header("Arm Joint Ref")]
    public Transform armJoint;

    [Header("Audio References")]
    public FMODUnity.StudioEventEmitter emitter;
    public FMODUnity.EventReference eventRef;


    private FMOD.Studio.EventInstance instance;
    
    private float lastRotation = 0;
    private float phase = 0;
    private bool isPlaying = false;

    private float smoothedAngularVelocity = 0;
    Quaternion lastAngle;

    private SPatializerSwitchManager spatialRefs;

    private float cooldown = 0;


    private void Awake()
    {
        emitter.EventReference = eventRef;
        instance = emitter.EventInstance;
        lastRotation = armJoint.localEulerAngles.y;

        lastAngle = transform.localRotation;

        spatialRefs = FindAnyObjectByType<SPatializerSwitchManager>();
    }

    private void OnEnable()
    {
        instance.setPaused(false);
    }

    private void OnDisable()
    {
        instance.setPaused(true);
    }

    private void FixedUpdate()
    {
        float currentRotation = armJoint.localEulerAngles.y;

        Quaternion currentAngle = armJoint.localRotation;
        float angle = Quaternion.Angle(currentAngle, lastAngle);
        lastAngle = currentAngle;

        float angularVelocity = Mathf.Abs(angle) * Time.fixedDeltaTime * 100;
        smoothedAngularVelocity = Mathf.Lerp(smoothedAngularVelocity, angularVelocity, 0.1f);
        lastRotation = currentRotation;

        

        if (isLooping)
        {
            PlayLoop(smoothedAngularVelocity);
        }
        else
        {
            PlayNonLoop(smoothedAngularVelocity);
        }

        cooldown += Time.fixedDeltaTime;
    }

    private void PlayLoop(float velocity)
    {
        instance.setParameterByName("RobotArmVelocity", Mathf.Clamp(velocity * servoFrequencyMultiplier, 0, 1));

        if (velocity > 0.001f && !isPlaying)
        {
            isPlaying = true;
            emitter.Play();
            FMODUnity.RuntimeManager.PlayOneShot(spatialRefs.GetEvent(6)[spatialRefs.currentSpatializer], transform.position);
            cooldown = 0;
        }

        if (velocity <= 0.001f && isPlaying)
        {
            isPlaying = false;
            if(cooldown>1)
                FMODUnity.RuntimeManager.PlayOneShot(spatialRefs.GetEvent(7)[spatialRefs.currentSpatializer], transform.position);
        }

    }

    private void PlayNonLoop(float velocity)
    {
        phase += velocity;
        if (phase > 2)
        {
            phase = 0;
            instance.setParameterByName("RobotArmVelocity", Mathf.Clamp(velocity * servoFrequencyMultiplier, 0, 1));
            emitter.Play();
        }
    }

}

