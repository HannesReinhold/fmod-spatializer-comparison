using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionVisualizerEvent : MonoBehaviour
{
    public DirectionGuessingGameManager dirGameManager;

    public WireSphereEditor sphereEditor;

    public GameObject sphereCollider;

    public GameObject speakerVisualizer;
    public GameObject crosshairVisualizer;

    public float radius = 2f;


    private float targetSpeakerSize = 0;
    private float lastSpeakerSize = 0;
    private float speakerSize;

    private float currentAnimationFrame = 0;
    private float currentCrosshairFrame = 0;

    private float targetCrosshairSize = 0;
    private float lastCrosshairSize = 0;
    private float crosshairSize;


    public Vector3 dir;
    private Transform controllerTransform;

    private bool isRunning=false;


    private void OnEnable()
    {
        //StartVisualizer();
        speakerVisualizer.transform.localScale = Vector3.zero;
        crosshairVisualizer.transform.localScale = Vector3.zero;
        sphereEditor.SetSphereInstant(0, 0, 0);
    }

    // Start is called before the first frame update
    void Awake()
    {
        sphereEditor.animationTime = 2;

        GameObject controller = GameObject.Find("RightHandAnchor");
        if (controller != null) controllerTransform = controller.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isRunning) return;
        //Vector3 mousePos = Input.mousePosition;
        Raycast(controllerTransform.position, new Vector3(-0.1f,0.2f,1).normalized);

    }

    private void OnDisable()
    {
    }

    public void Close()
    {
        CloseSphere();
        CloseSpeaker();
        CloseCrosshair();
        CancelInvoke("PlayCue");

        Invoke("CloseEvent",1);

        isRunning = false;
    }

    private void CloseEvent()
    {
        dirGameManager.CloseEvent(1);
    }

    public void OpenVisualizer()
    {
        Invoke("OpenCrosshair", 1);
        OpenSphere();
        isRunning = true;
    }


    public void StartVisualizer()
    {
        isRunning = true;
        Invoke("OpenCrosshair",4);

        Invoke("OpenSphere", 3);
        speakerVisualizer.transform.localScale = Vector3.zero;
        MapSpeakerToSphere(new Vector3(0.3f, 0, 1).normalized);
        Invoke("OpenSpeaker", 6);
        Invoke("PlayCue", 7);
        Invoke("CloseSpeaker", 8);

        Invoke("Map1",10);
        Invoke("OpenSpeaker", 10);
        Invoke("PlayCue", 11);
        Invoke("CloseSpeaker", 12);

        Invoke("Map2", 14);
        Invoke("OpenSpeaker", 14);
        Invoke("PlayCue", 15);
        Invoke("CloseSpeaker", 16);

        Invoke("Map3", 18);
        Invoke("OpenSpeaker", 18);
        Invoke("PlayCue", 19);
        Invoke("CloseSpeaker", 20);

        //Invoke("CloseSphere", 22);
        //Invoke("CloseCrosshair", 22);
    }


    public void MapSpeakerToSphere(Vector3 dir)
    {
        Vector3 mappendPosition = (transform.forward * dir.x + transform.right * dir.y + transform.up * dir.z) * radius;
        speakerVisualizer.transform.position = sphereEditor.parent.position + mappendPosition;
    }

    public void MapCrosshairToSphere(Vector3 dir)
    {


        Vector3 mappendPosition = (transform.forward * dir.x + transform.right * dir.y + transform.up * dir.z) * radius;
        speakerVisualizer.transform.position = sphereEditor.parent.position + mappendPosition;
    }

    public void Raycast(Vector3 pos, Vector3 dir)
    {
        if (controllerTransform != null) dir = (controllerTransform.forward - controllerTransform.right * 0.1f).normalized;

        int layerMask = 1 << 6;
        RaycastHit hit;
        if(Physics.Raycast(pos+dir*10, -dir, out hit, 100, layerMask))
        {
            crosshairVisualizer.transform.position = hit.point;

            crosshairVisualizer.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal) * transform.rotation;
            Vector3 newRotation = crosshairVisualizer.transform.eulerAngles;
            crosshairVisualizer.transform.rotation = Quaternion.Euler(newRotation.x,newRotation.y,0);
            Debug.DrawLine(controllerTransform.position, controllerTransform.position+dir);
            Debug.DrawLine(controllerTransform.position, hit.point);
        }

        /*
        Vector3 actualDir = (speakerVisualizer.transform.position - controllerTransform.position).normalized;

        if (Vector3.Dot(actualDir, dir) < 0.1f)
        {
            crosshairVisualizer.transform.position = speakerVisualizer.transform.position;
            speakerVisualizer.transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            speakerVisualizer.transform.localScale = new Vector3(1,1,1);
        }
        */

    }


    public void OpenSphere()
    {
        sphereEditor.SetSphereInstant(radius * 0.9f, 0, 0);
        sphereEditor.SetSphere(radius, 0.5f, 1);
    }


    public void CloseSphere()
    {
        sphereEditor.SetSphere(radius, 0, 0);
    }


    public void OpenSpeaker()
    {
        ToggleSpeaker(true);
    }

    public void CloseSpeaker()
    {
        ToggleSpeaker(false);
    }

    public void OpenCrosshair()
    {
        ToggleCrosshair(true);
        isRunning = true;
    }

    public void CloseCrosshair()
    {
        ToggleCrosshair(false);
        isRunning = false;
    }

    private void ToggleCrosshair(bool open)
    {
        currentCrosshairFrame = 0;
        targetCrosshairSize = open ? 1 : 0;
        StartCoroutine(SetCrosshair());
    }


    private void ToggleSpeaker(bool open)
    {
        currentAnimationFrame = 0;
        lastSpeakerSize = speakerSize;
        targetSpeakerSize = open ? 1 : 0;

        StartCoroutine(SetSpeaker());
    }


    private void Map1()
    {
        MapSpeakerToSphere(new Vector3(-0.5f, -0.2f, 0.7f).normalized);
    }

    private void Map2()
    {
        MapSpeakerToSphere(new Vector3(0.1f, 0.3f, 0.4f).normalized);
    }

    private void Map3()
    {
        MapSpeakerToSphere(new Vector3(0.6f, 0.4f, 0.4f).normalized);
    }

    private void PlayCue()
    {
        GUIAudioManager.PlayMenuSubmit(speakerVisualizer.transform.position);
    }



    IEnumerator SetSpeaker()
    {

        while (currentAnimationFrame < 1f)
        {
            currentAnimationFrame += Time.deltaTime/0.3f;
            speakerSize = Mathf.Lerp(lastSpeakerSize, targetSpeakerSize, currentAnimationFrame);
            speakerVisualizer.transform.localScale = new Vector3(speakerSize, speakerSize, speakerSize);

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

    }

    IEnumerator SetCrosshair()
    {

        while (currentCrosshairFrame < 1f)
        {
            currentCrosshairFrame += Time.deltaTime / 1f;
            crosshairSize = Mathf.Lerp(lastSpeakerSize, targetCrosshairSize, currentCrosshairFrame);
            crosshairVisualizer.transform.localScale = new Vector3(crosshairSize, crosshairSize, crosshairSize);

            yield return null;
        }

        yield return new WaitForSeconds(1);

    }


}