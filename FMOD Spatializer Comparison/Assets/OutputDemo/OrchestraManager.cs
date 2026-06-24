using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrchestraManager : MonoBehaviour
{
    public OutputDemoManager demoManager;
    public List<PopupObject> instrumentObjects;
    public List<InstrumentReactive> instruments;

    public void StartOrchestra()
    {
        SpawnPiano(1);
        SpawnViolins(3);
        SpawnCellos(5);
        SpawnHorns(7);
        SpawnTrumpets(9);
        SpawnDrums(11);
        SpawnSynth(13);

        PlayPiano(4);
        PlayDrums(14);

        for(int i=0; i<instrumentObjects.Count; i++)
        {
            //StartCoroutine(DelayedCloseInstrument(i,20));
            //StartCoroutine(DelayedStopInstrument(i,20));
        }

    }

    public IEnumerator DelayedOpenInstrument(int instrument, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        instrumentObjects[instrument].Open();

    }

    public IEnumerator DelayedCloseInstrument(int instrument, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        instrumentObjects[instrument].Close();

    }

    public IEnumerator DelayedPlayInstrument(int instrument, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        instruments[instrument].Play();

    }

    public IEnumerator DelayedStopInstrument(int instrument, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        instruments[instrument].Stop();

    }

    public void SpawnPiano(float delay)
    {
        Debug.Log("Spawn Piano");
        StartCoroutine(DelayedOpenInstrument(0, delay));
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

    public void PlayPiano(float delay)
    {
        Debug.Log("Play Piano");
        StartCoroutine(DelayedPlayInstrument(0, delay));
    }

    public void PlayViolins(float delay)
    {
        for(int i=1; i<4; i++){
            StartCoroutine(DelayedPlayInstrument(i, i*0.1f+delay));
        }
    }

    public void PlayCellos(float delay)
    {
        for(int i=4; i<6; i++){
            StartCoroutine(DelayedPlayInstrument(i, i*0.2f+delay));
        }
    }

    public void PlayHorns(float delay)
    {
        for(int i=6; i<8; i++){
            StartCoroutine(DelayedPlayInstrument(i, i*0.3f+delay));
        }
    }

    public void PlayTrumpets(float delay)
    {
        for(int i=8; i<11; i++){
            StartCoroutine(DelayedPlayInstrument(i, i*0.3f+delay));
        }
    }

    public void PlayDrums(float delay)
    {
        StartCoroutine(DelayedPlayInstrument(11, delay));
    }

    public void PlaySynth(float delay)
    {
        StartCoroutine(DelayedPlayInstrument(12, delay));
    }



    
}
