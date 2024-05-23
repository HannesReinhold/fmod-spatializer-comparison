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

    private float acc = 0;

    private bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        target = grapTransform.position;
        Invoke("SetTarget",5);
    }

    public void Restart()
    {
        grapTransform.position = startPosition;
        target = startPosition;
    }

    void SetTarget()
    {
        canMove = true;
        startPosition = grapTransform.position;
        SetStartTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove) return;
        if(targetTransform==null) grapTransform.position = Vector3.Lerp(grapTransform.position, target, Time.deltaTime * vel * acc);
        else grapTransform.position = Vector3.Lerp(grapTransform.position, targetTransform.position+targetTransform.up*0.5f, Time.deltaTime*10 * vel * acc);
        acc += Time.fixedDeltaTime * 3f;
        acc = Mathf.Clamp(acc,0,1);
    }

    public void StartRobotArm()
    {

    }

    public void SetTarget(Vector3 pos)
    {
        target = pos;
        targetTransform = null;
        vel = 1.5f;
        acc = 0;

    }
    public void SetTargetTransform(Transform pos)
    {
        targetTransform = pos;
        vel = 1.5f;
        acc = 0;
    }

    public void SetStartTarget()
    {
        SetTarget(startPosition);
        acc = 0;
    }
}
