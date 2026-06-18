using System;
using System.Collections;
using System.Collections.Generic;

public static class PlayerKnowledgeState 
{
    // Stores a combination of room and time and knowledge type to track what the player has discovered so far
    private static HashSet<(RoomType, int, KnowledgeType)> HourlyDiscovered = new();
    private static HashSet<(RoomType, KnowledgeType)> GenerallyDiscovered = new();
    private static HashSet<(RoomType, CharacterType, KnowledgeType)> CharacterDiscovered = new();

    public static event Action OnKnowledgeUpdated;

    // classes can log their specific information as discovered
    public static void Discover(RoomType room, int time, KnowledgeType type)
    {
        //Debug.Log($"Recorded information about {type} in room {room}!");
        
        HourlyDiscovered.Add((room, time, type));
        OnKnowledgeUpdated.Invoke();
    }

    public static void Discover(RoomType room, KnowledgeType type)
    {
        //Debug.Log($"Recorded information about {type} in room {room}!");
        
        GenerallyDiscovered.Add((room, type));
        OnKnowledgeUpdated.Invoke();
    }

    public static void Discover(RoomType room, CharacterType character, KnowledgeType type)
    {
        //Debug.Log($"Recorded information about {type} in room {room}!");
        
        CharacterDiscovered.Add((room, character, type));
        OnKnowledgeUpdated.Invoke();
    }

    // other classes can query this to figure out what the players knows or not yet
    public static bool IsKnownHourly(RoomType room, int time, KnowledgeType type)
    {
        return HourlyDiscovered.Contains((room, time, type));
    }

    public static bool IsKnownGenerally(RoomType room, KnowledgeType type)
    {
        return GenerallyDiscovered.Contains((room, type));
    }

    public static bool IsKnownCharacterly(RoomType room, CharacterType character, KnowledgeType type)
    {
        return CharacterDiscovered.Contains((room, character, type));
    }

    public static readonly Dictionary<PollutantType, KnowledgeType> PollutantKnowledgeKey = new Dictionary<PollutantType, KnowledgeType>
    {
        [PollutantType.CO2] = KnowledgeType.CO2,
        [PollutantType.O3] = KnowledgeType.O3,
        [PollutantType.NO] = KnowledgeType.NO,
        [PollutantType.VOC] = KnowledgeType.VOC
    };

    public static readonly KnowledgeType[] PollutantKnowledgeTypes =
    {
        KnowledgeType.CO2, KnowledgeType.O3, KnowledgeType.NO, KnowledgeType.VOC
    };
}

public enum KnowledgeType
{
    RoomInfo,
    PollutantPresence, CO2, O3, NO, VOC,
    FanStatus, FurnaceStatus, SpraycanStatus, StoveStatus,
    NPCSymptom, NPCDialogue, 
}
