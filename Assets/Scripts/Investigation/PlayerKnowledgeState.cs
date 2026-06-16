using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerKnowledgeState 
{
    // Stores a combination of room and time and knowledge type to track what the player has discovered so far
    private static HashSet<(RoomType, int, KnowledgeType)> Discovered = new();

    public static event Action OnKnowledgeUpdated;

    // classes can log their specific information as discovered
    public static void Discover(RoomType room, int time, KnowledgeType type)
    {
        Discovered.Add((room, time, type));
        OnKnowledgeUpdated.Invoke();
    }

    // other classes can query this to figure out what the players knows or not yet
    public static bool IsKnown(RoomType room, int time, KnowledgeType type)
    {
        return Discovered.Contains((room, time, type));
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
    PollutantPresence, CO2, O3, NO, VOC
}
