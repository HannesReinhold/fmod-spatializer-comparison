using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionEEvaluationTest : MonoBehaviour
{
    public Vector3 dir;
    public Vector3 dir2;

    public Vector3 dirG;

    public float az;
    public float el;

    // Start is called before the first frame update
    void OnValidate()
    {

        Vector3 guessedDirection = dir2.normalized;
        Vector3 actualDirection = dir.normalized;

        // azimuth
        Vector3 horizontalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.up).normalized;
        Vector3 horizontalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.up).normalized;

        float azimuthGuess = -Vector3.SignedAngle(horizontalProjGuessed, Vector3.forward, Vector3.up);
        float azimuthActual = -Vector3.SignedAngle(horizontalProjActual, Vector3.forward, Vector3.up);
        float azimuthDif = -Vector3.SignedAngle(horizontalProjActual, horizontalProjGuessed, Vector3.up);

        Vector3 verticalProjGuessed = Vector3.ProjectOnPlane(guessedDirection, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        Vector3 verticalProjActual = Vector3.ProjectOnPlane(actualDirection, Vector3.Cross(horizontalProjActual, Vector3.up));

        float evlevationGuess = -Vector3.SignedAngle(verticalProjGuessed, horizontalProjGuessed, Vector3.Cross(horizontalProjGuessed, Vector3.up));
        float evlevationActual = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up));
        float evlevationDif = -Vector3.SignedAngle(verticalProjActual, horizontalProjActual, Vector3.Cross(horizontalProjActual, Vector3.up)) - evlevationGuess;
        float azimuth = azimuthDif;
        float elevation = evlevationDif;

        Debug.Log("Guessed: "+guessedDirection+", Actual: "+actualDirection+", az: "+azimuth+", el: "+elevation);

        az = azimuth;
        el = elevation;
        dirG = guessedDirection;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
