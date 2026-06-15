using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerKnowledgeState 
{
    // Stores a combination of room and time and knowledge type to track what the player has discovered so far
    public static HashSet<(RoomType, int, KnowledgeType)> Discovered = new();

    // classes can log their specific information as discovered
    public static void Discover(RoomType room, int time, KnowledgeType type)
    {
        Discovered.Add((room, time, type));
    }

    // other classes can query this to figure out what the players knows or not yet
    public static bool IsKnown(RoomType room, int time, KnowledgeType type)
    {
        return Discovered.Contains((room, time, type));
    }
}

public enum KnowledgeType
{
    PollutantPresence, MeterInfo
}
