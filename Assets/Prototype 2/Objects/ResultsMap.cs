using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Results Map")]
public class ResultsMap : ScriptableObject
{
    public BackgroundSet[] BackgroundSets;
    public SubjectSet[] SubjectSets;
    public MessageSet[] MessageSets;
}

[System.Serializable]
public class BackgroundSet
{
    public Sprite BackgroundSprite;
    public string Label;
    public FeatureType RelevantPolluter;
}

[System.Serializable]
public class SubjectSet
{
    public Sprite SubjectSprite;
    public string Label;
    public PollutantType RelevantPollutant;
}

[System.Serializable]
public class MessageSet
{
    public string Label;
    public PollutantType RelevantPollutant;
}

