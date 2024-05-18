using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ixRobotMasterController : MonoBehaviour
{
    public static ixRobotMasterController Get()
    {
        return Instance;
    }

    public static ixRobotMasterController Instance;

    public UnityEvent GotObjectFromHuman;

    public int nRobots = 6;
    public float fRadius = 7.0f;
    public float speedRotation = 10.0f;
    public float positionSmoothTime = 0.2f;
    public float maxSpeed = 0.8f;
    public GameObject robotPrefab;
    public GameObject targetObject;

    [SerializeField] public List<GameObject> robots = new List<GameObject>();


    //Vector3 pos = new Vector3();

    public Transform initialTransform;

    int firstRobotId;

    private void Awake()
    {
        Instance = this;
        Debug.Log(robots.Count+"Robot Count");
        // Check if the list is null and initialize it if needed
        // if (robots == null)
        // {
        // 	robots = new List<GameObject>();
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Vector3 position = new Vector3();
        // Quaternion rotation = new Quaternion();
        //
        // // Arrange robots in a circle
        // for (int i = 0; i < nRobots; i++)
        // {
        // 	position.z = fRadius * Mathf.Sin(i * Mathf.PI * 2.0f / nRobots);
        // 	position.x = fRadius * Mathf.Cos(i * Mathf.PI * 2.0f / nRobots);
        // 	rotation.SetLookRotation(position);
        // 	GameObject r = Instantiate(robotPrefab, position + transform.parent.position, rotation, transform);
        // 	//r.GetComponent<NetworkObject>().Spawn(true);
        // 	
        // 	r.name = "Robot." + i;
        // 	robots.Add(r);
        // 	r.SetActive(true);
        //
        // 	ixRobotArmController rc = r.GetComponent<ixRobotArmController>();
        // 	rc.TargetObject = rc.IdleTarget; // targetObject.transform;
        // 	rc.id = i;
        // }
    }

    public void OnGrab(ixRobotArmController robot, GameObject collidingObject)
    {
        if (robot.HeldObject != null)
        {
            if (collidingObject == robot.GroundTarget.gameObject)
            {
                robot.HeldObject.transform.SetParent(null, true);
                robot.state = ixRobotArmController.State.Sleeping;
                robot.HeldObject = null;
                robot.timeOut = 1.0f;
                robot.TargetObject = robot.IdleTarget;
                return;
            }
            else
                return;
        }

        // Transfer ownership..
        ixRobotArmController lastRobot = collidingObject.GetComponentInParent<ixRobotArmController>();

        if (lastRobot)
        {
            lastRobot.state = ixRobotArmController.State.Sleeping;
            lastRobot.HeldObject = null;
            lastRobot.timeOut = 1.0f;
            lastRobot.TargetObject = lastRobot.IdleTarget;
        }
        else
        {
            // Object given to the robot by a human..
            firstRobotId = robot.id;

            GotObjectFromHuman?.Invoke();
        }

        robot.HeldObject = collidingObject;
        robot.state = ixRobotArmController.State.Holding;
        robot.HeldObject.transform.SetParent(robot.IKEnd.transform, true);
//		Debug.Log(robot.name + " Grabs " + collidingObject.name);

        int nextRobotId = (robot.id + 1) % robots.Count;
        if (nextRobotId == firstRobotId)
        {
            robot.TargetObject = robot.GroundTarget;
        }
        else
        {
            ixRobotArmController nextRobot = robots[nextRobotId].GetComponent<ixRobotArmController>();

            robot.TargetObject = nextRobot.IKEnd;
            nextRobot.TargetObject = collidingObject.transform;
            nextRobot.state = ixRobotArmController.State.Seeking;

//			Debug.Log(robot.gameObject.name + " Passes " + collidingObject.name + " to " + nextRobot.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < nRobots; i++)
        {
            GameObject r = robots[i];
            ixRobotArmController rc = r.GetComponent<ixRobotArmController>();

            // Rotate our transform a step closer to the target's.
            if (rc.TargetObject != null)
            {
                rc.UpdateIKTarget(speedRotation, positionSmoothTime, maxSpeed, Time.deltaTime);
            }

            // We snap it repeatedly to take over control from the user interface
            if (rc.state == ixRobotArmController.State.Holding && rc.HeldObject != null)
                rc.HeldObject.transform.localPosition = Vector3.zero;
        }
    }

    // Function to reset the cube to the initial position
    public void ResetToInitialPosition()
    {
        if (initialTransform != null)
        {
            targetObject.transform.position = initialTransform.position;
        }
        else
        {
            Debug.LogWarning("ResetTransform is not set. Cannot reset object.");
        }
    }
}