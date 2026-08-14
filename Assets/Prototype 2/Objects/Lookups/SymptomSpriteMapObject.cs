using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Symptom Sprite Map")]
public class SymptomSpriteMapObject : ScriptableObject
{
    public SymptomSpritePair[] SpritePairs;

    private Dictionary<Symptom, Sprite> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<Symptom, Sprite>();
        foreach (var entry in SpritePairs) {
            _lookup[entry.SymptomType] = entry.SymptomSprite;
        }
    }

    public Sprite GetSprite(Symptom feature)
    {
        _lookup.TryGetValue(feature, out Sprite sprite);
        return sprite;
    }
}

[System.Serializable]
public class SymptomSpritePair
{
    public Symptom SymptomType;
    public Sprite SymptomSprite;
}