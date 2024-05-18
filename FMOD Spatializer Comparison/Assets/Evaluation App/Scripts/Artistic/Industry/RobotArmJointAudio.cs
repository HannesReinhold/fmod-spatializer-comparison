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


    private void Start()
    {
        instance = emitter.EventInstance;
        emitter.EventReference = eventRef;
        lastRotation = armJoint.localEulerAngles.y;

        lastAngle = transform.localRotation;
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
    }

    private void PlayLoop(float velocity)
    {
        instance.setParameterByName("RobotArmVelocity", Mathf.Clamp(velocity * servoFrequencyMultiplier, 0, 1));

        if (velocity > 0.001f && !isPlaying)
        {
            isPlaying = true;
            emitter.Play();
        }

        if (velocity <= 0.001f && isPlaying)
        {
            isPlaying = false;
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

