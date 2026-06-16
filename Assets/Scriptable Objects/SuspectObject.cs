using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pollution Level")]
public class SuspectObject : ScriptableObject
{
    public string levelName;
    public RoomType sourceRoom;
    public FeatureType pollutionSource;
    public PollutantType pollutant;
    public Symptom Symptom;
}