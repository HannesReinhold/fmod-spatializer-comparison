using System;
using UnityEngine;

public class ixRobotClaw : MonoBehaviour
{
    public ixRobotArmController controller;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "RobotTarget") return;
        /*
        if (NetworkManager.Singleton.IsClient)
        {
            if (col.gameObject.name == "GroundTarget") controller.OnGrab(col.gameObject);

            if (col.GetComponent<NetworkObject>().IsOwner)
                try
                {
                    col.GetComponent<ForceControlInteractables>().ForceRelease();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
        }

        if (NetworkManager.Singleton.IsServer) controller.OnGrab(col.gameObject);
        */
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag != "RobotTarget") return;

        /*
        if (NetworkManager.Singleton.IsServer)
            //			Debug.Log(controller.gameObject.name + " Trigger Exit");
            controller.OnRelease(col.gameObject);
        */
    }
}