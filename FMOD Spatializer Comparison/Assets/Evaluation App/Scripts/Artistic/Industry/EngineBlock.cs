using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBlock : MonoBehaviour
{
    public bool isFaulty;

    public GameObject normalModel;
    public GameObject faultyModel;

    void Start()
    {
        isFaulty = Random.value > 0.5 ? true : false;

        normalModel.SetActive(!isFaulty);
        faultyModel.SetActive(isFaulty);
    }

}
