using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergingObject : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;
    public GameObject objectToEmerge;

    public float appearTime = 3;
    public float disappearTime = 3;
    public float initialHeight = 2;
    public float appearDelay = 1;

    private float startHeight;

    public bool moveX = false;
    public bool moveZ = false;

    private void Awake()
    {
        objectToEmerge = this.gameObject;
    }

    private void OnEnable()
    {
        if (!moveX)
        {
            startHeight = transform.localPosition.y;
            objectToEmerge.transform.localPosition -= Vector3.up * initialHeight;
        }
        else if(!moveZ)
        {
            startHeight = transform.localPosition.z;
            objectToEmerge.transform.localPosition -= Vector3.forward * initialHeight;
        }
        else
        {
            startHeight = transform.localPosition.x;
            objectToEmerge.transform.localPosition -= Vector3.left * initialHeight;
        }
        Invoke("Appear", appearDelay);

        
    }

    public void Appear()
    {
        objectToEmerge.SetActive(true);
        if(!moveX) LeanTween.moveLocalY(objectToEmerge, startHeight, appearTime).setEaseInOutQuad();
        else if(!moveZ) LeanTween.moveLocalZ(objectToEmerge, startHeight, appearTime).setEaseInOutQuad();
        else  LeanTween.moveLocalX(objectToEmerge, startHeight, appearTime).setEaseInOutQuad();
        if (emitter!=null) emitter.Play();
        Invoke("StopAppearing", appearTime);
    }

    private void StopAppearing()
    {
        if (emitter != null) emitter.Stop();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/FMOD Spatializer/EmergeEnd", transform.position);
    }

    public void Disappear()
    {
        if(!moveX)LeanTween.moveLocalY(objectToEmerge, -initialHeight, disappearTime).setEaseInOutQuad().setOnComplete(Disable);
        else if(!moveZ) LeanTween.moveLocalZ(objectToEmerge, -initialHeight, disappearTime).setEaseInOutQuad().setOnComplete(Disable);
        else LeanTween.moveLocalX(objectToEmerge, -initialHeight, disappearTime).setEaseInOutQuad().setOnComplete(Disable);
        if (emitter != null) emitter.Play();
        Invoke("Disable", disappearTime);
    }

    public void Disable()
    {
        emitter.Stop();
        objectToEmerge.SetActive(false);
    }
}
