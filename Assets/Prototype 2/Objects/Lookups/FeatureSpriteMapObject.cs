using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Source Image")]
public class FeatureSpriteMapObject : ScriptableObject
{
    public FeatureSpriteMap[] FeatureSprites;
}

[System.Serializable]
public class FeatureSpriteMap
{
    public FeatureType Feature;
    public Sprite OnSprite, OffSprite, UnknownSprite;
}

public static class FeatureSpriteMapUtility
{
    public static Sprite GetOnSprite(FeatureSpriteMapObject map, FeatureType feature)
    {
        foreach (var spriteMap in map.FeatureSprites)
        {
            if (spriteMap.Feature == feature)
            {
                return spriteMap.OnSprite;
            }
        }

        return null;
    }

    public static Sprite GetOffSprite(FeatureSpriteMapObject map, FeatureType feature)
    {
        foreach (var spriteMap in map.FeatureSprites)
        {
            if (spriteMap.Feature == feature)
            {
                return spriteMap.OffSprite;
            }
        }

        return null;
    }

    public static Sprite GetUnkownSprite(FeatureSpriteMapObject map, FeatureType feature)
    {
        foreach (var spriteMap in map.FeatureSprites)
        {
            if (spriteMap.Feature == feature)
            {
                return spriteMap.UnknownSprite;
            }
        }

        return null;
    }
}