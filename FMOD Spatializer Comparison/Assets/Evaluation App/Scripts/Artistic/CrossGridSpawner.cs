using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossGridSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int n = 10;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=-n; i<n; i++)
        {
            for(int j=-n; j<n; j++)
            {
                CrossGrower cross = Instantiate(prefab).GetComponent<CrossGrower>();
                cross.transform.position = transform.position + new Vector3(i * 2, 0, j * 2);
                cross.delay = Vector3.Distance(transform.position, cross.transform.position) * 0.2f;
                cross.StartGrowing();
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
