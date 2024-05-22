using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedWindow : MonoBehaviour
{
    public SubjectiveEvaluationManager evaluationManager;
    public WindowManager windowManager;

    public float time = 1;

    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("Complete",time);
    }


    void Complete()
    {
        if (evaluationManager != null) evaluationManager.NextRound();
        else windowManager.NextPage();
    }

}
