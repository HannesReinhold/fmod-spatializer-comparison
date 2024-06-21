using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{

    public List<GameObject> trailObjects;

    public int currenttrailObject;

    // Start is called before the first frame update
    void Start()
    {
        //trailObjects[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPart()
    {
        Invoke("ShowPath1", 1);
        Invoke("ShowPath2", 1.5f);
        Invoke("ShowPath3", 2f);
        Invoke("ShowPath4", 2.5f);
        Invoke("ShowPath5", 3f);
    }

    public void SetNextPart()
    {
        currenttrailObject++;
        //trailObjects[currenttrailObject - 1].SetActive(false);
        trailObjects[currenttrailObject].SetActive(true);
        Debug.Log("Show Footstep "+currenttrailObject);
    }

    private void ShowPath1()
    {
        trailObjects[0].SetActive(true);
        Debug.Log("Show Footstep ");
    }
    private void ShowPath2()
    {
        trailObjects[1].SetActive(true);
        Debug.Log("Show Footstep ");
    }

    private void ShowPath3()
    {
        trailObjects[2].SetActive(true);
        Debug.Log("Show Footstep ");
    }

    private void ShowPath4()
    {
        trailObjects[3].SetActive(true);
        Debug.Log("Show Footstep ");
    }

    private void ShowPath5()
    {
        trailObjects[4].SetActive(true);
        Debug.Log("Show Footstep ");
    }
}
