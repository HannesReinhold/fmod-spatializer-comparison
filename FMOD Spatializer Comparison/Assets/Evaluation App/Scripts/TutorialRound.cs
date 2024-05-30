using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRound : MonoBehaviour
{
    public ToggleGroup spatializerSwitch;
    public UnityEngine.UI.Toggle t1;
    public UnityEngine.UI.Toggle t2;
    int setSpat = 0;

    public SPatializerSwitchManager spatialManager;


    private void Start()
    {
        setSpat = 0;
        t1.isOn = true;
        t2.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && setSpat == 1) { setSpat = 0; ToggleSpatializerButton(0); };
        if (OVRInput.GetDown(OVRInput.Button.Two) && setSpat == 0) { setSpat = 1; ToggleSpatializerButton(1); };
    }

    void ToggleSpatializer(int index)
    {
        spatialManager.SetSpatializer(index == 0 ? spatialManager.spatializerA : spatialManager.spatializerB); 
    }

    private void ToggleSpatializerButton(int index)
    {
        if (index == 0) t1.isOn = true;
        else t2.isOn = true;
    }
}
