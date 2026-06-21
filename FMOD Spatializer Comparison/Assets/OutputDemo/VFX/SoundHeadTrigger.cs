using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SoundHeadTrigger : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Collider triggerCollider1;
    public Collider triggerCollider2;
    public MeshRenderer meshRendererLeft;
    public MeshRenderer meshRendererRight;

    // Verbleibende Lebenszeit, sobald ein Partikel den Trigger erreicht
    public float lifetimeInsideTrigger = 0.2f;

    private ParticleSystem.Particle[] particles;

    private float smoothedHighlightValueLeft = 0;
    private float smoothedHighlightValueRight = 0;

    void Update()
    {

        int count = particleSystem.particleCount;

        if (particles == null || particles.Length < count)
            particles = new ParticleSystem.Particle[count];

        count = particleSystem.GetParticles(particles);

        bool isLocal = particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local;

        for (int i = 0; i < count; i++)
        {
            Vector3 particlePos = isLocal
                ? particleSystem.transform.TransformPoint(particles[i].position)
                : particles[i].position;

            if (triggerCollider1.bounds.Contains(particlePos))
            {
                particles[i].remainingLifetime = Mathf.Min(
                    particles[i].remainingLifetime,
                    lifetimeInsideTrigger
                );
                smoothedHighlightValueLeft = Mathf.Max(smoothedHighlightValueLeft, particles[i].remainingLifetime);
                smoothedHighlightValueLeft = Mathf.Clamp(smoothedHighlightValueLeft,0,1);
                //Debug.Log("Collide Left");
            }

            if (triggerCollider2.bounds.Contains(particlePos))
            {
                smoothedHighlightValueRight = Mathf.Max(smoothedHighlightValueRight, particles[i].remainingLifetime);
                smoothedHighlightValueRight = Mathf.Clamp(smoothedHighlightValueRight, 0, 1);
                //Debug.Log("Collide Right");
            }

        }
        

        meshRendererLeft.material.SetFloat("_Highlighting", smoothedHighlightValueLeft);
        meshRendererRight.material.SetFloat("_Highlighting", smoothedHighlightValueRight);

        smoothedHighlightValueLeft = Mathf.Lerp(smoothedHighlightValueLeft, 0, Time.deltaTime * 7);
        smoothedHighlightValueRight = Mathf.Lerp(smoothedHighlightValueRight, 0, Time.deltaTime * 7);

        particleSystem.SetParticles(particles, count);
    }
}