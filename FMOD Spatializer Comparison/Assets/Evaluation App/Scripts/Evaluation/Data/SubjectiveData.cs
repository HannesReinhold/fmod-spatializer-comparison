using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConcreteSubjectiveData
{
    public int evaluationID;
    public int spatializerA;
    public int spatializerB;
    public string attribute;
    public string description;
    public int rating;
}

[System.Serializable]
public class SubjectiveData
{
    public List<ConcreteSubjectiveData> comparisons;
}