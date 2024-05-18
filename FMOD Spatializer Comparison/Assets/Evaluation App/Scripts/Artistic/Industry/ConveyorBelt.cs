using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject SpawnObject;

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

    // Start is called before the first frame update
    void Start()
    {
        currentTime = spawnTime;
        scanObject.SetActive(false);
        SetAlertLights(false);
        SetApproveLight(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= spawnTime)
        {
            currentTime = 0;
            SpawnMotor();
        }
        currentTime += Time.deltaTime;
    }

    void SpawnMotor()
    {
        GameObject motor = Instantiate(SpawnObject);
        PathCreation.Examples.PathFollower follower = motor.GetComponent<PathCreation.Examples.PathFollower>();
        follower.pathCreator = conveyorBelt1;
        follower.speed = conveyorBeltSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        PathCreation.Examples.PathFollower follower = other.GetComponent<PathCreation.Examples.PathFollower>();
        follower.speed = 0;
        currentFollower = follower;
        armController.SetTarget(follower.transform.position + new Vector3(-0.5f,0.4f,0));
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Siren", alertLights[0].transform.position);
        SetAlertLights(true);
    }

    void MoveToTrash()
    {
        armController.SetTargetTransform(currentFollower.transform);
        currentFollower.pathCreator = trashPath;
        currentFollower.speed = armSpeed;
        currentFollower.distanceTravelled = 0;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Grab", currentFollower.transform.position);
        Invoke("EndRobot2", 2.7f);
    }

    void StartScanning()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Scanner 2", currentFollower.transform.position);
        scanObject.SetActive(true);
    }

    void StartRobot()
    {
        armController.SetTargetTransform(currentFollower.transform);
        currentFollower.pathCreator = armPath;
        currentFollower.speed = armSpeed;
        currentFollower.distanceTravelled = 0;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Grab", currentFollower.transform.position);
        Invoke("EndRobot",2.4f);
        
    }

    void EndRobot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Release", currentFollower.transform.position);
        currentFollower.distanceTravelled = 0;
        currentFollower.pathCreator = conveyorBelt2;
        currentFollower.speed = conveyorBeltSpeed * 0.6f;
        currentFollower = null;
        armController.SetStartTarget();
        SetApproveLight(false);

    }

    void EndRobot2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Release", currentFollower.transform.position);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Fire Swoosh", currentFollower.transform.position);
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
        FMODUnity.RuntimeManager.PlayOneShot("event:/Industrial/Steam/Approved", currentFollower.transform.position);
        SetApproveLight(true);
    }
}
