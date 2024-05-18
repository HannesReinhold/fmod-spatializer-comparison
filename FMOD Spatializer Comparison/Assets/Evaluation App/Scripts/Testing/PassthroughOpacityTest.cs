using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughOpacityTest : MonoBehaviour
{
    public OVRPassthroughLayer passthroughLayer;
    public Renderer passthroughMaterial;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = passthroughMaterial.material;
    }

    // Update is called once per frame
    void Update()
    {
        float mix = Mathf.Min(Mathf.Max(Mathf.Sin(Time.time*0.5f)*2, 0),1);
        //passthroughLayer.textureOpacity = Mathf.Sin(Time.time)*0.5f+0.5f;
        mat.SetFloat("_Opacity", mix);
    }
}
