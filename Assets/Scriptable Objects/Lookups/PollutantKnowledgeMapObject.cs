using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Pollutant Knowledge Map")]
public class PollutantKnowledgeMapObject : ScriptableObject
{
    public PollutantKnowledgePair[] Pairs;

    private Dictionary<PollutantType, KnowledgeType> _lookup;
    private Dictionary<PollutantType, Sprite> _spriteLookup;
    private Dictionary<PollutantType, Color> _materialLookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<PollutantType, KnowledgeType>();
        _spriteLookup = new Dictionary<PollutantType, Sprite>();
        _materialLookup = new Dictionary<PollutantType, Color>();
        foreach (var entry in Pairs) {
            _lookup[entry.Pollutant] = entry.Knowledge;
            _spriteLookup[entry.Pollutant] = entry.SpriteOverlay;
            _materialLookup[entry.Pollutant] = entry.ColorOverlay;
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

    public Color GetMaterial(PollutantType pollutant)
    {
        _materialLookup.TryGetValue(pollutant, out Color mat);
        return mat;
    }
}

[System.Serializable]
public class PollutantKnowledgePair
{
    public PollutantType Pollutant;
    public KnowledgeType Knowledge;
    public Sprite SpriteOverlay;
    public Color ColorOverlay;
}