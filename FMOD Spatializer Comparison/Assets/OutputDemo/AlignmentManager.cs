using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using Meta.XR.MRUtilityKitSamples.QRCodeDetection;
using System;

public class AlignmentManager : MonoBehaviour
{
    public OutputDemoManager demoManager;

    public Bounded2DVisualizer qrTracker;
    private OVRSpatialAnchor anchor;
    public Transform ovrHead;
    public PopupWindow alignmentTutorialWindow;
    public FlyingWindow alignmentFlyingWindow;
    public AlignmentWindow alignmentWindow;

    public List<string> hints;

    public Gradient distanceColor;

    private float distToTracker;
    private float angleToTracker;
    private bool trackerActive=false;
    private UnityEngine.Vector3 trackerLastPosition;
    private float trackingLossTimer=0;

    private bool isAligned=false;



    public void UpdateAlignment()
    {

        CheckForTracker();
        MoveAlignmentWindow();

        CheckForAnchor();
    }

    private void CheckForTracker()
    {
        if (qrTracker==null) qrTracker = (Bounded2DVisualizer)FindAnyObjectByType(typeof(Bounded2DVisualizer));
        if (qrTracker==null) return;

        if (UnityEngine.Vector3.Distance(trackerLastPosition, qrTracker.transform.position) != 0)
        {
            trackingLossTimer=0;
            trackerActive=true;
        }

        else
        {
            trackingLossTimer+=Time.deltaTime;
            if(trackingLossTimer>=1) trackerActive=false;
        }
        trackerLastPosition = qrTracker.transform.position;

        distToTracker = UnityEngine.Vector3.Distance(ovrHead.position, qrTracker.transform.position);
        angleToTracker = UnityEngine.Vector3.Angle(ovrHead.forward, -qrTracker.transform.forward);

        float distToTargetDiff = Mathf.Pow(Mathf.Clamp01(Mathf.Abs(distToTracker-0.4f)),2);
        Color trackerColor = distanceColor.Evaluate(distToTargetDiff);
        qrTracker.GetComponentInChildren<LineRenderer>().startColor = trackerColor;
        qrTracker.GetComponentInChildren<LineRenderer>().endColor = trackerColor;
        if (distToTracker < 0.2)
        {
            alignmentWindow.SetText("To close");
        }else if (distToTracker < 0.4)
        {
            alignmentWindow.SetText("Back of a little...");
        }else if (distToTracker < 0.5)
        {
            alignmentWindow.SetText("Hold this distance");
        }else if (distToTracker < 1)
        {
            alignmentWindow.SetText("Come a little closer...");
        }
        else
        {
            alignmentWindow.SetText("To far");
        }
        //alignmentWindow.SetText("Distance: "+distToTracker+", Diff: "+distToTargetDiff+", AngleDiff: "+angleToTracker);
        Debug.Log("Distance: "+distToTracker+", Diff: "+distToTargetDiff+", AngleDiff: "+angleToTracker);
    }

    private void CheckForAnchor()
    {
        if(anchor==null) anchor = (OVRSpatialAnchor)FindAnyObjectByType(typeof(OVRSpatialAnchor));
        if(anchor!=null && !isAligned)
        {
            isAligned=true;
            alignmentTutorialWindow.Close();
            OnAlignmentComplete();
        }
    }

    private void MoveAlignmentWindow()
    {
        if (qrTracker!=null && trackerActive)
        {
            alignmentFlyingWindow.alwaysInFront = false;
            alignmentFlyingWindow.alwaysFacing = false;
            alignmentFlyingWindow.target = qrTracker.transform;
            alignmentFlyingWindow.offset = new UnityEngine.Vector3(0.3f,0,0.01f);
            alignmentFlyingWindow.speed= 2;
        }
        else
        {
            alignmentFlyingWindow.alwaysInFront = true;
            alignmentFlyingWindow.alwaysFacing = true;
            alignmentFlyingWindow.target = null;
            alignmentFlyingWindow.speed = 0.5f;
        }
    }

    public void StartAlignment()
    {
        Debug.Log("Start Alignment");
    }


    public void OnAlignmentComplete()
    {
        Debug.Log("Alignment Complete");
        demoManager.StartPositioning();
    }

    public void ShowAlignmentWindow()
    {
        alignmentTutorialWindow.Open();
    }

    public void HideAlignmentWindow()
    {
        alignmentTutorialWindow.Close();
    }
}
