using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrchestraManager : MonoBehaviour
{
    public List<PopupObject> instrumentObjects;

    public void StartOrchestra()
    {
        

    }

    public IEnumerator DelayedOpenInstrument(int instrument, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

    }

    public void SpawnPiano(float delay)
    {
        StartCoroutine(DelayedOpenInstrument(0, i*delay));
    }

    public void SpawnViolins(float delay)
    {
        for(int i=1; i<7; i++){
            StartCoroutine(DelayedOpenInstrument(i, i*0.1f+delay));
        }
    }

    public void SpawnCellos(float delay)
    {
        for(int i=7; i<11; i++){
            StartCoroutine(DelayedOpenInstrument(i, i*0.2f+delay));
        }
    }

    public void SpawnHorns(float delay)
    {
        for(int i=11; i<13; i++){
            StartCoroutine(DelayedOpenInstrument(i, i*0.3f+delay));
        }
    }

    public void SpawnTrumpets(float delay)
    {
        for(int i=13; i<16; i++){
            StartCoroutine(DelayedOpenInstrument(i, i*0.3f+delay));
        }
    }

    public void SpawnDrums(float delay)
    {
        StartCoroutine(DelayedOpenInstrument(16, delay));
    }

    public void SpawnSynth(float delay)
    {
        StartCoroutine(DelayedOpenInstrument(17, delay));
        StartCoroutine(DelayedOpenInstrument(18, delay));
    }



    
}
