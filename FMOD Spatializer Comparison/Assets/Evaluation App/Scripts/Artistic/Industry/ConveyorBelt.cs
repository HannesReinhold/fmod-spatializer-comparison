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
    public PathCreation.PathCreator conveyorBelt2;

    public float spawnTime = 5;

    public float conveyorBeltSpeed = 1;
    public float armSpeed = 1;

    private float currentTime = 0;

    private PathCreation.Examples.PathFollower currentFollower = null;

    public bool isWorking = false;


    public SPatializerSwitchManager spatializerManager;
    public SPatializerSwitchManager spatializerManagerTutorial;

    private List<GameObject> engineBlocks = new List<GameObject>();

    private bool isTutorial = true;

    public void PlaySpatializedOneShot(int a, int b, int index, Vector3 pos)
    {

        FMODUnity.RuntimeManager.PlayOneShot(spatializerManager.GetEvent(index)[a], pos);
        FMODUnity.RuntimeManager.PlayOneShot(spatializerManager.GetEvent(index)[b], pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = spawnTime-1;
        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);

        
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
        foreach(GameObject g in engineBlocks)
        {
            if (g == null) continue;
            g.GetComponent<PathCreation.Examples.PathFollower>().stopped = true;
        }

        if(currentFollower!=null) currentFollower.stopped = true;

        spatializerManager.StopAll();

        currentTime = spawnTime - 1;

        foreach (GameObject g in engineBlocks)
        {
            if (g==null) continue;
            Destroy(g);
        }

        engineBlocks.Clear();

        armController.Restart();

        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);

        CancelInvoke("EndRobot");
        CancelInvoke("EndRobot2");
    }

    public void Restart()
    {
        currentTime = spawnTime-1;

        foreach(GameObject g in engineBlocks)
        {
            Destroy(g);
        }

        engineBlocks.Clear();

        armController.Restart();

        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);

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
        engineBlocks.Remove(e);
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

        if (currentTime >= spawnTime)
        {
            currentTime = 0;
            SpawnMotor();
        }
        currentTime += Time.deltaTime;
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
        motor.GetComponent<SpatialAudioSwitcher>().SetSpatializer(spatializerManager.currentSpatializer);
    }

    private void OnTriggerEnter(Collider other)
    {
        PathCreation.Examples.PathFollower follower = other.GetComponent<PathCreation.Examples.PathFollower>();
        follower.speed = 0;
        currentFollower = follower;
        armController.SetTarget(follower.transform.position + new Vector3(0.5f,0.4f,0));
        Invoke("StartScanning",1f);
        

        if (other.GetComponent<EngineBlock>().isFaulty)
        {
            Invoke("StartSiren",2.5f);
            Invoke("MoveToTrash",3.5f);
        }
        else
        {
            Invoke("ActivateApproveLight", 2.5f);
            Invoke("StartRobot", 3f);
        }

        Invoke("MoveToEngine", 2.5f);
        

        Debug.Log("Hit");
    }

    void MoveToEngine()
    {
        armController.SetTarget(currentFollower.transform.position);
        scanObject.SetActive(false);
    }

    void StartSiren()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Siren", alertLights[0].transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 0, alertLights[0].transform.position);
        SetAlertLights(true);
    }

    void MoveToTrash()
    {
        armController.SetTargetTransform(currentFollower.transform);
        currentFollower.pathCreator = trashPath;
        currentFollower.speed = armSpeed;
        currentFollower.distanceTravelled = 0;
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Grab", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 1, currentFollower.transform.position);
        Invoke("EndRobot2", 3f);
    }

    void StartScanning()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Scanner 2", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 3, currentFollower.transform.position);
        scanObject.SetActive(true);
    }

    void StartRobot()
    {
        armController.SetTargetTransform(currentFollower.transform);
        currentFollower.pathCreator = armPath;
        currentFollower.speed = armSpeed;
        currentFollower.distanceTravelled = 0;
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Grab", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 1, currentFollower.transform.position);
        Invoke("EndRobot",2.4f);
        
    }

    void EndRobot()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Release", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 2, currentFollower.transform.position);
        currentFollower.distanceTravelled = 0;
        currentFollower.pathCreator = conveyorBelt2;
        currentFollower.speed = conveyorBeltSpeed * 0.6f;
        currentFollower = null;
        armController.SetStartTarget();
        SetApproveLight(false);

    }

    void EndRobot2()
    {

        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Release", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 2, currentFollower.transform.position);
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Fire Swoosh", currentFollower.transform.position);
        PlaySpatializedOneShot(spatializerManager.spatializerA, spatializerManager.spatializerB, 4, currentFollower.transform.position);
        Destroy(currentFollower.gameObject);
        currentFollower = null;
        armController.SetStartTarget();
        SetAlertLights(false);
       

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
}
