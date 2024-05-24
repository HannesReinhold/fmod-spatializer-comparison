using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarlyReflections
{
    private VariableDelay[] delays;

    public float[] delayTimes;
    public Vector3[] delayDirections;
    public float[] reflectionStrength;

    private float[] panning;

    public float dry = 0.75f;
    public float wet = 0.75f;

    
    public EarlyReflections(int numRefl = 8)
    {
        delays = new VariableDelay[numRefl];
        delayTimes = new float[numRefl];
        delayDirections = new Vector3[numRefl];
        reflectionStrength = new float[numRefl];
        panning = new float[numRefl];
        for(int i=0; i<numRefl; i++)
        {
            delays[i] = new VariableDelay(4000);
        }
    }

    public void Process(float[] buffer)
    {
        for (int i = 0; i < buffer.Length; i += 2)
        {
            float sample = buffer[i] + buffer[i + 1];

            float outputLeft = 0;
            float outputRight = 0;

            for (int j = 0; j < delays.Length; j++)
            {
                float refl = delays[j].Process(sample) * reflectionStrength[j];
                outputLeft += (1 - panning[j]) * refl;
                outputRight += panning[j] * refl;
            }

            buffer[i] = outputLeft * wet + buffer[i]*dry;
            buffer[i + 1] = outputRight * wet + buffer[i+1]*dry;
        }
    }

    public void SetDelays(float[] times, float[] pan, float[] str)
    {
        delayTimes = times;
        panning = pan;
        reflectionStrength = str;
    }

}
