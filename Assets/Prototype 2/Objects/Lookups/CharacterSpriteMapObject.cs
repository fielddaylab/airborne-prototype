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

public class CharacterLookupUtility
{
    public static Sprite GetSprite(CharacterSpriteMapObject map, CharacterType character)
    {
        foreach (var p in map.Pairs)
        {
            if (p.Character == character)
            {
                return p.CharacterPortrait;
            }
        }
        return null;
    }

    public static string GetBlurb(CharacterSpriteMapObject map, CharacterType character)
    {
        foreach (var p in map.Pairs)
        {
            if (p.Character == character)
            {
                return p.CharacterBlurb;
            }
        }
        return null;
    }
}

[System.Serializable]
public class CharacterSpritePair
{
    public CharacterType Character;
    public Sprite CharacterPortrait;
    public string CharacterBlurb;
}