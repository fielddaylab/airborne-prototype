using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pollutant Data Object")]
public class PollutantDataObject : ScriptableObject
{
    public PollutantType Type;
    public Symptom[] Symptoms;
    public FeatureType[] Sources;
}
