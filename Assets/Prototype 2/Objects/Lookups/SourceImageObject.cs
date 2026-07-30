using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Source Image")]
public class SourceImageObject : ScriptableObject
{
    public SourceImagePair[] ImagePairs;

    private Dictionary<FeatureType, Sprite> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<FeatureType, Sprite>();
        foreach (var entry in ImagePairs) {
            _lookup[entry.Feature] = entry.FeatureSprite;
        }
    }

    public Sprite GetSprite(FeatureType feature)
    {
        _lookup.TryGetValue(feature, out Sprite sprite);
        return sprite;
    }
}

[System.Serializable]
public class SourceImagePair
{
    public FeatureType Feature;
    public Sprite FeatureSprite;
}