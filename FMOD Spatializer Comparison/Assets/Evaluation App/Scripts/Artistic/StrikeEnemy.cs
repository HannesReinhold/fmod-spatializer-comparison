using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeEnemy : MonoBehaviour
{
    public Transform rightControllerTransform;
    public GameObject rightController;
    public GameObject ray;

    public GameObject sword;
    public GameObject swordTip;
    public GameObject enemy;

    public GameObject music;



    private float currentOpacity = 0;
    private bool equipped = false;
    private Renderer rend;

    public AudioSource source;
    public AudioClip clip;
    private Vector3 lastPosition = Vector3.zero;
    private bool alreadySwung = false;

    public MainIntroductionManager introductionManager;
    public WindowManager windowManager;

    private GameObject laser;

    public PopupWindow completeWindow;
    public PopupWindow warningWindow;

    private void Start()
    {
        
        GameObject anchor = GameObject.Find("RightHandAnchor");
        if (anchor != null) rightControllerTransform = anchor.transform;
        rend = sword.GetComponentInChildren<Renderer>();

        Invoke("EquipSword",3);
        rend.material.SetFloat("Opacity", 0.0f);

        if(rightControllerTransform!=null) lastPosition = rightControllerTransform.position;
        source = sword.GetComponent<AudioSource>();
        source.clip = clip;
        

        
    }

    private void OnEnable()
    {
        
        completeWindow.gameObject.SetActive(false);
        if (rightControllerTransform != null) lastPosition = rightControllerTransform.position;
        Invoke("EquipSword", 1);
        
        enemy.SetActive(true);
        music.SetActive(true);
        sword.SetActive(false);
        rend.material.SetFloat("Opacity", 0.0f);

    }



    private void Update()
    {
        if (rightControllerTransform == null) return;

        sword.transform.position = rightControllerTransform.position;
        sword.transform.rotation = rightControllerTransform.rotation;

        if(equipped && currentOpacity<1)
        {
            currentOpacity += Time.deltaTime * 0.4f;
            rend.material.SetFloat("_Opacity", currentOpacity);
        }

        float d = Vector3.Distance(swordTip.transform.position, lastPosition)/Time.deltaTime;
        if (d > 10f)
        {
            if (!alreadySwung)
            {
                source.PlayOneShot(clip);
                alreadySwung = true;
            }
        }
        else if(d<1)
        {
            alreadySwung = false;
        }
        lastPosition = swordTip.transform.position;

        Debug.Log("Vel: "+d);
    }

    private void ResetSwing()
    {
        alreadySwung=false;
    }


    public void EquipSword()
    {

        GameManager.Instance.HideController();
        sword.SetActive(true);
        equipped = true;

        laser = GameObject.Find("LaserPointer");
        if (laser != null) laser.GetComponent<LineRenderer>().widthMultiplier = 0;

        warningWindow.Close();

    }

    public void OpenComplete()
    {
        completeWindow.gameObject.SetActive(true);
        warningWindow.Close();
        completeWindow.transform.position = enemy.transform.position;
    }

    public void OnKill()
    {
        //laser.SetActive(true);
        

        Invoke("OpenComplete",0.5f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Knight/KnightDefeat", enemy.transform.position);
        FMODUnity.StudioEventEmitter audio = enemy.GetComponent<FMODUnity.StudioEventEmitter>();    
        audio.Stop();
        music.SetActive(false);

        Invoke("UnEquippSword",1);
    }

    private void UnEquippSword()
    {
        sword.SetActive(false);
        if (laser != null) laser.GetComponent<LineRenderer>().widthMultiplier = 1;
        GameManager.Instance.ShowController();
    }

    private void StartNewEvent()
    {

        windowManager.NextPage();
        windowManager.NextPage();
        introductionManager.StartEvent(1);
    }

    public void OnCompleClick()
    {
        completeWindow.Close();
        Invoke("StartNewEvent",1);
    }

}
