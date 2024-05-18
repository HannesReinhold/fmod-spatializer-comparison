using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class DirectionGuessingTutorial : MonoBehaviour
{
    public List<Vector3> positionList;

    public Transform centerTransform;

    public WindowManager windowManager;

    public GameObject target;

    private int roundID = 0;

    private bool enableInput = false;

    private Vector3 guessedDirection;
    private Vector3 actualDirection;
    private Vector3 playerPositionWhileGuessing;
    private bool showVisualizerSphere=false;
    private bool showDiffereLine = false;


    public int gridResolution = 8;

    public GameObject circlePrefab;
    public Transform sphereParent;
    private List<LineRenderer> sphereLines = new List<LineRenderer>();

    public GameObject GuessedPoint;

    public LineRenderer differenceLine;

    private int maxN = 0;


    private float currentAnimationTime = 0;
    private float maxAnimationTime = 10;

    private float targetRadius;
    private float lastCurrentRadius;

    private float targetAlpha;
    private float lastTargetAlpha;

    private float currentRadius;

    public GameObject differenceWindow;

    private float azimuthDifference;
    private float elevationDifference;
    private Vector3 scoreWindowPosition = Vector3.zero;

    public GameObject crosshairVisualizer;
    public GameObject guessedDirectionVisualizer;
    private Transform controllerTransform;


    public DirectionVisualizerEvent directionVisualizer;

    private void OnEnable()
    {
        showVisualizerSphere = false;
        showDiffereLine=false;

        maxN = 0;
        currentAnimationTime = 0;
        lastCurrentRadius = 0;
        targetAlpha = 0.3f;
        lastTargetAlpha=0;
        roundID = 0;
        Invoke("StartTutorial", 2);
        target.SetActive(false);

        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
        bus.setVolume(GameManager.Instance.dataManager.currentSessionData.volume);
        GUIAudioManager.SetAmbientVolume(0f);

        differenceWindow.GetComponentInChildren<PopupWindow>().Close();
    }

    private void Start()
    {
        CreateWireSphere();
        GameObject controller = GameObject.Find("RightHandAnchor");
        if (controller != null) controllerTransform = controller.transform;
    }

    private void Update()
    {


        if (enableInput && OVRInput.GetDown(OVRInput.Button.One)) Shoot();
        if (enableInput && target.activeSelf && Mouse.current.leftButton.wasPressedThisFrame) Shoot();


        
        if (showDiffereLine && currentAnimationTime<=maxAnimationTime)
        {
            currentRadius = Mathf.Lerp(lastCurrentRadius, targetRadius, currentAnimationTime);
            //SetSphereAlpha(Mathf.Lerp(lastTargetAlpha,targetAlpha,currentAnimationTime));
            //SetWireSphere(64, gridResolution);
            if (maxN < 100) SetGuessedDifference2((int)Mathf.Lerp(0, 100, currentAnimationTime - 1.5f));
            if (maxN >= 100) SetGuessedDifference2((int)Mathf.Lerp(100, 200, currentAnimationTime - 2.5f));
            
            currentAnimationTime += Time.deltaTime;
            //Debug.Log(currentAnimationTime);
        }
        //SetGuessedDifference(200);


        sphereParent.gameObject.SetActive(showVisualizerSphere);
        GuessedPoint.SetActive(showDiffereLine);
        differenceLine.gameObject.SetActive(showDiffereLine);
        //if(showDiffereLine) SetGuessedDifference2(maxN);
    }

    public void StartTutorial()
    {
        StartRound();
        currentAnimationTime = 0;
    }

    public void StartRound()
    {
        enabled = true;
        enableInput = true;

        target.transform.position = positionList[roundID];
        target.SetActive(true);

        // Play 3 times audio cue
        PlayAudioCue();
        Invoke("PlayAudioCue", 1);
        Invoke("PlayAudioCue", 2);

        // Invoke("Shoot", 3);
        showVisualizerSphere = false;
        showDiffereLine = false;
        currentAnimationTime = 0;
        maxN = 0;
        currentRadius=0;

        directionVisualizer.OpenCrosshair();
        directionVisualizer.OpenSphere();
    }

    void Shoot()
    {
        enableInput = false;
        Vib();
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Gunshot", transform.position);
        CancelInvoke("PlayAudioCue");

        playerPositionWhileGuessing = FindObjectOfType<FollowTarget>().transform.position;
        //showVisualizerSphere = true;
        showDiffereLine=true;

        targetRadius = Vector3.Distance(playerPositionWhileGuessing, target.transform.position);

        //SetWireSphere(64, gridResolution);
        SetGuessedPointPosition();
        //SetGuessedDifference();

        Invoke("ShowScore", 2);

        directionVisualizer.CloseCrosshair();
        GameManager.Instance.LogServerEvent("Direction Tutorial Shoot");
    }

    public void Vib()
    {
        startVib();
        Invoke("stopVib", 0.05f);
        Invoke("startVib", 0.1f);
        Invoke("stopVib", 0.15f);
    }
    public void startVib()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
    public void stopVib()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    private void PlayAudioCue()
    {
        GUIAudioManager.PlayMenuSubmit(target.transform.position);
    }

    void Evaluate()
    {
        roundID++;
        windowManager.NextPage();
        Debug.Log("Shoot");
    }



    public void ShowScore()
    {
        differenceWindow.transform.position = scoreWindowPosition;
        differenceWindow.SetActive(true);
        differenceWindow.GetComponent<PopupWindow>().Open();
        TextMeshProUGUI text = differenceWindow.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Measured difference:\n" + "Horiontal: " + azimuthDifference.ToString("F1") + "Degrees\nVertical: " + elevationDifference.ToString("F1") + " Degrees";
        Debug.Log(text);
        Invoke("HideScore",3);
        Invoke("Evaluate", 4);
    }

    public void HideScore()
    {
        differenceWindow.GetComponentInChildren<PopupWindow>().Close();
    }


    void CreateWireSphere()
    {
        for (int i = 0; i < gridResolution * 2-1; i++)
        {
            GameObject circle = Instantiate(circlePrefab);
            circle.transform.parent = sphereParent;
            LineRenderer line = circle.GetComponent<LineRenderer>();
            sphereLines.Add(line);
        }
    }

    void SetWireSphere(int numSegments, int gridN)
    {

        sphereParent.position = playerPositionWhileGuessing;


        int i = 0;

        if (gridN < 2)
        {
            gridN = 2;
        }

        int doubleSegments = gridN * 2;

        // Draw meridians

        float meridianStep = 180.0f / gridN;

        for (i = 0; i < gridN; i++)
        {
            sphereLines[i].positionCount = numSegments + 1;
            sphereLines[i].SetPositions(WireSphereDrawer.CreateCircle(Vector3.zero, Quaternion.Euler(0, meridianStep * i, 0), currentRadius, numSegments, Color.red));
            //DrawCircle(position, orientation * Quaternion.Euler(0, meridianStep * i, 0), radius, doubleSegments, color);
        }

        // Draw parallels

        Vector3 verticalOffset = Vector3.zero;
        float parallelAngleStep = Mathf.PI / (gridN);
        float stepRadius = 0.0f;
        float stepAngle = 0.0f;

        for (i = gridN; i < gridN*2-1; i++)
        {
            stepAngle = parallelAngleStep * (i-gridN+1);
            verticalOffset = (Vector3.up) * Mathf.Cos(stepAngle) * currentRadius;
            stepRadius = Mathf.Sin(stepAngle) * currentRadius;

            sphereLines[i].positionCount = numSegments + 1;
            sphereLines[i].SetPositions(WireSphereDrawer.CreateCircle(verticalOffset, Quaternion.Euler(90.0f, 0, 0), stepRadius, numSegments, Color.red));
        }
    }

    void SetGuessedPointPosition()
    {
       
        guessedDirection = (directionVisualizer.crosshairVisualizer.transform.position - centerTransform.position).normalized;
        GuessedPoint.transform.position = directionVisualizer.crosshairVisualizer.transform.position;

        float rad = Vector3.Distance(centerTransform.position, target.transform.position);
        targetRadius = rad;
        directionVisualizer.sphereEditor.SetSphere(rad,0.5f,1);
        //float rad = Vector3.Distance(playerPositionWhileGuessing, target.transform.position);
        //GuessedPoint.transform.position = playerPositionWhileGuessing + guessedDirection * rad;
    }

    void SetGuessedDifference(int max)
    {

        //TODO: Fix vertical difference. Currently its set to 0

        maxN = max;

        differenceLine.gameObject.transform.position = centerTransform.position;

        Vector3 actualDirection = (target.transform.position - centerTransform.position).normalized;

        // azimuth
        Vector3 horizontalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.up).normalized;
        Vector3 horizontalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.up).normalized;

        int n = 200;
        differenceLine.positionCount = maxN;

        float offset = -Vector3.SignedAngle(horizontalProjGuessed, Vector3.forward, Vector3.up);
        offset *= Mathf.Deg2Rad;
        float range = -Vector3.SignedAngle(horizontalProjActual, horizontalProjGuessed, Vector3.up);
        azimuthDifference = range;
        range *= Mathf.Deg2Rad;
        range /= (n / 2-1);
        Vector3 pos = new Vector3();
        Vector3 lastPos = new Vector3();
        float lastAzOffset=0;

        for (int i = 0; (i < maxN)&&(i<n/2); i++)
        {
            pos.x = Mathf.Sin(offset+ range * i) * currentRadius;
            pos.z = Mathf.Cos(offset+ range * i) * currentRadius;
            pos.y = 0;

            differenceLine.SetPosition(i, pos);
            lastAzOffset = offset + range * i;

            if (i == n / 2-1)
            {
                //scoreWindowPosition = pos + playerPositionWhileGuessing;
            }
        }

        Vector3 dir = pos.normalized;
        scoreWindowPosition = new Vector3(pos.x, -0.2f, pos.z) - dir * 0.3f + centerTransform.position;
        lastPos = pos;

        // elevation
        Vector3 verticalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        Vector3 verticalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.Cross(horizontalProjActual, Vector3.up));

        //Vector3 verticalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.right).normalized;
        //Vector3 verticalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.right).normalized;

        //Debug.DrawLine(centerTransform.position, centerTransform.position + (verticalProjGuessed), Color.green);
        //Debug.DrawLine(centerTransform.position, centerTransform.position + (horizontalProjActual), Color.red);


        offset = -Vector3.SignedAngle(verticalProjGuessed, horizontalProjGuessed, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        Debug.Log("O:"+offset);
        offset *= Mathf.Deg2Rad;
        //Debug.Log(offset);
        range = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up)) - offset;
        elevationDifference = range;
        //Debug.Log(range);
        range *= Mathf.Deg2Rad;
        range /= (n / 2);
        

        for (int i = n / 2; (i < maxN)&&(i<n); i++)
        {
            int index = i - n / 2;
            pos.x = Mathf.Sin(lastAzOffset) * Mathf.Cos(offset + range * index) * currentRadius;
            pos.z = Mathf.Cos(lastAzOffset) * Mathf.Cos(offset + range * index) * currentRadius;
            pos.y = Mathf.Sin(offset + range * index)* currentRadius;

            differenceLine.SetPosition(i, pos);
        }


        
        
    }

    void SetGuessedDifference2(int max)
    {
        int n = 200;
        maxN = max;
        differenceLine.positionCount = max;

        // set line origin
        differenceLine.gameObject.transform.position = centerTransform.position;
        Vector3 actualDirection = (target.transform.position - centerTransform.position).normalized;

        // azimuth
        Vector3 horizontalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.up).normalized;
        Vector3 horizontalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.up).normalized;

        // elevation
        Vector3 verticalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        Vector3 verticalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.Cross(horizontalProjActual, Vector3.up));

        // calculate start offset 
        float offsetAz = -Vector3.SignedAngle(horizontalProjGuessed, Vector3.forward, Vector3.up);
        float offsetEl = -Vector3.SignedAngle(verticalProjGuessed, horizontalProjGuessed, Vector3.Cross(horizontalProjGuessed, Vector3.up));

        float rangeAz = -Vector3.SignedAngle(horizontalProjActual, horizontalProjGuessed, Vector3.up);
        float rangeEl = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up)) - offsetEl;


        

        // set differences
        azimuthDifference = rangeAz;
        elevationDifference = rangeEl;

        offsetAz *= Mathf.Deg2Rad;
        offsetEl *= Mathf.Deg2Rad;

        rangeAz *= Mathf.Deg2Rad / (n / 2f - 1f);
        rangeEl *= Mathf.Deg2Rad / (n / 2f - 1f);

        Debug.Log("OffsetAz: " + offsetAz + ", offsetEl: " + offsetEl + ", rangeAz: " + rangeAz + ", rangeEl: " + rangeEl);

        Vector3 pos = Vector3.zero;
        for (int i = 0; (i < maxN) && (i < n/2); i++)
        {
            int index = i - n / 2;
            pos.x = Mathf.Sin(offsetAz + rangeAz * i) * Mathf.Cos(offsetEl) * currentRadius;
            pos.z = Mathf.Cos(offsetAz + rangeAz * i) * Mathf.Cos(offsetEl) * currentRadius;
            pos.y = Mathf.Sin(offsetEl) * currentRadius;

            differenceLine.SetPosition(i, pos);
        }

        Vector3 dir = pos.normalized;
        scoreWindowPosition = new Vector3(pos.x, -0.2f, pos.z) - dir * 0.3f + centerTransform.position;

        for (int i = n / 2; (i < maxN) && (i < n); i++)
        {
            int index = i - n / 2;
            pos.x = Mathf.Sin(offsetAz + rangeAz * n / 2) * Mathf.Cos(offsetEl + rangeEl * index) * currentRadius;
            pos.z = Mathf.Cos(offsetAz + rangeAz * n / 2) * Mathf.Cos(offsetEl + rangeEl * index) * currentRadius;
            pos.y = Mathf.Sin(offsetEl + rangeEl * index) * currentRadius;

            differenceLine.SetPosition(i, pos);
        }



    }

    void SetSphereAlpha(float alpha)
    {
        for(int i=0; i<sphereLines.Count; i++)
        {
            sphereLines[i].material.SetColor("_Color", new Color(0.8f, 0.8f, 0.8f, alpha));
            sphereLines[i].widthMultiplier = Mathf.Pow(alpha*4,2);
        }
    }

    public void CloseTutorial()
    {
        currentAnimationTime = 0;
        lastCurrentRadius = targetRadius;
        targetRadius = 0;
        lastTargetAlpha = targetAlpha;
        targetAlpha = 0;

        showDiffereLine = false;


        Invoke("Disable",2);


    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }



}