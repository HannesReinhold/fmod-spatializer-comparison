using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAudio : MonoBehaviour
{


    public Transform spatializer;

    private FollowTarget target;

    public List<Transform> points;


    // Start is called before the first frame update
    void Start()
    {
        target = FindAnyObjectByType<FollowTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        float minDist = 1000;
        Vector3 minPos = new Vector3();

        for(int i=0; i<points.Count-1; i++)
        {
            Vector3 pos = NearestPointOnLine(points[i].position, points[i+1].position - points[i].position, target.transform.position);
            if(Vector3.Distance(pos, target.transform.position)<minDist)
            {
                minDist = Vector3.Distance(pos, target.transform.position);
                minPos = pos;
            }
        }

        //Vector3 pos = NearestPointOnLine(start.position, end.position- start.position, target.transform.position);
        spatializer.position = minPos;
    }

    public Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        float dist = lineDir.magnitude;
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);

        d = Mathf.Clamp(d,0,dist);
        return linePnt + lineDir * d;
    }
}
