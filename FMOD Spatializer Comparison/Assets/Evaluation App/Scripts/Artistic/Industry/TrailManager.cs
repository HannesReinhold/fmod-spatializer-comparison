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
        trailObjects[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextPart()
    {
        currenttrailObject++;
        trailObjects[currenttrailObject - 1].SetActive(false);
        trailObjects[currenttrailObject].SetActive(true);
    }
}
