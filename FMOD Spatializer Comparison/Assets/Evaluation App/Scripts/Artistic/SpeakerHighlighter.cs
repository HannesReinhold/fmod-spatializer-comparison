using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerHighlighter : MonoBehaviour
{
    public Renderer[] renderers;
    public ParticleSystem particles;


    private float targetIntensity;
    private float lastIntensity;
    private float currentIntensity;

    private float currentTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        //renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHighlighting()
    {
        foreach(Renderer r in renderers)
        {
            r.materials[1].SetFloat("_Intensity", currentIntensity);
        }
        Debug.Log(currentIntensity);
    }

    public void ApplyHighlight()
    {
        targetIntensity = 1;
        currentTime = 0;
        particles.Play();
        StartCoroutine(UpdateHighlight());
        
        Debug.Log("Apply Highlight"+gameObject);
    }

    public void RemoveHighlight()
    {
        targetIntensity = 0;
        currentTime = 0;
        StartCoroutine(UpdateHighlight());
        particles.Stop();
    }


    IEnumerator UpdateHighlight()
    {
        
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;

            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, currentTime);

            UpdateHighlighting();

            yield return null;
        }
        lastIntensity = currentIntensity;
        yield return new WaitForSeconds(1);

    }
}
