using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{

    public string id;
    public Gender sex;
    public int age;
    public bool hearingImpairment;
    public string hearingImparimentDetails;
    public int experienceAudio;
    public int experienceMixedReality;
    public float volume;
    public long dateTime;

    public List<ConcreteSubjectiveEvaluation> subjectiveEvaluationResults;
    public List<ConcreteSubjectiveData> subjectiveData;
    public List<ConcretePositionGuessingData> positionGuessData;
    public List<DirectionGuessingData> directionGuessingBaseLineResults;
    public List<DirectionGuessingData> directionGuessingResults;

    public SessionData(string id)
    {
        this.id = id;
        subjectiveEvaluationResults = new List<ConcreteSubjectiveEvaluation>();
        subjectiveData = new List<ConcreteSubjectiveData>();
        positionGuessData = new List<ConcretePositionGuessingData>();
        directionGuessingBaseLineResults = new List<DirectionGuessingData>();
        directionGuessingResults = new List<DirectionGuessingData>();
    }

}

[System.Serializable]
public enum Gender
{
    Male,
    Female,
    Diverse
}
