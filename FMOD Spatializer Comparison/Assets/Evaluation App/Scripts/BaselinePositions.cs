using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class BaselinePositions : MonoBehaviour
{
    public DirectionGuessingGameManager manager;

    public GameObject target;
    public Transform controllerTransform;


    public PopupWindow startWindow;
    public PopupWindow finishWindow;


    public FMODUnity.StudioEventEmitter emitter;

    public GameObject noiseDistraction;

    [SerializeField] public AudioEvent[] events;

    private Vector3 guessedDirection;

    public int currentRound = 0;
    public int numRounds = 10;
    public int countdownTime = 3;

    public int maxRounds = 5;

    private bool enableInput = false;

    private float startTime = 0;

    public bool tutorial = false;


    private List<DirectionGuessingData> guessList = new List<DirectionGuessingData>();
    private int currentSpatializer = 0;
    private int currentPositionID = 0;

    public FMODUnity.StudioEventEmitter distractionEmitter;

    public DirectionVisualizerEvent directionVisualizer;

    public GameObject targetVis;


    private List<int> alreadySpawned = new List<int>();

    void OnEnable()
    {
        //StartGame();
        //controllerTransform = GameObject.Find("RightHandAnchor").transform;
        //GUIAudioManager.SetAmbientVolume(0.1f);

        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
        //bus.setVolume(GameManager.Instance.dataManager.currentSessionData.volume);
        bus.setVolume(1);
        StopDistraction();
    }

    private void Start()
    {

    }

    public void OnStartClick()
    {
        StartGame();
        GameObject controller = GameObject.Find("RightHandAnchor");
        if (controller != null) controllerTransform = controller.transform;
        else controllerTransform = Camera.main.transform;


        GUIAudioManager.SetAmbientVolume(0f);
    }




    void Update()
    {
        // Shoot if pressed
        if (enableInput && target.activeSelf && OVRInput.GetDown(OVRInput.Button.Two)) Shoot();
        if (enableInput && target.activeSelf && Mouse.current.leftButton.wasPressedThisFrame) Shoot();

    }

    /// <summary>
    /// Initial start game
    /// </summary>
    public void StartGame()
    {
        //startWindow.gameObject.SetActive(true);
        //startWindow.Open();


        target.SetActive(false);
        GameManager.Instance.HideController(true);
    }

    /// <summary>
    /// end the game
    /// </summary>
    public void OnFinishClick()
    {
        GameManager.Instance.StartLocationGuessing();
        GUIAudioManager.SetAmbientVolume(0.5f);
        GameManager.Instance.ShowController();
    }

    public void FinishGame()
    {
        GameManager.Instance.SaveData();
        manager.windowManager.ResetSlow();


        finishWindow.gameObject.SetActive(true);

        DisableControllerInput();

        GameManager.Instance.SetBaselineDirection(-1);
    }

    /// <summary>
    /// Starts a small waiting period until the round starts
    /// </summary>
    public void StartCountdown()
    {
        PlayDistraction();
        Invoke("StopDistraction",countdownTime-0.5f);

        GUIAudioManager.SetAmbientVolume(0.0f);
        //startWindow.Close();
        Invoke("StopDistraction", countdownTime-0.5f);
        Invoke("StartRound", countdownTime);
        // hide target

        int id = Mathf.RoundToInt(Random.Range(0, GameManager.Instance.baselineDirections.actualCount));
        for(int i=0; i<alreadySpawned.Count; i++)
        {
            if (id == alreadySpawned[i])
            {
                id = Mathf.RoundToInt(Random.Range(0, GameManager.Instance.baselineDirections.actualCount));
            }
        }
        GameManager.Instance.SetBaselineDirection(id);
        alreadySpawned.Add(id);
        
    }

    public void PlayDistraction()
    {
        noiseDistraction.SetActive(true);
    }

    public void StopDistraction()
    {
        noiseDistraction.SetActive(false);
    }

    /// <summary>
    /// Start the round
    /// </summary>
    private void StartRound()
    {
        
        Vector3 respawnPosition =  targetVis.transform.position;
        Debug.Log(respawnPosition);
        int spatializerID = 3;

        //respawnPosition = Random.onUnitSphere*1.5f;


        EnableControllerInput();

        startTime = Time.time;
        RespawnTarget(respawnPosition, spatializerID);
        Debug.Log("Start");
    }

    /// <summary>
    /// Enable controller input
    /// </summary>
    private void EnableControllerInput()
    {
        enableInput = true;
    }

    /// <summary>
    /// Disable controller input
    /// </summary>
    private void DisableControllerInput()
    {
        enableInput = false;
    }


    /// <summary>
    /// Spawn the target at a new random direction
    /// </summary>
    void RespawnTarget(Vector3 position, int spatializerID)
    {
        currentSpatializer = spatializerID;
        // select a random position
        //Vector2 dir = Random.insideUnitCircle.normalized;
        //Vector3 position = new Vector3(controllerTransform.position.x,0,controllerTransform.position.z) +new Vector3(dir.x,0.6f,dir.y)*3;


        target.transform.position = position;
        target.SetActive(true);

        // Play 3 times audio cue

        Debug.Log("Respawn at "+position);
    }

    /// <summary>
    /// Shoots a ray in the guessed direction
    /// </summary>
    void Shoot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Gunshot", controllerTransform.position);
        // auditive and tactile feedback
        Vib();

        // disable target and input
        target.SetActive(false);
        DisableControllerInput();

        EvaluateShot();

        GameManager.Instance.LogServerEvent("Direction Game Baseline Shoot");
        Debug.Log("shoot");
    }



    /// <summary>
    /// Calculates the error for the guessed direction and starts a new round
    /// </summary>
    void EvaluateShot()
    {
        // calculate azimuth and elevation error
        Vector3 headPosition = FindFirstObjectByType<FollowTarget>().transform.position;
        guessedDirection = (directionVisualizer.crosshairVisualizer.transform.position - headPosition).normalized;

        Vector3 actualDirection = (target.transform.position - headPosition).normalized;

        // azimuth
        Vector3 horizontalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.up).normalized;
        Vector3 horizontalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.up).normalized;

        float azimuthGuess = -Vector3.SignedAngle(horizontalProjGuessed, Vector3.forward, Vector3.up);
        float azimuthActual = -Vector3.SignedAngle(horizontalProjActual, Vector3.forward, Vector3.up);
        float azimuthDif = -Vector3.SignedAngle(horizontalProjActual, horizontalProjGuessed, Vector3.up);

        Vector3 verticalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        Vector3 verticalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.Cross(horizontalProjActual, Vector3.up));

        float evlevationGuess = -Vector3.SignedAngle(verticalProjGuessed, horizontalProjGuessed, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        float evlevationActual = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up));
        float evlevationDif = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up)) - evlevationGuess;
        float azimuth = azimuthDif;
        float elevation = evlevationDif;

        // draw azimuth projection of guessed and actual
        //DrawDirectionLines(new Vector3(guessedDirection.x, guessedDirection.y, guessedDirection.z).normalized, new Vector3(actualDirection.x, actualDirection.y, actualDirection.z).normalized);

        // show error as text
        //textMesh.text = "Azimuth: " + azimuth + "\n Elevation: " + elevation;


        // show target

        float elapsedTime = Time.time - startTime;


        DirectionGuessingData data = new DirectionGuessingData(currentRound);
        data.spatializerID = currentSpatializer;
        data.timeToGuessDirection = elapsedTime;
        data.sourceDirection = actualDirection;
        data.guessedDirection = guessedDirection;
        data.azimuthDifference = azimuth;
        data.elevationDifference = elevation;
        data.stimuliID = 0;


        GameManager.Instance.dataManager.currentSessionData.directionGuessingBaseLineResults.Add(data);
        GameManager.Instance.SaveData();


        currentRound++;
        currentSpatializer = currentRound % 4 + 1;

        // if all rounds are over, finish game
        if (currentRound < numRounds && currentRound < maxRounds)
            Invoke("StartCountdown", 1);
        else
            Invoke("FinishGame", 1);
    }

    /// <summary>
    /// Vibration of the controller
    /// </summary>
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
}
