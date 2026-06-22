using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPathManager : MonoBehaviour
{
    public List<Transform> pathObjects = new List<Transform>();

    public GameObject mover;

    public bool enableMoving=false;

    public int targetIndex = 0;
    public float transitionFactor = 0.1f;
    private float interpolatedTarget = 0;
    private int lastTargetIndex;



    public int currentTarget = 0;


    private void Update()
    {
        //if(currentTarget != targetIndex) SetTargetIndex(currentTarget);
        if (!enableMoving) return;

        if (targetIndex == lastTargetIndex)
        {
            mover.transform.position = pathObjects[targetIndex].position;
            return;
        }

        interpolatedTarget = Mathf.Lerp(interpolatedTarget, targetIndex, Time.deltaTime*transitionFactor);
        float offset = Mathf.Abs(targetIndex - interpolatedTarget) / Mathf.Abs(targetIndex - lastTargetIndex);

        Vector3 pos = pathObjects[lastTargetIndex].position * (offset) + pathObjects[targetIndex].position * (1-offset);

        mover.transform.position = pos;
        //Debug.Log("Move");
    }

    public void SetTargetIndex(int i)
    {
        enableMoving = true;
        currentTarget = i;

        lastTargetIndex = targetIndex;
        targetIndex = i;
    }

    public void DisableMoving()
    {
        enableMoving = false;
    }
}
