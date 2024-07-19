using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoShoot : MonoBehaviour
{
    void Start()
    {
        //Invoke("Set",2);
    }

    void Set()
    {
        GameManager.Instance.NextState(1);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            GameManager.Instance.NextState(-1);
        }
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            GameManager.Instance.NextState(1);
        }
    }
}
