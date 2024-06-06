using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public Material skyBox;
    public OVRPassthroughLayer passthrough;
    public Camera cam;

    private float target = 1;
    private float lastTarget = 1;
    private float currentValue;
    private float t = 0;
    public void SetPassthrough(bool on)
    {
        cam = Camera.main;
        lastTarget = target;
        target = on ? 1 : 0;

        StartCoroutine(FadeInOutPassthrough());
    }

    IEnumerator FadeInOutPassthrough()
    {
        t = 0;
        
        while (t<1)
        {
            currentValue = Mathf.Lerp(lastTarget, target, t);
            passthrough.textureOpacity = currentValue;
            cam.backgroundColor = new Color(0.9f * (1-currentValue), 0.85f * (1 - currentValue), 0.82f * (1 - currentValue), (1 - currentValue)) ;
            t += Time.deltaTime/3f;
            yield return null;
        }

        yield return new WaitForSeconds(3f);
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //SetPassthrough(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
