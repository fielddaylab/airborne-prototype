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
    private Dictionary<PollutantType, string> _nameLookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<PollutantType, KnowledgeType>();
        _spriteLookup = new Dictionary<PollutantType, Sprite>();
        _materialLookup = new Dictionary<PollutantType, Color>();
        _nameLookup = new();
        foreach (var entry in Pairs) {
            _lookup[entry.Pollutant] = entry.Knowledge;
            _spriteLookup[entry.Pollutant] = entry.SpriteOverlay;
            _materialLookup[entry.Pollutant] = entry.ColorOverlay;
            _nameLookup[entry.Pollutant] = entry.FullName;
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

    public string GetFullName(PollutantType pollutant)
    {
        _nameLookup.TryGetValue(pollutant, out string name);
        return name;
    }
}

[System.Serializable]
public class PollutantKnowledgePair
{
    public PollutantType Pollutant;
    public KnowledgeType Knowledge;
    public Sprite SpriteOverlay;
    public Color ColorOverlay;
    public string FullName;
}