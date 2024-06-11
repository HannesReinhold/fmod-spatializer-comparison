using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionDifferenceTest : MonoBehaviour
{
    public Transform target;
    public Transform guess;
    public Transform head;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // calculate azimuth and elevation error
        Vector3 headPosition = Vector3.zero;

        Vector3 actualDirection = (target.transform.position - head.position).normalized;

        // azimuth
        Vector3 horizontalProjGuessed = Vector3.ProjectOnPlane(guess.position, Vector3.up).normalized;
        Vector3 horizontalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.up).normalized;

        float azimuthGuess = -Vector3.SignedAngle(horizontalProjGuessed, Vector3.forward, Vector3.up);
        float azimuthActual = -Vector3.SignedAngle(horizontalProjActual, Vector3.forward, Vector3.up);
        float azimuthDif = -Vector3.SignedAngle(horizontalProjActual, horizontalProjGuessed, Vector3.up);

        Vector3 verticalProjGuessed = Vector3.ProjectOnPlane(guess.position, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        Vector3 verticalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.Cross(horizontalProjActual, Vector3.up));

        float evlevationGuess = -Vector3.SignedAngle(verticalProjGuessed, horizontalProjGuessed, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        float evlevationActual = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up));
        float evlevationDif = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up)) - evlevationGuess;

        //Debug.Log("Az Guess: "+ azimuthGuess + ", Az Target: "+azimuthActual + ", Az Dif: "+azimuthDif);
        Debug.Log("El Guess: " + evlevationGuess + ", El Target: " + evlevationActual + ", El Dif: " + evlevationDif);
    }
}
