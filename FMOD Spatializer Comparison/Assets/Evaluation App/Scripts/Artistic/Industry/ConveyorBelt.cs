using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject SpawnObject;
    public GameObject SpawnObjectTutorial;

    public RobotArmController1 armController;

    public GameObject scanObject;
    public List<AlertLight> alertLights;
    public AlertLight approveLight;

    public PathCreation.PathCreator conveyorBelt1;
    public PathCreation.PathCreator armPath;
    public PathCreation.PathCreator trashPath;
    public PathCreation.PathCreator trashPath2;

    public PathCreation.PathCreator conveyorBelt2;

    public AppearingObject hologramEngineIndicator1;
    public AppearingObject hologramEngineIndicator2;

    public float spawnTime = 5;

    public float conveyorBeltSpeed = 1;
    public float armSpeed = 1;

    private float currentTime = 0;

    private PathCreation.Examples.PathFollower currentFollower = null;

    public bool isWorking = false;
    public bool manual = true;

    public Transform Grip;
    public Transform GripReal;


    public SPatializerSwitchManager spatializerManager;
    public SPatializerSwitchManager spatializerManagerTutorial;

    private List<GameObject> engineBlocks = new List<GameObject>();

    private Vector3 holoIndicatorPos;
    private Transform holoIndicatorTarget = null;

    private bool isTutorial = true;

    public ParticleSystem fireParticles;

    public GameObject manualController;
    public GameObject scanner;

    public RobotArmController1 robotController;

    public void PlaySpatializedOneShot(int a, int b, int index, Vector3 pos)
    {

        FMODUnity.RuntimeManager.PlayOneShot(spatializerManager.GetEvent(index)[spatializerManager.currentSpatializer], pos);
        //FMODUnity.RuntimeManager.PlayOneShot(spatializerManager.GetEvent(index)[b], pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = spawnTime-1;
        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);

        holoIndicatorPos = hologramEngineIndicator1.transform.position;

        manualController.SetActive(manual);
        //scanner.SetActive(manual);
        robotController.enabled = !manual;
    }


    public void SetTutorial(bool tut)
    {
        isTutorial = tut;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        isWorking = false;
    }

    public void StopAll()
    {
        isWorking = false;
        if (currentFollower != null) currentFollower.stopped = true;
        foreach (GameObject g in engineBlocks)
        {
            if (g == null) continue;
            if (isTutorial) spatializerManager.engineBlocks.Remove(g.GetComponent<SpatialAudioSwitcher>());
            else spatializerManagerTutorial.engineBlocks.Remove(g.GetComponent<SpatialAudioSwitcher>());
            Destroy(g);
        }

        engineBlocks.Clear();

        if (isTutorial) spatializerManagerTutorial.StopAll();
        else spatializerManager.StopAll();

        currentTime = spawnTime - 1;

        armController.Restart();

        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);


        CancelInvoke("EndRobot");
        CancelInvoke("EndRobot2");
        CancelInvoke("StartSiren");
        CancelInvoke("MoveToTrash");
        CancelInvoke("ActivateApproveLight");
        CancelInvoke("StartRobot");
        CancelInvoke("MoveToEngine");


        /*
        foreach(GameObject g in engineBlocks)
        {
            if (g == null) continue;
            //if(isTutorial) spatializerManagerTutorial.engineBlocks.Remove(g.GetComponent<SpatialAudioSwitcher>());
           //else spatializerManager.engineBlocks.Remove(g.GetComponent<SpatialAudioSwitcher>());
            g.GetComponent<PathCreation.Examples.PathFollower>().stopped = true;
        }

        if(currentFollower!=null) currentFollower.stopped = true;

        
        if (isTutorial) spatializerManagerTutorial.StopAll();
        else spatializerManager.StopAll();

        currentTime = spawnTime - 1;

        

        armController.Restart();

        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);

        
        CancelInvoke("EndRobot");
        CancelInvoke("EndRobot2");
        CancelInvoke("StartSiren");
        CancelInvoke("MoveToTrash");
        CancelInvoke("ActivateApproveLight");
        CancelInvoke("StartRobot");
        CancelInvoke("MoveToEngine");

        foreach (GameObject g in engineBlocks)
        {
            if (g == null) continue;
            RemoveEngine(g);
        }

        engineBlocks.Clear();
        */
    }

    public void Restart()
    {
        currentTime = spawnTime-1;

        foreach(GameObject g in engineBlocks)
        {
            if (isTutorial) spatializerManager.engineBlocks.Remove(g.GetComponent<SpatialAudioSwitcher>());
            else spatializerManagerTutorial.engineBlocks.Remove(g.GetComponent<SpatialAudioSwitcher>());
            Destroy(g);
        }

        engineBlocks.Clear();

        armController.RestartSlow();

        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);

        CancelInvoke("EndRobot");
        CancelInvoke("EndRobot2");
        CancelInvoke("StartSiren");
        CancelInvoke("MoveToTrash");
        CancelInvoke("ActivateApproveLight");
        CancelInvoke("StartRobot");
        CancelInvoke("MoveToEngine");

        StartWorking();

        if(isTutorial) spatializerManagerTutorial.PlayAll();
        else spatializerManager.PlayAll();
        foreach (GameObject g in engineBlocks)
        {
            g.GetComponent<SpatialAudioSwitcher>().SetSpatializer(spatializerManager.currentSpatializer);
        }
    }

    public void RemoveEngine(GameObject e)
    {
        if (isTutorial) spatializerManager.engineBlocks.Remove(e.GetComponent<SpatialAudioSwitcher>());
        else spatializerManagerTutorial.engineBlocks.Remove(e.GetComponent<SpatialAudioSwitcher>());
        engineBlocks.Remove(e);
        Destroy(e);
    }

    public void StartWorking()
    {
        isWorking = true;
        foreach (GameObject g in engineBlocks)
        {
            g.GetComponent<PathCreation.Examples.PathFollower>().stopped = false;
        }
        if(currentFollower!=null) currentFollower.stopped = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (!isWorking) return;

        if (holoIndicatorTarget != null) hologramEngineIndicator1.transform.position = holoIndicatorTarget.position;
        if (currentFollower != null) currentFollower.transform.position = GripReal.position + new Vector3(0,-0.2f,0);

        if (currentTime >= spawnTime)
        {
            currentTime = 0;
            SpawnMotor();
        }
        currentTime += Time.deltaTime;
    }

    public void Grab()
    {
        
        holoIndicatorTarget = Grip;
        currentFollower.enabled = false;

        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 1, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 1, currentFollower.transform.position);
    }

    public void UnGrab()
    {
        if (currentFollower == null) return;
        currentFollower.enabled = true;
        currentFollower.distanceTravelled = 0;
        currentFollower.pathCreator = conveyorBelt2;
        currentFollower.speed = conveyorBeltSpeed * 0.6f;
        SetApproveLight(false);
        ResetHologramIndicator();

        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 2, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 2, currentFollower.transform.position);

        currentFollower = null;
    }

    public void Incinerate()
    {
        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 4, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 4, currentFollower.transform.position);
        currentFollower.distanceTravelled = 0;
        currentFollower.pathCreator = trashPath;
        currentFollower.speed = conveyorBeltSpeed * 0.6f;
        currentFollower.enabled = true;
        Invoke("DeleteEngine", 1);
        //RemoveEngine(currentFollower.gameObject);
        SetAlertLights(false);
        ResetHologramIndicator();
        fireParticles.Play();
    }

    public void DeleteEngine()
    {
        
        RemoveEngine(currentFollower.gameObject);
        currentFollower = null;
    }

    void SpawnMotor()
    {
        GameObject motor = null;
        if(isTutorial) motor = Instantiate(SpawnObjectTutorial);
        else motor = Instantiate(SpawnObject);
        PathCreation.Examples.PathFollower follower = motor.GetComponent<PathCreation.Examples.PathFollower>();
        follower.pathCreator = conveyorBelt1;
        follower.speed = conveyorBeltSpeed;
        engineBlocks.Add(motor);
        if (isTutorial) motor.GetComponent<SpatialAudioSwitcher>().SetSpatializer(spatializerManagerTutorial.currentSpatializer);
        else motor.GetComponent<SpatialAudioSwitcher>().SetSpatializer(spatializerManager.currentSpatializer);
        if (isTutorial) spatializerManagerTutorial.engineBlocks.Add(motor.GetComponent<SpatialAudioSwitcher>());
        else spatializerManager.engineBlocks.Add(motor.GetComponent<SpatialAudioSwitcher>());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (manual)
        {
            hologramEngineIndicator1.FadeIn();

            if (other.GetComponent<PathCreation.Examples.PathFollower>() == null) return;
            currentFollower = other.GetComponent<PathCreation.Examples.PathFollower>();
            Invoke("StartScanning", 1f);

            if (other.GetComponent<EngineBlock>().isFaulty)
            {
                Invoke("StartSiren", 2.5f);
            }
            else
            {
                Invoke("ActivateApproveLight", 2.5f);
            }
        }

        else
        {

            if (!isWorking) return;
            PathCreation.Examples.PathFollower follower = other.GetComponent<PathCreation.Examples.PathFollower>();
            follower.speed = 0;
            currentFollower = follower;
            armController.SetTarget(follower.transform.position + new Vector3(0.5f, 0.4f, 0));
            Invoke("StartScanning", 1f);


            if (other.GetComponent<EngineBlock>().isFaulty)
            {
                Invoke("StartSiren", 2.5f);
                Invoke("MoveToTrash", 3.5f);
            }
            else
            {
                Invoke("ActivateApproveLight", 2.5f);
                Invoke("StartRobot", 3f);
            }

            Invoke("MoveToEngine", 2.5f);


        }
    }

    private void OnTriggerExit(Collider other)
    {
        //hologramEngineIndicator1.FadeOut();
    }

    void MoveToEngine()
    {
        armController.SetTarget(currentFollower.transform.position);
        scanObject.SetActive(false);
    }

    void StartSiren()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Siren", alertLights[0].transform.position);
        if(isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 0, alertLights[0].transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 0, alertLights[0].transform.position);
        SetAlertLights(true);
    }

    void MoveToTrash()
    {
        armController.SetTargetTransform(currentFollower.transform);
        currentFollower.pathCreator = trashPath2;
        currentFollower.speed = armSpeed;
        currentFollower.distanceTravelled = 0;
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Grab", currentFollower.transform.position);
        if (isTutorial) PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 1, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 1, currentFollower.transform.position);
        Invoke("EndRobot2", 3f);
    }

    void StartScanning()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Scanner 2", currentFollower.transform.position);
        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 3, scanObject.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 3, scanObject.transform.position);
        scanObject.SetActive(true);
        Invoke("StopScanning",1f);
    }

    void StopScanning()
    {
        scanObject.SetActive(false);
    }

    void StartRobot()
    {
        armController.SetTargetTransform(currentFollower.transform);
        currentFollower.pathCreator = armPath;
        currentFollower.speed = armSpeed;
        currentFollower.distanceTravelled = 0;
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Grab", currentFollower.transform.position);
        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 1, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 1, currentFollower.transform.position);
        Invoke("EndRobot",2.4f);
        
    }

    void EndRobot()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Release", currentFollower.transform.position);
        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 2, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 2, currentFollower.transform.position);
        currentFollower.distanceTravelled = 0;
        currentFollower.pathCreator = conveyorBelt2;
        currentFollower.speed = conveyorBeltSpeed * 0.6f;
        currentFollower = null;
        armController.canMove = false;
        Invoke("SetStartTarget", 0.1f);
        SetApproveLight(false);

    }

    void EndRobot2()
    {

        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Release", currentFollower.transform.position);
        //if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 2, currentFollower.transform.position);
        //else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 2, currentFollower.transform.position);
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Fire Swoosh", currentFollower.transform.position);
        if (isTutorial) PlaySpatializedOneShot(spatializerManagerTutorial.spatializerA, spatializerManagerTutorial.spatializerB, 4, currentFollower.transform.position);
        else PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 4, currentFollower.transform.position);
        RemoveEngine(currentFollower.gameObject);
        currentFollower = null;
        armController.canMove = false;
        Invoke("SetStartTarget",0.1f);
        SetAlertLights(false);
        fireParticles.Play();

    }

    void SetStartTarget()
    {
        armController.SetStartTarget();
        armController.canMove = true;
    }

    void SetAlertLights(bool on)
    {
        foreach(AlertLight l in alertLights)
        {
            l.SetBrightness(on ? 0.1f : 0);
        }
    }

    void SetApproveLight(bool on)
    {
        approveLight.SetBrightness(on ? 0.1f : 0);
    }

    void ActivateApproveLight()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Approved", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 5, currentFollower.transform.position);
        SetApproveLight(true);
    }

    public void ResetHologramIndicator()
    {
        hologramEngineIndicator1.FadeOut();
        Invoke("ResetHologramIndicator2",1);
    }
    private void ResetHologramIndicator2()
    {
        holoIndicatorTarget = null;
        hologramEngineIndicator1.transform.position = holoIndicatorPos;
    }
}
