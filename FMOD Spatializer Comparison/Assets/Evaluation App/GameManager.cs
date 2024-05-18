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


    public GameObject introductionObject;
    public GameObject subjectiveObject;
    public GameObject directionGuessingObject;
    public GameObject completeObject;

    public GameObject roomModel;

    public GameObject rightControllerObject;
    public GameObject rayObject;

    public OVRPassthroughLayer passthroughLayer;

    public ServerLogEvent serverLog;




    public enum EvaluationState
    {
        Introduction,
        SubjectiveEvaluation,
        DirectionGuessing,
        Complete
    }

    public EvaluationState startEvaluationState = 0;
    private EvaluationState evaluationState = 0;

    public List<GameObject> VRStuff;
    public List<GameObject> NonVRStuff;

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
        DontDestroyOnLoad(gameObject);

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

            if (allowEvaluationNonVR) VRStuff[VRStuff.Count - 1].SetActive(true);




            StartNewSession();
        }


    }

    private void Start()
    {
        SetupWorldCamera();
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
            case EvaluationState.Complete:
                StartComplete(); break;
            default: break;
        }

        InitializeGame();
        sessionID++;

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
        completeObject.SetActive(false);
    }

    public void StartSubjectiveEvaluation()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(true);
        directionGuessingObject.SetActive(false);
        completeObject.SetActive(false);
    }

    public void StartDirectionGuessing()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(true);
        completeObject.SetActive(false);
    }

    public void StartComplete()
    {
        introductionObject.SetActive(false);
        subjectiveObject.SetActive(false);
        directionGuessingObject.SetActive(false);
        completeObject.SetActive(true);
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
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 1, time);
        }
    }

    public void HideRoomModel(float time)
    {
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 0, time);
        }

    }

    public void HideController()
    {
        rightControllerObject.SetActive(false);
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
        serverLog.LogAll(currentWindowManager, additionalEvent);
    }

    public void LogServerEvent(string additionalEvent = "")
    {
        serverLog.LogEvent(additionalEvent);
    }

    public void NextPageEvent(int nextPage)
    {
        serverLog.NextPageEvent(nextPage);
    }

    public void RestartEvent()
    {
        serverLog.NextPageEvent(-999);
    }

    public void SkipIntroductionEvent()
    {
        serverLog.NextPageEvent(-100);
    }

    public void SkipIntroduction()
    {
        StartSubjectiveEvaluation();
    }
}



