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
}

[System.Serializable]
public class ObjectStep
{
    public PollutionSource pollutionSource;
    public SourceAction sourceAction;
}

[System.Serializable]
public class PollutantStep
{
    public Pollutant pollutantType;
    public int concentration;
}

[System.Serializable]
public class CharacterStep
{
    public CharacterType character;
    public string dialogue;
    public Symptom observedSymptom;
}
