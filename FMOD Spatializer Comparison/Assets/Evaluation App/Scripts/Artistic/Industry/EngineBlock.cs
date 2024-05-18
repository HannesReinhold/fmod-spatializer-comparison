using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBlock : MonoBehaviour
{
    public bool isFaulty;

    void Start()
    {
        isFaulty = Random.value > 0.5 ? true : false;
    }

}
