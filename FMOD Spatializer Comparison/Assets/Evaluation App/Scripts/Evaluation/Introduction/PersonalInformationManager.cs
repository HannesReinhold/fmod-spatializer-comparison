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
