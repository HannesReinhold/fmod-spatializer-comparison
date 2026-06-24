using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class AudioParticle : MonoBehaviour
{
    public StudioEventEmitter emitter;
    public VisualEffect vfx;

    

    public bool ready=true;

    void Awake()
    {
        vfx.Stop();
    }

    private void Kill()
    {
        emitter.Stop();
        ready=true;
    }

    public void Blink(Vector3 pos, EventReference audioEvent, Color color)
    {
        transform.localPosition = pos;
        emitter.EventReference = audioEvent;
        RuntimeManager.PlayOneShot(audioEvent, pos);
        vfx.SetVector4("Color",color);
        vfx.SetFloat("EmissionStrength",4);
        vfx.Play();
        Invoke("Kill",0.5f);
        ready=false;
    }
}
