using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLight : MonoBehaviour
{
    public Light light;

    private float currentBrightness;
    private float targetBrightness = 0;

    private void Update()
    {
        currentBrightness = Mathf.Lerp(currentBrightness, targetBrightness, Time.deltaTime*10);
        light.intensity = currentBrightness;
    }
    
    public void SetBrightness(float b)
    {
        targetBrightness = b;
    }
}
