using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ixRobotMasterControllerEditor : MonoBehaviour
{
    [CustomEditor(typeof(ixRobotMasterController))]
    public class RobotSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ixRobotMasterController ixRobotMasterControllerObject = (ixRobotMasterController)target;

            if (GUILayout.Button("Spawn Robots"))
            {
                SpawnRobots(ixRobotMasterControllerObject);
            }
            if (EditorGUI.EndChangeCheck()) {
                // Mark the object as dirty to ensure changes are saved
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void SpawnRobots(ixRobotMasterController ixRobotMasterControllerObject)
        {
            Vector3 position = new Vector3();
            Quaternion rotation = new Quaternion();

            // Arrange robots in a circle
            for (int i = 0; i < ixRobotMasterControllerObject.nRobots; i++)
            {
                position.z = ixRobotMasterControllerObject.fRadius * Mathf.Sin(i * Mathf.PI * 2.0f / ixRobotMasterControllerObject.nRobots);
                position.x = ixRobotMasterControllerObject.fRadius * Mathf.Cos(i * Mathf.PI * 2.0f / ixRobotMasterControllerObject.nRobots);
                rotation.SetLookRotation(position);
                
                GameObject r = Instantiate(ixRobotMasterControllerObject.robotPrefab, position + ixRobotMasterControllerObject.transform.parent.position, rotation, ixRobotMasterControllerObject.transform);
                r.name = "Robot." + i;
                ixRobotMasterControllerObject.robots.Add(r);
                Debug.Log(ixRobotMasterControllerObject.robots[i]+"Added");
                r.SetActive(true);

                ixRobotArmController rc = r.GetComponent<ixRobotArmController>();
                rc.TargetObject = rc.IdleTarget; // targetObject.transform;
                rc.id = i;
                
            }
        }
    }
}
