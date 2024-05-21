using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPatializerFileManager : MonoBehaviour
{
    public List<SpatializedEvents> oneShots;
}


[System.Serializable]
public struct SpatializedEvents
{
    public List<FMODUnity.EventReference> eventRefs;

}