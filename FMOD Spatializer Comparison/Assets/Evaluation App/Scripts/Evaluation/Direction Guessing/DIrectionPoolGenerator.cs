using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIrectionPoolGenerator : MonoBehaviour
{
    public int numDirections = 128;
    public int actualCount = 0;
    public List<Vector3> directions = new List<Vector3>();

    public string output;
    public string output2 = "";

    [Range(0,Mathf.PI)] public float minElevation = 0;
    [Range(0, Mathf.PI)] public float maxElevation = 1;




    void Start()
    {
        GenerateDirections();
        
    }


    void Update()
    {
        GenerateDirections();
    }

    private void OnDrawGizmos()
    {
        GenerateDirections();
        GenerateShuffledIndices();
        actualCount = directions.Count;
        for (int i = 0; i < directions.Count; i++)
        {
            Gizmos.DrawSphere(directions[i], 0.1f);
        }
    }

    void GenerateDirections()
    {
        directions = new List<Vector3>();
        output = "";

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numDirections; i++)
        {
            float t = (float)i / numDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            //inclination = Mathf.Clamp(inclination, minElevation, maxElevation);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            if (inclination >= minElevation && inclination <= maxElevation)
            {
                directions.Add(transform.position + new Vector3(x, z, y) * 2);
                Vector3 p = new Vector3(x, z, y) * 2;
                output += "{\"x\":" + p.x + ",\"y\":"+p.y+",\"z\":"+p.z+"},";
            }
        }
    }

    void GenerateShuffledIndices()
    {
        output2 = "";
        int n = 72;
        int[] pos = new int[n];
        int[] spat = new int[n];
        int[] stim = new int[n];
        System.Random rand = new System.Random();
        for (int i = 0; i < n; i++)
        {
            pos[i] = i + 1;
            spat[i] = i % 3;
            stim[i] = i % 3;
        }
        Shuffle(pos);
        Shuffle(spat);
        Shuffle(stim);

        for (int i = 0; i < n; i++)
        {
            output2 += "{\"roundID\": " + i + ",\"positionID\": " + pos[i] + ",\"spatializerID\": " + spat[i] + ",\"cueID\": " + stim[i] +"},";
        }
        
    }

    void Shuffle(int[] arr)
    {
        System.Random rand = new System.Random();
        for (int i = arr.Length - 1; i >= 1; i--)
        {
            int j = rand.Next(0, i + 1);
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}
