using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Equipment Map")]
public class EquipmentMapObject : ScriptableObject
{
    public EquipmentSet[] Sets;
}

[System.Serializable]
public class EquipmentSet
{
    public EquipmentType Type;
    public Sprite Sprite;
    public string Label;
    public string Description;
    public bool UsesPips;
    public int NumPips;
}

// after learning a bit from spacefab and ais, it seems this is a better way to handle things than holding lookup tables in the object itself
public static class EquipmentMapUtility
{
    public static Sprite GetSprite(EquipmentMapObject map, EquipmentType type)
    {
        foreach (var set in map.Sets)
        {
            if (set.Type == type)
            {
                return set.Sprite;
            }
        }

        return null;
    }

    public static string GetLabel(EquipmentMapObject map, EquipmentType type)
    {
        foreach (var set in map.Sets)
        {
            if (set.Type == type)
            {
                return set.Label;
            }
        }

        return null;
    }

    public static string GetDescription(EquipmentMapObject map, EquipmentType type)
    {
        foreach (var set in map.Sets)
        {
            if (set.Type == type)
            {
                return set.Description;
            }
        }

        return null;
    }

    public static bool UsesPips(EquipmentMapObject map, EquipmentType type)
    {
        foreach (var set in map.Sets)
        {
            if (set.Type == type)
            {
                return set.UsesPips;
            }
        }

        return false;
    }

    public static int GetNumPips(EquipmentMapObject map, EquipmentType type)
    {
        foreach (var set in map.Sets)
        {
            if (set.Type == type)
            {
                return set.NumPips;
            }
        }

        return -1;
    }
}