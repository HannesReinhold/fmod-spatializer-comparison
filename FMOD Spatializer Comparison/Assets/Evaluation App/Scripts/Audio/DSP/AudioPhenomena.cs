using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPhenomena : MonoBehaviour
{
    [System.Serializable]
    public struct headVolume
    {
        public float forward;
        public float backward;
        public float left;
        public float right;
        public float up;
        public float down;
    }

    public ParticleSystem particles;
    public Transform speakerModel;
    public Light light;


    [Header("Source Properties")]
    public float loudnessDb = 70;

    [Header("HRTF")]
    public float earDelayDifference;
    public headVolume earVolumeDifference;
    public float earFilterDifference;



    public bool enableMono;
    public bool enableStereo;
    public bool enableITD;
    public bool enableIID;
    public bool enableAttenuation;
    public bool enableOcclusion;
    public bool enableDoppler;
    public bool enableEarlyReflections;
    public bool enableReverb;

    public Transform trans;

    public MetaXRAudioSource spat;


    private AudioSource source; 


    private VariableDelay itdDelayLeft = new VariableDelay(1000);
    private VariableDelay itdDelayRight = new VariableDelay(1000);

    private FirstOrderLowpass iidFilterLeft = new FirstOrderLowpass();
    private FirstOrderLowpass iidFilterRight = new FirstOrderLowpass();

    private FirstOrderLowpass airAttenuationFilterLeft = new FirstOrderLowpass();
    private FirstOrderLowpass airAttenuationFilterRight = new FirstOrderLowpass();

    private FirstOrderLowpass occlusionFilterLeft = new FirstOrderLowpass();
    private FirstOrderLowpass occlusionFilterRight = new FirstOrderLowpass();

    private EarlyReflections earlyRefflections = new EarlyReflections(8);
    private RealisticReverb reverb = new RealisticReverb(8, 4);

    // Audio parameters

    private float volumeLeft;
    private float volumeRight;

    private float filterCutoffLeft;
    private float filterCutoffRight;

    private float delayLeft;
    private float delayRight;

    private float airAttenuation = 1;
    private float airAttenuationVolume = 1;
    private float airAttenuationFilterCutoff = 1;

    private float occlusionVolume = 1;
    private float occlusionFilterCutoff = 1;



    [Range(0, 1)] public float dry = 1;
    [Range(0, 1)] public float wet = 1;

    [Range(0, 1)] public float feedback = 0.5f;
    [Range(0, 1)] public float feedbackCutoff = 0.5f;

    [Range(0, 50)] public float diffDelaysMin = 20, diffDelaysMax = 40;
    [Range(0, 1000)] public float revDelaysMin = 12, revDelaysMax = 234;

    [Range(0, 1)] public float diffuserOutputCutoff = 0.9f;

    [Range(0, 4)] public int numDiffusionStages = 1;

    public bool enableDiffusion = true;


    private float currentVolume = 0;
    private float targetVolume = 1;

    private float smoothedOcc = 0;


    private void Awake()
    {
        source = GetComponent<AudioSource>();
        
    }


    // Start is called before the first frame update
    void Start()
    {
        if (trans == null) trans = FindObjectOfType<FollowTarget>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosition = trans.InverseTransformDirection((transform.position - trans.position).normalized);

        float sourceDistance = Vector3.Distance(trans.position, transform.position);
        float near = 1 - Sigmoid(sourceDistance, -2, 2);
        float nearExp = Mathf.Lerp(0.9f, 1.25f, near);

        // calc hrtf
        float dotLeft = Mathf.Pow((Vector3.Dot(-Vector3.right, relativePosition) + 1) * 0.5f, nearExp);
        float dotRight = Mathf.Pow((Vector3.Dot(Vector3.right, relativePosition) + 1) * 0.5f, nearExp);
        float dotForward = Mathf.Pow((Vector3.Dot(Vector3.forward, relativePosition) + 1) * 0.5f, nearExp);
        float dotUpward = Mathf.Pow((Vector3.Dot(Vector3.up, relativePosition) + 1) * 0.5f, nearExp);

        volumeLeft = (Mathf.Lerp(Mathf.Lerp(0.3f, 0.15f, near), 1, dotLeft) * 0.6f + dotForward * 0.3f + dotUpward * 0.1f) * 1.3f;
        volumeRight = (Mathf.Lerp(Mathf.Lerp(0.3f, 0.15f, near), 1, dotRight) * 0.6f + dotForward * 0.3f + dotUpward * 0.1f) * 1.3f;

        filterCutoffLeft = Mathf.Lerp(0.4f, 1, volumeLeft);
        filterCutoffRight = Mathf.Lerp(0.4f, 1, volumeRight);

        delayLeft = (Vector3.Dot(Vector3.right, relativePosition) + 1);
        delayRight = (Vector3.Dot(-Vector3.right, relativePosition) + 1);
        itdDelayLeft.SetDelay(delayLeft* earDelayDifference);
        itdDelayRight.SetDelay(delayRight* earDelayDifference);

        // calc air attenuation
        airAttenuation = calculateAirAttenuation(sourceDistance);
        airAttenuationVolume = Mathf.Pow(airAttenuation, 0.9f);
        airAttenuationFilterCutoff = Mathf.Lerp(0.7f, 1, Mathf.Pow(airAttenuation, 0.5f));



        // calc occlusion
        float occ = calcOcclusion();
        smoothedOcc = Mathf.Lerp(smoothedOcc, occ, Time.deltaTime*4f);
        //float occ = 0;
        occlusionVolume = smoothedOcc;
        occlusionFilterCutoff = Mathf.Lerp(0.01f, 0.99f, smoothedOcc);

        // calc reverb parameters
        CalcEarlyReflections();


        reverb.dry = dry;
        reverb.wet = wet;

        reverb.delaynetwork.setFeedback(feedback);
        reverb.delaynetwork.setCutoffFrequencies(feedbackCutoff);
        reverb.diffuser.setDelays(diffDelaysMin, diffDelaysMax);
        reverb.delaynetwork.setDelays(revDelaysMin, revDelaysMax);

        reverb.enableReverb = enableReverb;
        reverb.diffuser.SetOtputCutoffFrequency(diffuserOutputCutoff);

        reverb.diffuser.numStages = numDiffusionStages;



        Debug.Log("Volume:[L: " + volumeLeft + ", R: " + volumeRight + "], " +
                  "Filter:[L: " + filterCutoffLeft + ", R: " + filterCutoffRight + "]" +
                  "Delay:[L: " + delayLeft + ", R: " + delayRight + "]" +
                  "Near Field: " + near + "]" +
                  "Attenuation:[ " + airAttenuation + "]" +
                  "Occlusion:[ " + occlusionVolume + "]"
                  );


        iidFilterLeft.cutoffFrequency = filterCutoffLeft;
        iidFilterRight.cutoffFrequency = filterCutoffRight;

        airAttenuationFilterLeft.cutoffFrequency = airAttenuationFilterCutoff;
        airAttenuationFilterRight.cutoffFrequency = airAttenuationFilterCutoff;

        occlusionFilterLeft.cutoffFrequency = occlusionFilterCutoff;
        occlusionFilterRight.cutoffFrequency= occlusionFilterCutoff;


        //var emission = particles.emission;
        //emission.rateOverTimeMultiplier = rms*25;
        if(rms>0.3f && !playParticles)
        {
            playParticles = true;
            particles.Play();
        }
        if (rms < 0.3f)
        {
            playParticles = false;
        }

        speakerModel.localScale = Vector3.one+Vector3.one*rms*0.2f;
        light.intensity = rms * 0.1f;
    }

    bool playParticles = false;

    void CalcEarlyReflections()
    {
        int n = 0;

        RaycastHit[] hits = new RaycastHit[8];
        

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < 8; i++)
        {
            float t = (float)i / 8;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);


            Vector3 dir = new Vector3(x, y, z);
            hits[i] = new RaycastHit();

            if (Physics.Raycast(transform.position + dir * 0.02f, dir, out hits[i], Mathf.Infinity))
            {
                
            }
            Debug.DrawLine(transform.position, transform.position + dir*hits[i].distance);
        }

        float[] delayTimes = new float[8];
        float[] delayPanning = new float[8];
        float[] delayStrength = new float[8];

        for (int i=0; i<8; i++)
        {
            delayTimes[i] = (hits[i].distance + Vector3.Distance(trans.position,hits[i].point)) *1000f/343f;
            delayStrength[i] = Mathf.Pow(1f / ((hits[i].distance * 0.2f + Vector3.Distance(trans.position, hits[i].point) * 0.35f + 1)),2f);
            
            delayPanning[i] = Vector3.Dot(trans.right, (hits[i].point-trans.position).normalized) * 0.5f + 0.5f;
            Debug.Log("DIst: " + delayTimes[i] + " STR: " + delayStrength[i]+ " PAN: "+delayPanning[i]);
        }

            earlyRefflections.SetDelays(delayTimes, delayPanning, delayStrength);

    }


    float calcOcclusion()
    {
        float occlusion = 0;
        int n = 0;
        int layerMask = 1 << 8;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (trans.position - transform.position).normalized, out hit, Vector3.Distance(trans.position,transform.position), layerMask))
        {
            return 0.4f;
        }
        return 1;
        /*
            float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < 50; i++)
        {
            float t = (float)i / 50;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination);
            float y = 0;
            float z = Mathf.Cos(inclination);
            ;


            Vector3 dir = new Vector3(x, y, z);
            //Vector3 dir = Random.onUnitSphere;
            RaycastHit hit = new RaycastHit();
            float att = 2;
            Vector3 lastHit = Vector3.zero;

            for (int j = 0; j < 2; j++)
            {
                lastHit = hit.point;
                if (Physics.Raycast(transform.position + dir * 0.02f, dir, out hit, Mathf.Infinity))
                {
                    //Debug.DrawLine(transform.position, hit.point);
                    RaycastHit hit2;
                    att *= 0.5f;
                    dir = Vector3.Reflect(dir, hit.normal);
                    if (Physics.Raycast(hit.point - (trans.position - hit.point).normalized * 0.02f, (trans.position - hit.point).normalized, out hit2, Vector3.Distance(trans.position, hit.point)))
                    {
                        att = 0;
                        Debug.DrawLine(hit.point, hit2.point);
                    }

                    n++;
                    occlusion += att;
                }
                Debug.DrawLine(lastHit, hit.point);

            }
        }

        return Mathf.Pow(occlusion / n, 0.5f);
        */
    }

    Vector3[] preferredDirections = new Vector3[5];


    float calcOcclusion2()
    {
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < 100; i++)
        {
            float t = (float)i / 100;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);


            Vector3 dir = new Vector3(x,y,z);
            dir += preferredDirections[0] * 0.5f;
            dir.Normalize();

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(transform.position - dir * 0.02f, dir, out hit, Mathf.Infinity))
            {
                //Debug.DrawLine(transform.position, hit.point);
                RaycastHit hit2;


                if (Physics.Raycast(hit.point - (trans.position - hit.point).normalized * 0.05f, (trans.position - hit.point).normalized, out hit2, Vector3.Distance(trans.position, hit.point)))
                {

                }
                else
                {
                    Debug.DrawLine(hit.point, hit2.point);
                    preferredDirections[0] += dir * 0.3f;
                    preferredDirections[0].Normalize();
                }
            }
        }

        Debug.DrawLine(transform.position, transform.position + preferredDirections[0]);

        return 0.4f;
    }




    private float Sigmoid(float x, float offset, float size)
    {
        return 1f / (1 + Mathf.Exp(-size * (x + offset)));
    }

    private float calculateAirAttenuation(float distance)
    {
        float att = DbToLin(loudnessDb - 70) / Mathf.Pow((distance*0.3f) + 1, 1.1f);

        return att;
    }

    private float DbToLin(float x)
    {
        return Mathf.Pow(10, x / 20);
    }



    public float sum = 0;
    private float rms = 0;


    private void OnAudioFilterRead(float[] data, int channels)
    {
        sum = 0;
        for(int i=0; i<data.Length; i += channels)
        {
            sum += Mathf.Max(Mathf.Abs(data[i]), Mathf.Abs(data[i+1]));
        }
        rms = Mathf.Sqrt(sum/(data.Length/2));




        for (int i = 0; i < data.Length; i += channels)
        {

            currentVolume = 0.99997f * currentVolume + 0.00003f * targetVolume;

            float monoValue = (data[i] + data[i + 1]) * 0.5f;

            if (enableMono)
            {
                data[i] = monoValue * currentVolume;
                data[i + 1] = monoValue * currentVolume;
            }

            if (enableStereo)
            {
                data[i] = data[i] * currentVolume;
                data[i + 1] = data[i + 1] * currentVolume;
            }

            // ITD
            if (enableITD)
            {
                data[i] = itdDelayLeft.Process(data[i]);
                data[i + 1] = itdDelayRight.Process(data[i + 1]);
            }



            // IID
            if (enableIID)
            {
                data[i] = iidFilterLeft.Process(data[i]) * volumeLeft;
                data[i + 1] = iidFilterRight.Process(data[i + 1]) * volumeRight;
            }



            // Attenuation
            if (enableAttenuation)
            {
                data[i] = airAttenuationFilterLeft.Process(data[i]) * airAttenuationVolume;
                data[i + 1] = airAttenuationFilterRight.Process(data[i + 1]) * airAttenuationVolume;
            }



            // Occlusion
            if (enableOcclusion)
            {
                data[i] = occlusionFilterLeft.Process(data[i]) * occlusionVolume;
                data[i + 1] = occlusionFilterRight.Process(data[i + 1]) * occlusionVolume;
            }


            

        }

        if (enableEarlyReflections)
        {
            earlyRefflections.Process(data);
        }

        // Reverb
        if (enableReverb)
        {
            reverb.Process(data);
        }


    }

    public void EnableMono()
    {
        enableMono = true;
        spat.EnableSpatialization = false;
        source.volume = 0.5f;
    }

    public void EnableStereo()
    {
        enableMono = false;
        enableStereo = true;
    }

    public void EnableITD()
    {
        enableStereo = false;
        enableMono = true;
        enableITD = true;
        earDelayDifference = 1f;
    }

    public void EnableIID()
    {
        enableIID = true;
        earDelayDifference = 1f;
    }

    public void EnableHRTF()
    {
        spat.EnableSpatialization = true;
        enableMono = false;
        enableStereo = true;
        enableIID = false;
        enableITD = false;
        source.spatialize = true;
    }

    public void EnableAttenuation()
    {
        enableAttenuation = true;
        GameManager.Instance.roomModel.SetActive(true);
        source.volume = 1f;
    }

    public void EnableOcclusion()
    {
        enableOcclusion = true;
    }

    public void EnableDoppler()
    {
        enableDoppler = true;
    }

    public void EnableEarly()
    {
        enableEarlyReflections = true;
    }

    public void EnableReverb()
    {
        enableReverb = true;
    }

    public void Mute()
    {
        targetVolume = 0;
        Invoke("Disable",2);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
        GameManager.Instance.roomModel.SetActive(false);
    }

}
