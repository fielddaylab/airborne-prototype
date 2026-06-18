using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Pollutant Knowledge Map")]
public class PollutantKnowledgeMapObject : ScriptableObject
{
    public PollutantKnowledgePair[] Pairs;

    private Dictionary<PollutantType, KnowledgeType> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<PollutantType, KnowledgeType>();
        foreach (var entry in Pairs) {
            _lookup[entry.Pollutant] = entry.Knowledge;
        }
    }

    public KnowledgeType GetKnowledge(PollutantType pollutant)
    {
        _lookup.TryGetValue(pollutant, out KnowledgeType knowledge);
        return knowledge;
    }
}

[System.Serializable]
public class PollutantKnowledgePair
{
    public PollutantType Pollutant;
    public KnowledgeType Knowledge;
}