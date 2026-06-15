using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Room Object")]
public class InvestigationRoomObject : ScriptableObject
{
    public RoomType RoomType;
    public TimeSlot[] TimeSlots;
}

[System.Serializable]
public class TimeSlot
{
    public int Time;
    public PollutantReading[] PollutantReadings;
    public FeatureEvent[] FeatureEvents;
    public NPCEvent[] NPCEvents;
}

[System.Serializable]
public class PollutantReading
{
    public PollutantType Pollutant;
    public int Concentration;
}

[System.Serializable]
public class FeatureEvent
{
    
}

[System.Serializable]
public class NPCEvent
{
    
}
