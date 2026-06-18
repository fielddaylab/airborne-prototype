using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Pollutant Knowledge Map")]
public class PollutantKnowledgeMapObject : ScriptableObject
{
    public PollutantKnowledgePair[] Pairs;

    private Dictionary<PollutantType, KnowledgeType> _lookup;
    private Dictionary<PollutantType, Sprite> _spriteLookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<PollutantType, KnowledgeType>();
        _spriteLookup = new Dictionary<PollutantType, Sprite>();
        foreach (var entry in Pairs) {
            _lookup[entry.Pollutant] = entry.Knowledge;
        }
        foreach (var entry in Pairs)
        {
            _spriteLookup[entry.Pollutant] = entry.SpriteOverlay;
        }
    }

    public KnowledgeType GetKnowledge(PollutantType pollutant)
    {
        _lookup.TryGetValue(pollutant, out KnowledgeType knowledge);
        return knowledge;
    }

    public Sprite GetSprite(PollutantType pollutant)
    {
        _spriteLookup.TryGetValue(pollutant, out Sprite sprite);
        return sprite;
    }
}

[System.Serializable]
public class PollutantKnowledgePair
{
    public PollutantType Pollutant;
    public KnowledgeType Knowledge;
    public Sprite SpriteOverlay;
}