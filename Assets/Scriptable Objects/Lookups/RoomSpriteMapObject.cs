using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Lookups/Room Sprite Map")]
public class RoomSpriteMapObject : ScriptableObject
{
    public RoomSpritePair[] Pairs;
    private Dictionary<RoomType, Sprite> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<RoomType, Sprite>();
        
        foreach (var entry in Pairs)
        {
            _lookup[entry.Room] = entry.RoomIcon;
        }
    }

    public Sprite GetSprite(RoomType room)
    {
        _lookup.TryGetValue(room, out Sprite sprite);
        return sprite;
    }
}

[System.Serializable]
public class RoomSpritePair
{
    public RoomType Room;
    public Sprite RoomIcon;
}