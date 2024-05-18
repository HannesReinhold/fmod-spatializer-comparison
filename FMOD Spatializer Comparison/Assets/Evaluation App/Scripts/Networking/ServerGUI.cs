using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerGUI : MonoBehaviour
{
    public TextMeshProUGUI pageNumText;
    public TextMeshProUGUI windowManagerNumText;
    public TextMeshProUGUI eventLogText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLog(int pageNum, string eventLog, int windowManagerNum)
    {

        pageNumText.text = pageNum.ToString();
        windowManagerNumText.text = windowManagerNum.ToString();
        if(eventLog!="") eventLogText.text = eventLog;
        Debug.Log("Update Log");
    }

    public void NextPage(int next)
    {
        GameManager.Instance.NextPageEvent(next);
    }

    public void Restart()
    {
        GameManager.Instance.RestartEvent();
    }

    public void SkipIntroduction()
    {
        GameManager.Instance.SkipIntroduction();
    }
}
