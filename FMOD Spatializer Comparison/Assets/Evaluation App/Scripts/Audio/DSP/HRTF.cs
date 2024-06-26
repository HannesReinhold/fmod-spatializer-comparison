using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRTF
{
    public EQ eqLeft = new EQ(4);
    public EQ eqRight = new EQ(4);

    public DynamicDelay earDelay = new DynamicDelay(1000,44100);



    public float earDistance = 0.215f;

    float[] irLeft = new float[128];
    float[] irRight = new float[128];

    float[] bufferLeft = new float[128];
    float[] bufferRight = new float[128];

    int readP=0;

    public int del = 10;

    public void UpdateHRTF(Vector3 direction, float distance)
    {


        float dotLeft = Mathf.Pow((Vector3.Dot(-Vector3.right, direction) + 1) * 0.5f, 1);
        float dotRight = Mathf.Pow((Vector3.Dot(Vector3.right, direction) + 1) * 0.5f, 1);
        float dotForward = Mathf.Pow((Vector3.Dot(Vector3.forward, direction) + 1) * 0.5f, 1);
        float dotUpward = Mathf.Pow((Vector3.Dot(Vector3.up, direction) + 1) * 0.5f, 1);


        float earDelayInSamples = earDistance / 343f * 44100f * dotLeft;

        float leftDelayInSamples = earDelayInSamples * dotLeft;
        float rightDelayInSamples = leftDelayInSamples * dotRight;

        earDelay.SetDelays(leftDelayInSamples, rightDelayInSamples);

        Debug.Log(dotLeft);

       



        eqLeft.SetFilter(0, BiquadType.Peak,17000,0.6f,10);
        eqLeft.SetFilter(1, BiquadType.Peak, 10000, 25, -30);
        eqLeft.SetFilter(2, BiquadType.Peak, 6500, 30, -20);
        eqLeft.SetFilter(3, BiquadType.Peak, 4000, 0.5f, 3);

        eqRight.SetFilter(0, BiquadType.Peak, 17000, 0.6f, 10);
        eqRight.SetFilter(1, BiquadType.Peak, 10000, 25, -30);
        eqRight.SetFilter(2, BiquadType.Peak, 6500, 30, -20);
        eqRight.SetFilter(3, BiquadType.Peak, 4000, 0.5f, 3);


        for(int i=0; i<128; i++)
        {
            irLeft[i] = (Mathf.PerlinNoise1D(i*0.05f+del*0.13f)*2-1)*1f/Mathf.Pow(i+1,0.2f);
            irRight[i] = (Mathf.PerlinNoise1D(i * 0.05f + del * 0.13f+43.1241f) * 2 - 1) * 1f / Mathf.Pow(i + 1, 0.2f);
        }

    }





    public void Process(float[] data, int channels)
    {
        float outputL = 0;
        float outputR = 0;

        earDelay.ProcessBlock(data, channels);

        for(int i = 0; i < data.Length; i+=channels)
        {
            float inputL = data[i];
            float inputR = data[i+1];

            //outputL = eqLeft.Process(inputL);
            //outputR = eqRight.Process(inputR);

            
            float sumLeft = 0;
            float sumRight = 0;

            bufferLeft[readP] = inputL;
            bufferRight[readP] = inputR;

            readP++;

            for (int j=0; j<128; j+=1)
            {
                sumLeft += bufferLeft[(readP + j) % 128] * irLeft[j];
                sumRight += bufferRight[(readP + j) % 128] * irRight[j];
            }
            
            if (readP >= 128) readP -= 128;


            data[i] = sumLeft;
            data[i + 1] = sumRight;
            
            //data[i] = outputL;
            //data[i + 1] = outputR;
        }
        
    }
}


public class EQ
{
    private int numFilters;
    private BiquadSingleChannel[] filters;

    public EQ(int numFilers)
    {
        this.numFilters = numFilers;
        filters = new BiquadSingleChannel[numFilers];
        for(int i=0; i<numFilers; i++)
        {
            filters[i] = new BiquadSingleChannel();
        }
    }

    public void SetFilter(int index, BiquadType type, float fc, float q, float pG)
    {
        fc = fc / 44100f;
        filters[index].SetCoefficients(BiquadCalculator.CalculateCoefficients(type, fc, q, pG));
    }

    public float Process(float input)
    {
        for(int i=0; i< numFilters; i++)
        {
            input = filters[i].Process(input);
        }

        return input;
    }

}
