using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Feature Map")]
public class FeatureMapObject : ScriptableObject
{
    public FeaturePair[] Pairs;
    private Dictionary<FeatureType, KnowledgeType> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<FeatureType, KnowledgeType>();
        
        foreach (var entry in Pairs)
        {
            _lookup[entry.Featre] = entry.KnowledgeType;
        }
    }

    public KnowledgeType GetKnowledgeType(FeatureType feature)
    {
        _lookup.TryGetValue(feature, out KnowledgeType know);
        return know;
    }
}

[System.Serializable]
public class FeaturePair
{
    public FeatureType Featre;
    public KnowledgeType KnowledgeType;
}