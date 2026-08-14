using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Character Sprite Map")]
public class CharacterSpriteMapObject : ScriptableObject
{
    public CharacterSpritePair[] Pairs;
    private Dictionary<CharacterType, Sprite> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<CharacterType, Sprite>();
        
        foreach (var entry in Pairs)
        {
            _lookup[entry.Character] = entry.CharacterPortrait;
        }
    }

    public Sprite GetSprite(CharacterType character)
    {
        _lookup.TryGetValue(character, out Sprite sprite);
        return sprite;
    }
}

[System.Serializable]
public class CharacterSpritePair
{
    public CharacterType Character;
    public Sprite CharacterPortrait;
}