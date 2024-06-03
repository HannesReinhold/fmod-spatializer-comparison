using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualRobotArmController : MonoBehaviour
{
    public Transform controllSection;
    public Transform moveSection;

    public Transform controllerTransform;
    public Transform robotArmTransform;

    public float maxVelocity = 0.1f;
    public float acceleration;

    private float currentVelocity = 0;


    private Transform rightControllerTransform;

    // Start is called before the first frame update
    void Start()
    {
        rightControllerTransform = GameObject.Find("RightControllerAnchor").transform;
    }

    // Update is called once per frame
    void Update()
    {

        controllerTransform.position = rightControllerTransform.position;
        Vector3 controllerPos = controllerTransform.position;

        Vector3 controllSectionScale = controllSection.localScale;
        Vector3 moveScale = moveSection.localScale;

        float offsetX = (controllerPos.x - (controllSection.position.x - controllSectionScale.x/2f)) / controllSectionScale.x;
        float offsetY = (controllerPos.y- (controllSection.position.y - controllSectionScale.y / 2f)) / controllSectionScale.y;
        float offsetZ = (controllerPos.z - (controllSection.position.z - controllSectionScale.z / 2f)) / controllSectionScale.z;

        offsetX = Mathf.Clamp01(offsetX);
        offsetY = Mathf.Clamp01(offsetY);
        offsetZ = Mathf.Clamp01(offsetZ);

        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);


        Vector3 mappedPosition = new Vector3(
            moveSection.position.x - moveScale.x / 2f + offsetX * moveScale.x,
            moveSection.position.y - moveScale.y / 2f + offsetY * moveScale.y,
            moveSection.position.z - moveScale.z / 2f + offsetZ * moveScale.z
            );

        Vector3 direction = (mappedPosition-robotArmTransform.position).normalized;
        float currentVel = (mappedPosition-robotArmTransform.position).magnitude;
        currentVel = Mathf.Clamp(currentVel*acceleration, 0, maxVelocity) * acceleration;

        //currentVelocity = Mathf.Lerp(currentVelocity, currentVel, Time.deltaTime * acceleration);

        robotArmTransform.position = robotArmTransform.position + direction * currentVel * Time.deltaTime;


        Debug.Log(currentVel);
    }
}
