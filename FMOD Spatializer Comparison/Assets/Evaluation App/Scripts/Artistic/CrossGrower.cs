using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossGrower : MonoBehaviour
{
    public List<LineRenderer> lines;

    private float target = 1;
    private float lastTarget = 0;
    private float currentValue=0;
    private float t = 0;

    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        lines[0].SetPosition(0, new Vector3(0, 0, 0));
        lines[0].SetPosition(1, new Vector3(0, 0, 0));
        lines[1].SetPosition(0, new Vector3(0, 0, 0));
        lines[1].SetPosition(1, new Vector3(0, 0, 0));
        lines[0].widthMultiplier = 0;
        lines[1].widthMultiplier = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGrow()
    {
        StartCoroutine(Grow());
    }

    public void StartGrowing()
    {
        Invoke("StartGrow", delay);
    }


    IEnumerator Grow()
    {
        t = 0;

        while (t < 1)
        {
            currentValue = Mathf.Lerp(lastTarget, target, easeInOutQuad(t));
            lines[0].SetPosition(0, new Vector3(-1, 0, 0) * currentValue * 0.2f);
            lines[0].SetPosition(1, new Vector3(1, 0, 0) * currentValue * 0.2f);
            lines[1].SetPosition(0, new Vector3(-1, 0, 0) * currentValue * 0.2f);
            lines[1].SetPosition(1, new Vector3(1, 0, 0) * currentValue * 0.2f);
            lines[0].widthMultiplier = currentValue*0.04f;
            lines[1].widthMultiplier = currentValue*0.04f;
            t += Time.deltaTime/2f;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
    }

    float easeInOutQuad(float x){
        return x< 0.5 ? 2 * x* x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }
}
