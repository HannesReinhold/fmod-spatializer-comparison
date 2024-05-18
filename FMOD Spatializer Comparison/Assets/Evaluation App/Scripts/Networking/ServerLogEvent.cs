using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerLogEvent : RealtimeComponent<ServerLogEventModel>
{
    [SerializeField]
    public ServerGUI _serverGUI = default;
    public List<WindowManager> windowManagers;
    private int currentWindowManager = 0;

    // When we connect to a room server, we'll be given an instance of our model to work with.
    protected override void OnRealtimeModelReplaced(ServerLogEventModel previousModel, ServerLogEventModel currentModel)
    {
        if (previousModel != null)
        {
            // Unsubscribe from events on the old model.
            previousModel.eventDidFire -= EventDidFire;
        }
        if (currentModel != null)
        {
            // Subscribe to events on the new model
            currentModel.eventDidFire += EventDidFire;
        }
    }

    // A public method we can use to fire the event
    public void LogAll(int currentWindowManager, string additionalEvent)
    {
        this.currentWindowManager = currentWindowManager;
        int pageNum = windowManagers[currentWindowManager].currentWindowIndex;
        model.FireEvent(realtime.clientID, pageNum, additionalEvent,0);
    }

    public void LogEvent(string additionalEvent)
    {
        int pageNum = windowManagers[currentWindowManager].currentWindowIndex;
        model.FireEvent(realtime.clientID, pageNum, additionalEvent,0);
    }
    public void NextPageEvent(int nextPage)
    {
        int pageNum = windowManagers[currentWindowManager].currentWindowIndex + nextPage;
        if (pageNum >= windowManagers[currentWindowManager].windows.Count)
        {
            pageNum = 2;
        }
        if (pageNum <0)
        {
            pageNum = -2;
        }
        model.FireEvent(realtime.clientID, pageNum, "", nextPage);
    }

    // Called whenever our event fires
    private void EventDidFire(int senderID, int pageNum, string eventLog, int nextPage)
    {
        // Tell the particle system to trigger an explosion in response to the event
        Debug.Log("didFIre: "+pageNum);
        
        if (nextPage != 0) 
        {
            if(nextPage==1) windowManagers[currentWindowManager].NextPage();
            if (nextPage == -1) windowManagers[currentWindowManager].PreviousPage();
            if (nextPage == 2)
            {
                if(currentWindowManager<windowManagers.Count-1)currentWindowManager++;
                windowManagers[currentWindowManager].OpenPage(0);
            }
            if (nextPage == -2)
            {
                if(currentWindowManager>0) currentWindowManager--;
                windowManagers[currentWindowManager].OpenPage(windowManagers[currentWindowManager].windows.Count-1);
            }
            if (nextPage == -999)
            {
                GameManager.Instance.Restart();
                currentWindowManager = 0;
            }
            if (nextPage == -100)
            {
                currentWindowManager = 1;
                GameManager.Instance.SkipIntroduction();
                
            }
        }
        _serverGUI.UpdateLog(pageNum, eventLog, currentWindowManager);
    }

    public void SetWindowManager(int index)
    {
        currentWindowManager = index;
    }

    public void Reset()
    {
        currentWindowManager = 0;
    }
}
