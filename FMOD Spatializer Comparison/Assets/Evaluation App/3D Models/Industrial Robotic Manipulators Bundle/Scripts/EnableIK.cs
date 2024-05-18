
using UnityEngine;

public class EnableIK
{
    [SerializeField] private bool IKEnabled;

    // Start is called before the first frame update
    private void Start()
    {
        //NetworkManager.Singleton.OnServerStarted += EnableIK_OnServerStarted;
    }

    private void Update()
    {
        /*
        if (IKEnabled) return;
        if (NetworkManager.Singleton.IsServer)
        {
            GetComponent<UltimateIK>().enabled = true;

            IKEnabled = true;
        }
        */
    }


    private void EnableIK_OnServerStarted()
    {
        /*
        if (NetworkManager.Singleton.IsServer)
        {
            GetComponent<UltimateIK>().enabled = true;
            IKEnabled = true;
        }
        */
    }
}