using System.Numerics;
using UnityEngine;

public class SphereFollow : MonoBehaviour
{
    void Update()
    {
        UnityEngine.Vector3 targetPos = Camera.main.transform.position;
        targetPos.y=0.3f;
        transform.position = UnityEngine.Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }
}
