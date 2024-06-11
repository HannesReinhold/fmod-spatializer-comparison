using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //public GameObject IntroductionPrefab;
    //public GameObject SubjectiveEvaluationPrefab;
    //public GameObject DirectionGuessingPrefab;

    private int sessionID = 0;

    public bool IsVR = true;
    public bool allowEvaluationNonVR = false;
    public bool isAssistant = false;


    public GameObject introductionObject;
    public GameObject subjectiveObject;
    public GameObject directionGuessingObject;
    public GameObject locationGuessingObject;
    public GameObject completeObject;
    public GameObject results;

    

    public GameObject roomModel;

    public GameObject rightControllerObject;
    public GameObject rayObject;

    public OVRPassthroughLayer passthroughLayer;

    public ServerLogEvent serverLog;


    public AppearingObject grabButton;
    public AppearingObject submitButton;
    public AppearingObject aButton;
    public AppearingObject menuButton;

    public GUISway hud;
    public PopupWindow currentSpaitializerWindow;

    public GameObject standingIndicator;

    public SkyboxChanger skyboxChanger;

    public DIrectionPoolGenerator baselineDirections;

    public GameObject baselineTarget;



    public enum EvaluationState
    {
        Introduction,
        DirectionGuessing,
        LocationGuessing,
        SubjectiveEvaluation,
        Complete
    }

    public EvaluationState startEvaluationState = 0;
    private EvaluationState evaluationState = 0;

    public List<GameObject> VRStuff;
    public List<GameObject> NonVRStuff;

    public List<GameObject> assistentStuff;

    private static GameManager instance;


    public DataManager dataManager;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            dataManager = new DataManager();


            foreach (GameObject g in VRStuff)
            {
                g.SetActive(IsVR);
            }

            foreach (GameObject g in NonVRStuff)
            {
                g.SetActive(!IsVR);
            }
            
            foreach(GameObject g in assistentStuff)
            {
                g.SetActive(isAssistant);
            }

            if (allowEvaluationNonVR) VRStuff[VRStuff.Count - 1].SetActive(true);




            StartNewSession();
        }


    }

    private void Start()
    {
        HideAllHighlights();
        Random.seed = System.DateTime.Now.Millisecond;

        hud.cameraTransform = Camera.main.transform;
    }

    public void HideAllHighlights()
    {
        SetGrabHighlightVisible(false);
        SetSubmitHighlightVisible(false);
        SetAHighlightVisible(false);
        SetMenuHighlightVisible(false);
        CloseSpatializerSwitchWindow();
        SetBaselineTargetVisibility(false);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            WindowManager currentWindowManager = FindFirstObjectByType<WindowManager>();
            currentWindowManager.OpenCurrentWindow();
        }

    }

    private void SetupWorldCamera()
    {
        Canvas[] canvasList = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvasList)
        {
            canvas.worldCamera = Camera.main;
            Debug.Log(Camera.main);
        }
    }

    public void StartNewSession()
    {
        if (sessionID != 0)
        {
        }

        SetAmbienVolume(1);



        dataManager.InitializeSession();
        HideRoomModel(0);

        if (sessionID == 0) evaluationState = startEvaluationState;
        else evaluationState = EvaluationState.Introduction;


        switch (evaluationState)
        {
            case EvaluationState.Introduction:
                StartIntroduction(); break;
            case EvaluationState.SubjectiveEvaluation:
                StartSubjectiveEvaluation(); break;
            case EvaluationState.DirectionGuessing:
                StartDirectionGuessing(); break;
            case EvaluationState.LocationGuessing:
                StartLocationGuessing(); break;
            case EvaluationState.Complete:
                StartComplete(); break;
            default: break;
        }

        InitializeGame();
        sessionID++;

    }

    public void StartNewScene()
    {
        SceneManager.LoadScene("IndustryScene");
    }

    public void Restart()
    {
        StartNewSession();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartNewSession();
    }

    public void InitializeGame()
    {
        Canvas[] foundCanvasses = FindObjectsOfType<Canvas>();
        foreach(Canvas c in foundCanvasses)
        {
            c.worldCamera = Camera.main;
        }
    }

    public void StartIntroduction()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Intro", transform.position);
        introductionObject.SetActive(true);
        if(sessionID!=0) introductionObject.GetComponent<MainIntroductionManager>().ResetMenu();
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(false);
        locationGuessingObject.SetActive(false);
        completeObject.SetActive(false);
        CloseSpatializerSwitchWindow();
        LogServerEvent("Position introduction");
        
    }

    public void StartSubjectiveEvaluation()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(true);
        directionGuessingObject.SetActive(false);
        locationGuessingObject.SetActive(false);
        completeObject.SetActive(false);
    }

    public void StartDirectionGuessing()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(true);
        locationGuessingObject.SetActive(false);
        completeObject.SetActive(false);
        CloseSpatializerSwitchWindow();
        SetBaselineTargetVisibility(true);
    }

    public void StartLocationGuessing()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(false);
        locationGuessingObject.SetActive(true);
        completeObject.SetActive(false);
        SetBaselineTargetVisibility(false);
    }

    public void StartComplete()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(false);
        locationGuessingObject.SetActive(false);
        completeObject.SetActive(true);
    }

    public void StartResults()
    {
        completeObject.SetActive(false);
        results.SetActive(true);
    }

    public void FinishSession()
    {
        SaveData();
    }

    public void SaveData()
    {
        dataManager.SaveSession();
    }

    public void ShowRoomModel(float time)
    {
        roomModel.SetActive(true);
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 1, time);
        }
        
    }

    private void DisableRoomModel()
    {
        roomModel.SetActive(false);
    }

    public void HideRoomModel(float time)
    {
        
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 0, time);
        }
        Invoke("DisableRoomModel", time);
    }

    public void HideController(bool stillshowController = false )
    {
        if(!stillshowController)rightControllerObject.SetActive(false);
        rayObject.SetActive(false);
    }

    public void ShowController()
    {
        rightControllerObject.SetActive(true);
        rayObject.SetActive(true);
    }

    public void SetPassthroughOpacity(float opacity)
    {
        passthroughLayer.textureOpacity = opacity;
    }


    public void LogServerEvents(int currentWindowManager, string additionalEvent="")
    {
        if (serverLog == null) return;
        serverLog.LogAll(currentWindowManager, additionalEvent);
    }

    public void LogServerEvent(string additionalEvent = "")
    {
        if (serverLog == null) return;
        serverLog.LogEvent(additionalEvent);
    }

    public void NextPageEvent(int nextPage)
    {
        if (serverLog == null) return;
        serverLog.NextPageEvent(nextPage);
    }

    public void BaselineSetPosition(Vector3 dir)
    {

    }

    public void RestartEvent()
    {
        if (serverLog == null) return;
        serverLog.NextPageEvent(-999);
    }

    public void SkipIntroductionEvent()
    {
        if (serverLog == null) return;
        serverLog.NextPageEvent(-100);
    }

    public void SkipIntroduction()
    {
        if (serverLog == null) return;
        StartSubjectiveEvaluation();
    }

    public void SetAmbienVolume(float i)
    {
        GUIAudioManager.SetAmbientVolume(i);
    }


    public void FadeGrabHighlightVisible(bool vis)
    {
        if (vis) grabButton.FadeIn();
        else grabButton.FadeOut();
    }

    public void FadeSubmitHighlightVisible(bool vis)
    {
        if (vis) submitButton.FadeIn();
        else submitButton.FadeOut();
    }

    public void FadeAHighlightVisible(bool vis)
    {
        if (vis) aButton.FadeIn();
        else aButton.FadeOut();
    }

    public void SetGrabHighlightVisible(bool vis)
    {
        if (vis) grabButton.SetVisible();
        else grabButton.SetInvisible();
    }

    public void SetSubmitHighlightVisible(bool vis)
    {
        if (vis) submitButton.SetVisible();
        else submitButton.SetInvisible();
    }

    public void SetAHighlightVisible(bool vis)
    {
        if (vis) aButton.SetVisible();
        else aButton.SetInvisible();
    }

    public void SetMenuHighlightVisible(bool vis)
    {
        if (vis) menuButton.SetVisible();
        else menuButton.SetInvisible();
    }



    public void VibrationCue()
    {
        Vib(2, 2.5f);
    }


    public void Vib(float start, float end)
    {
        Invoke("startVib", start);
        Invoke("stopVib", end);
    }

    public void startVib()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
    public void stopVib()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    public void ShowHUD()
    {
        hud.gameObject.SetActive(true);
        hud.Show();
    }

    public void HideHUD()
    {
        hud.Hide();
    }

    public void OpenSpatializerSwitchWindow()
    {
        currentSpaitializerWindow.gameObject.SetActive(true);
        currentSpaitializerWindow.Open();
    }

    public void CloseSpatializerSwitchWindow()
    {
        currentSpaitializerWindow.Close();
        currentSpaitializerWindow.gameObject.SetActive(false);
    }

    public void SetStandingIndicatorVisibility(bool show)
    {
        standingIndicator.SetActive(show);
    }

    public void SetStandingIndicatorHighlight(bool high)
    {
        CanvasGroup canvas = standingIndicator.GetComponentInChildren<CanvasGroup>();
        LeanTween.alphaCanvas(canvas, high ? 1 : 0.5f , 0.5f);
        MeshRenderer highlight = standingIndicator.GetComponentInChildren<MeshRenderer>();
        highlight.enabled = high;
    }

    public void SetSkyboxVisibility(bool vis)
    {
        skyboxChanger.SetPassthrough(vis);
    }

    public void SetBaselineDirection(int index)
    {
        //if (serverLog == null) return;
        Debug.Log("SetBaselineDirection");
        serverLog.NextBaselineDirection(index);
    }

    public void SetBaselineTargetVisibility(bool vis)
    {
        baselineTarget.SetActive(vis);
    }
}



