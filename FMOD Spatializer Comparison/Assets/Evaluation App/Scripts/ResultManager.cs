using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    SessionData data;

    public GameObject directionalBaselineParent;
    public GameObject directionalAccuracyParent;
    public GameObject positionalAccuracyParent;


    void OnEnable()
    {
        data = GameManager.Instance.dataManager.currentSessionData;
    }



    public void DisplayDirectionalBaselineResults()
    {

    }

    public void DisplayDirectionalAccuracyResults()
    {

    }

}
