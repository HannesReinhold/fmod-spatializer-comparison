using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsflicker : MonoBehaviour
{
    public List<Light> lights;

    public float flickerIncreaseTime = 1;
    public float flickerDecreaseTime = 0.3f;

    public float flickerAmplitude = 1.5f;
    public float flickerFrequency = 10;
    public float flickerSteepness = 5;


    private float flickerIntensity = 0;
    private bool enableFlickering = false;
    private bool increaseFlickering = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableFlickering) return;

        if (increaseFlickering) flickerIntensity += flickerIncreaseTime * Time.deltaTime;
        else flickerIntensity -= flickerDecreaseTime * Time.deltaTime;

        flickerIntensity = Mathf.Clamp(flickerIntensity, 0, 1);
        
        for(int i=0; i<lights.Count; i++)
        {
            lights[i].intensity = sigmoid(Mathf.PerlinNoise1D(Time.time*flickerFrequency+i*3.521f)*2-1, flickerSteepness) * flickerAmplitude;
        }
    }

    public void StartFlicker()
    {
        flickerIntensity = 0;
        enableFlickering = true;
    }

    public void StopFlicker()
    {
        //flickerIntensity = 0;
        increaseFlickering = false;
        Invoke("EndFlicker", flickerDecreaseTime);
    }

    private void EndFlicker()
    {
        enableFlickering = false;
    }

    private float sigmoid(float x, float steepness)
    {
        return 1 / (Mathf.Exp(-x*steepness)+1);
    }
}
