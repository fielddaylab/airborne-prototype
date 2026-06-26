using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerKnowledgeState 
{
    // Stores a combination of room and time and knowledge type to track what the player has discovered so far
    private static HashSet<(RoomType, int, KnowledgeType)> HourlyDiscovered = new();
    private static HashSet<(RoomType, KnowledgeType)> GenerallyDiscovered = new();
    private static HashSet<string> IDDiscovered = new();

    private static HashSet<Symptom> SeenSymptoms = new();
    private static HashSet<FeatureType> SeenFeatures = new();

    public static event Action OnKnowledgeUpdated;

    // classes can log their specific information as discovered
    public static void Discover(RoomType room, int time, KnowledgeType type)
    {
        Debug.Log($"Recorded information about {type} in room {room}!");
        
        HourlyDiscovered.Add((room, time, type));
        OnKnowledgeUpdated.Invoke();
    }

    public static void Discover(RoomType room, KnowledgeType type)
    {
        Debug.Log($"Recorded information about {type} in room {room}!");
        
        GenerallyDiscovered.Add((room, type));
        OnKnowledgeUpdated.Invoke();
    }

    public static void Discover(string id)
    {
        IDDiscovered.Add(id);
        OnKnowledgeUpdated.Invoke();
    }

    public static void Discover(Symptom symptom)
    {
        SeenSymptoms.Add(symptom);
    } 

    public static void Discover(FeatureType feature)
    {
        SeenFeatures.Add(feature);
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

    public static bool IsKnownID(string id)
    {
        return IDDiscovered.Contains(id);
    }

    public static bool HasSeenSymptom(Symptom symptom)
    {
        return SeenSymptoms.Contains(symptom);
    }

    public static bool HasSeenFeature(FeatureType feature)
    {
        return SeenFeatures.Contains(feature);
    }

    public static readonly Dictionary<PollutantType, KnowledgeType> PollutantKnowledgeKey = new Dictionary<PollutantType, KnowledgeType>
    {
        [PollutantType.CO] = KnowledgeType.CO,
        [PollutantType.O3] = KnowledgeType.O3,
        [PollutantType.NOx] = KnowledgeType.NO,
        [PollutantType.VOC] = KnowledgeType.VOC
    };

    public static readonly KnowledgeType[] PollutantKnowledgeTypes =
    {
        KnowledgeType.CO, KnowledgeType.O3, KnowledgeType.NO, KnowledgeType.VOC
    };
}

public enum KnowledgeType
{
    RoomInfo,
    PollutantPresence, CO, O3, NO, VOC,
    FanStatus, FurnaceStatus, SpraycanStatus, StoveStatus, ElectricStatus,
    NPCPresence, NPCSymptom, NPCDialogue, 
}
