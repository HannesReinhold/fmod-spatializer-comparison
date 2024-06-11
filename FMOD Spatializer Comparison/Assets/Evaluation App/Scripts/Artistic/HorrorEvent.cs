using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEvent : MonoBehaviour
{
    public GameObject roomModel;
    private Renderer roomRenderer;
    public Light light;

    public MainIntroductionManager mainIntroductionManager;
    public WindowManager windowManager;

    public float minFog = 0;
    public float maxFog = 0.5f;

    private bool enableFog = false;

    private float currentFogTarget;
    private float currentFogValue;

    public GameObject ambience;
    public GameObject ambience2;
    public GameObject heartbeat;

    public EyesSpawner eyeSpawner;
    public GameObject jumpScarePrefab;
    public GameObject jumpscareSpring;

    public PopupWindow popupWindow;
    public GameObject completeWindow;

    public Transform monsterTransform;

    private bool playMonsterSounds = false;

    private float currentGrowlTime = 0;
    private float nextRandomGrowlTime = 1;

    private float currentFootTime = 0;
    private float nextRandomFootTime = 1;

    public GameObject monster;

    public Renderer passthroughBox;

    public Lightsflicker lightsFlicker;

    private float currentRoomAlpha;
    private float roomAlphaTarget;



    private void OnEnable()
    {
        GameManager.Instance.ShowRoomModel(3);

        ambience.SetActive(true);

        // variation 2
        monster.SetActive(false);
        Invoke("EnableFog",2.5f);
        Invoke("EnableEyes", 4);
        Invoke("CloseIntroWindow", 5);

        Invoke("DisableNoises",16);
        Invoke("DisableAmbience", 17);
        Invoke("EnableJumpscare", 18);
        Invoke("DisableEyes", 20);
        Invoke("OnComplete", 20);

        GUIAudioManager.SetAmbientVolume(0);

        GameManager.Instance.HideController();

        roomAlphaTarget = 1;

    }

    private void OnDisable()
    {
        GameManager.Instance.ShowController();

        currentFogTarget = 0;
        currentRoomAlpha = 0;

    }





    public void OnComplete()
    {
        //DisableEyes();
        Invoke("DisableFog",2);
        Invoke("OpenCompleteWindow",4);
        GameManager.Instance.HideRoomModel(2);
        GameManager.Instance.ShowController();
    }

    public void EnableFog()
    {
        currentFogTarget = maxFog;
        lightsFlicker.StopFlicker();
    }

    public void DisableFog()
    {
        currentFogTarget = minFog;
        roomAlphaTarget = 0;
    }

    public void EnableEyes()
    {
        eyeSpawner.SpawnEyes();
        monster.SetActive(true);
    }

    public void DisableEyes()
    {
        eyeSpawner.RemoveEyes();
        monster.SetActive(false);
    }

    public void EnableJumpscare()
    {
        //roomModel.SetActive(true);
        //passthroughBox.enabled = true;

        
        Vector3 lookDir = FindAnyObjectByType<FollowTarget>().transform.eulerAngles;
        Vector3 camPos = FindAnyObjectByType<FollowTarget>().transform.position;
        //jumpscareSpring.transform.position = new Vector3(camPos.x, 0, camPos.z) + new Vector3(Mathf.Sin(lookDir.y * Mathf.Deg2Rad),0,Mathf.Cos(lookDir.y * Mathf.Deg2Rad));
        jumpscareSpring = Instantiate(jumpScarePrefab, new Vector3(camPos.x, 0, camPos.z) + new Vector3(Mathf.Sin(lookDir.y * Mathf.Deg2Rad), 0, Mathf.Cos(lookDir.y * Mathf.Deg2Rad)), Quaternion.identity);
        //jumpscareSpring.transform.LookAt(new Vector3(camPos.x, 0, camPos.z));
        jumpscareSpring.GetComponentInChildren<Canvas>().transform.LookAt(new Vector3(camPos.x, jumpscareSpring.GetComponentInChildren<Canvas>().transform.position.y, camPos.z));
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Boing", jumpscareSpring.transform.position);
        Debug.Log("Boo");
    }

    private void DisableAmbience()
    {
        ambience.SetActive(false);
        ambience2.SetActive(false);
    }

    private void DisableHeartbeat()
    {
        heartbeat.SetActive(false);
    }

    private void CloseIntroWindow()
    {
        windowManager.CloseCurrentWindow();
        playMonsterSounds = true;
    }

    private void OpenCompleteWindow()
    {
        windowManager.NextPage();
    }

    public void OnCompleteClick()
    {
        GUIAudioManager.SetAmbientVolume(0.5f);
        completeWindow.GetComponent<PopupWindow>().Close();
        Invoke("DisableEvent",0.5f);
        Invoke("OpenNextPage",1.5f);
    }

    private void OpenNextPage()
    {
        windowManager.NextPage();
    }


    private void DisableEvent()
    {
        roomModel.SetActive(false);
        gameObject.SetActive(false);
        Destroy(jumpscareSpring);
        
    }



    private void Update()
    {
        currentFogValue = 0.98f * currentFogValue + 0.02f * currentFogTarget;
        currentRoomAlpha = 0.995f * currentRoomAlpha + 0.005f * roomAlphaTarget;
        //passthroughBox.material.SetFloat("_Opacity", currentFogValue / maxFog);
        RenderSettings.fogDensity = currentFogValue;
        //light.intensity = Mathf.Pow(1-currentFogValue/maxFog,1) * 0.2f;
        //GameManager.Instance.SetPassthroughOpacity(1 - currentFogValue / maxFog);


        if (!playMonsterSounds) return;
        currentGrowlTime += Time.deltaTime;
        if (currentGrowlTime >= nextRandomGrowlTime)
        {
            nextRandomGrowlTime = Random.Range(2f,4);
            currentGrowlTime = 0;
            PlayRandomGrowl();
        }

        currentFootTime += Time.deltaTime;
        if (currentFootTime >= nextRandomFootTime)
        {
            nextRandomFootTime = Random.Range(0.5f, 3);
            currentFootTime = 0;
            PlayRandomFootstep();
        }

        

    }

    private void DisableNoises()
    {
        playMonsterSounds = false;
    }

    private void PlayRandomFootstep()
    {
        int rand = (int)Random.Range(0,100)%4;
        switch (rand)
        {
            case 0:
                PlayFootstep1();
                break;
            case 1:
                PlayFootstep1();
                break;
            case 2:
                PlayFootstep1();
                break;
            case 3:
                PlayFootstep1();
                break;
        }
    }

    private void PlayRandomGrowl()
    {
        int rand = (int)Random.Range(0, 100) % 5;
        switch (rand)
        {
            case 0:
                PlayGrowl1();
                break;
            case 1:
                PlayGrowl2();
                break;
            case 2:
                PlayGrowl3();
                break;
            case 3:
                PlayGrowl4();
                break;
            case 4:
                PlayGrowl5();
                break;
        }
    }

    private void PlayFootstep1()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSneaking1", monsterTransform.position);
    }

    private void PlayFootstep2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSneaking2", monsterTransform.position);
    }

    private void PlayFootstep3()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSneaking3", monsterTransform.position);
    }

    private void PlayFootstep4()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSneaking4", monsterTransform.position);
    }

    private void PlayGrowl1()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSound1", monsterTransform.position);
    }

    private void PlayGrowl2()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSound2", monsterTransform.position);
    }

    private void PlayGrowl3()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSound3", monsterTransform.position);
    }

    private void PlayGrowl4()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSound4", monsterTransform.position);
    }

    private void PlayGrowl5()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Monster/MonsterSound5", monsterTransform.position);
    }

}
