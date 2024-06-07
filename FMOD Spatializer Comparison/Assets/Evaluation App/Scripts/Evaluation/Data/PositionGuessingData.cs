using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionGuessingData
{
    public List<ConcretePositionGuessingData> positionGuessingRounds;
}


[System.Serializable]
public class ConcretePositionGuessingData
{
    public int roundID;
    public int spatializerID;
    public int stimuliID;
    public Vector3 actualPosition;
    public Vector3 guessedPosition;
    public float totalDifference;
    public float horizontalDifference;
    public float verticalDifference;
    public float timeToGuess;

    public ConcretePositionGuessingData()
    {
        actualPosition = new Vector3();
        guessedPosition = new Vector3();
    }
}