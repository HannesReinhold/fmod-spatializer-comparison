using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RaytracingVisualizer))]
public class RaytracingVisualizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RaytracingVisualizer myScript = (RaytracingVisualizer)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.StartRaycasting();
        }
    }
}
