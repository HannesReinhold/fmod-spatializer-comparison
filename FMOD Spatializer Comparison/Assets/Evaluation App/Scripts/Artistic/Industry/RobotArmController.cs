using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmController : MonoBehaviour
{
    [SerializeField]
    public List<RobotJointSettings> robotJointSettings;

    private float[] currentRotations;


    void Start()
    {
        currentRotations = new float[robotJointSettings.Count];
    }


    void Update()
    {
        int i = 0;
        foreach(RobotJointSettings setting in robotJointSettings)
        {
            float speed = Mathf.Sign(setting.speed) * Mathf.Pow(setting.speed, 2);

            currentRotations[i] += Mathf.Abs(speed)>=0.05f ? speed : 0;
            setting.anchorTransform.localEulerAngles = new Vector3(0, currentRotations[i], 0);
            i++;
        }
    }
}

[System.Serializable]
public struct RobotJointSettings
{
    public Transform anchorTransform;

    [Range(-1, 1)] public float speed;

}
