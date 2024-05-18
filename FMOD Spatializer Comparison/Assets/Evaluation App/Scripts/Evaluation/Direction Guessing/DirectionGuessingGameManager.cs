using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionGuessingGameManager : MonoBehaviour
{
    public GameObject game;

    public GameObject tutorialGame;

    public List<GameObject> sciptedEvents;

    public WindowManager windowManager;

    private void OnEnable()
    {
        //OpenIntroduction();
        
    }

    private void Start()
    {
        ResetEvents();
    }

    private void ResetAll()
    {
        //introduction.SetActive(false);
        //tutorial.SetActive(false);
        //game.SetActive(false);
    }



    public void OpenIntroduction()
    {
        ResetAll();
        //introduction.SetActive(true);
        GUIAudioManager.SetAmbientVolume(0.5f);
        GameManager.Instance.LogServerEvent("Direction Game Introduction");
    }

    public void OpenTutorial()
    {
        ResetAll();
        tutorialGame.SetActive(true);
        GameManager.Instance.LogServerEvent("Direction Game Tutorial");
    }

    public void OpenGame()
    {
        ResetAll();
        game.SetActive(true);
        game.GetComponentInChildren<DirectionGuessingGame>().OnStartClick();
        GameManager.Instance.LogServerEvent("Direction Game Round");
    }

    public void OpenComplete()
    {
        ResetAll();
        windowManager.ResetSlow();
        Invoke("STartComplete",2);
    }

    private void StartComplete()
    {
        GameManager.Instance.StartComplete();
    }

    public void ResetEvents()
    {
        for (int i = 0; i < sciptedEvents.Count; i++)
        {
            sciptedEvents[i].SetActive(false);
        }
    }

    public void OpenEvent(int index)
    {
        for(int i=0; i<sciptedEvents.Count; i++)
        {
            sciptedEvents[i].SetActive(index == i);
        }
    }

    public void CloseEvent(int index)
    {
        sciptedEvents[index].SetActive(false);
    }

    public void StartTutorialGame()
    {
        tutorialGame.SetActive(true);
    }

    public void EndTutorialGame()
    {
        tutorialGame.SetActive(false);
    }

    public void OnCompleteClick()
    {
        windowManager.ResetSlow();
        Invoke("StartComplete", 2);
    }
}
