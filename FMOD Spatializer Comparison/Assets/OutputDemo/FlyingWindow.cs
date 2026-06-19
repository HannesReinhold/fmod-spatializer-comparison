using Meta.XR;
using Meta.XR.MRUtilityKit.SceneDecorator;
using UnityEngine;

public class FlyingWindow : MonoBehaviour
{
    public bool alwaysInFront=false;
    public bool alwaysFacing = false;

    public float speed = 1;

    public Transform target;
    public Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        if (alwaysInFront)
        {
            Vector3 forwards2d = Camera.main.transform.forward;
            forwards2d.y = 0.5f;
            transform.position = Vector3.Lerp(transform.position, Camera.main.transform.position + forwards2d.normalized * 0.5f + Vector3.down * 0.5f, 0.05f*speed);
        }
        else if(target!=null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position+target.right*offset.x+target.up*offset.y+target.forward*offset.z, 0.05f*speed);
        }

        if (alwaysFacing)
        {
            Vector3 dir = transform.position - Camera.main.transform.position;
            dir.y = -0.2f;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.05f);
        }else if (target != null)
        {
            Vector3 targetRotEuler = target.transform.eulerAngles;
            Quaternion targetRot = Quaternion.Euler(targetRotEuler.x,targetRotEuler.y+180,targetRotEuler.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
        }

    }
}
