using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PersonalInformationManager : MonoBehaviour
{
    public InputField ageInputField;
    public ToggleGroup sexToggle;
    public Slider volumeSlider;
    public ToggleGroup hearingToggle;

    public ToggleGroup expAudioToggle;
    public ToggleGroup expMRToggle;

    public WindowManager windowManager;


    public void SaveAge()
    {
        GameManager.Instance.dataManager.currentSessionData.age = int.Parse(ageInputField.text);
    }

    public void SaveSex()
    {
        var allToggles = sexToggle.GetComponentsInChildren<Toggle>();
        var activeToggle = sexToggle.ActiveToggles().First();

        Gender sex;
        if (allToggles[0] == activeToggle) sex = Gender.Male;
        else if (allToggles[1] == activeToggle) sex = Gender.Female;
        else sex = Gender.Diverse;

        GameManager.Instance.dataManager.currentSessionData.sex = sex;
    }

    public void SaveHearing()
    {
        var allToggles = hearingToggle.GetComponentsInChildren<Toggle>();
        var activeToggle = hearingToggle.ActiveToggles().First();

        bool hear = false;
        if (allToggles[0] == activeToggle) hear = false;
        else if (allToggles[1] == activeToggle) hear = true;

        GameManager.Instance.dataManager.currentSessionData.hearingImpairment = hear;
    }

    public void SaveExpAudio()
    {
        var allToggles = expAudioToggle.GetComponentsInChildren<Toggle>();
        var activeToggle = expAudioToggle.ActiveToggles().First();

        int exp = 0;
        int index = 0;
        foreach(Toggle t in allToggles)
        {
            if (t == activeToggle) exp = index;
            index++;
        }

        GameManager.Instance.dataManager.currentSessionData.experienceAudio = exp;
    }

    public void SaveExpMR()
    {
        var allToggles = expMRToggle.GetComponentsInChildren<Toggle>();
        var activeToggle = expMRToggle.ActiveToggles().First();

        int exp = 0;
        int index = 0;
        foreach (Toggle t in allToggles)
        {
            if (t == activeToggle) exp = index;
            index++;
        }

        GameManager.Instance.dataManager.currentSessionData.experienceMixedReality = exp;
    }

    public void SaveVolume()
    {
        GameManager.Instance.dataManager.currentSessionData.volume = volumeSlider.value;
    }

    public void NextPage()
    {
        if (GameManager.Instance.dataManager.currentSessionData.hearingImpairment)
        {
            windowManager.NextPage();
            Debug.Log("Yes");
        }
        else
        {
            windowManager.currentWindowIndex += 1;
            windowManager.NextPage();
            Debug.Log("No");
        }
    }
}
