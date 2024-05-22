using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmController1 : MonoBehaviour
{

    public Transform grapTransform;
    private Vector3 target;
    private Transform targetTransform;

    private float vel = 2;

    private Vector3 startPosition;

    private bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        target = grapTransform.position;
        Invoke("SetTarget",5);
    }

    void SetTarget()
    {
        canMove = true;
        startPosition = grapTransform.position;
        SetStartTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        if(targetTransform==null) grapTransform.position = Vector3.Lerp(grapTransform.position, target, Time.deltaTime * vel);
        else grapTransform.position = Vector3.Lerp(grapTransform.position, targetTransform.position+targetTransform.up*0.5f, Time.deltaTime*10 * vel);
    }

    public void StartRobotArm()
    {

    }

    public void SetTarget(Vector3 pos)
    {
        target = pos;
        targetTransform = null;
        vel = 1.5f;

    }
    public void SetTargetTransform(Transform pos)
    {
        targetTransform = pos;
        vel = 1.5f;
    }

    public void SetStartTarget()
    {
        SetTarget(startPosition);
    }
}
