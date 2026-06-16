using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Timeline Object")]
public class TimelineObject : ScriptableObject
{
    public TimelineStep[] timeline;
}

[System.Serializable]
public class TimelineStep
{
    public int hourTime;
    public RoomStep[] roomSteps;
}

[System.Serializable]
public class RoomStep
{
    public RoomType roomType;
    public PollutantStep[] pollutantSteps;
    public CharacterStep[] characterSteps;
    public SourceStep[] sourceSteps;
}

[System.Serializable]
public class SourceStep
{
    public FeatureType pollutionSource;
    public FeatureEvent sourceAction;
}

[System.Serializable]
public class PollutantStep
{
    public PollutantType pollutantType;
    public int concentration;
}

[System.Serializable]
public class CharacterStep
{
    public CharacterType character;
    public string dialogue = "";
    public Symptom observedSymptom;
}
