using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Scenario Data")]
public class ScenarioDataObject : ScriptableObject
{
    public string ScenarioName;
    public CharacterType MainNpc;
    public GameObject WorldEnvironment;
    public GameObject MapObject;
    public InvestigationRoomObject[] Rooms;
    public InvestigationNPCObject[] NPCs;
    public InvestigationFeatureEventObject[] FeatureEvents;
    public PollutantDataObject[] SuspectedPollutants;

    public TargetWinConditions WinConditions;
}

public static class ScenarioUtility
{
    public static RoomType GetRoom(FeatureType feature, ScenarioDataObject scenarioData)
    {
        foreach (var feat in scenarioData.FeatureEvents)
        {
            if (feat.FeatureType == feature)
            {
                return feat.RoomType;
            }
        }
        Debug.LogError("No feature found in scenario data matching type.");
        return RoomType.Kitchen;
    }
}

[System.Serializable]
public class TargetWinConditions
{
    public int TargetLoopCount = 10;
    
    public bool ReplaceFurnace = false;
    public bool ReplaceStove = false;
    
    public List<RoomType> PlacedFansInRooms = new();
    public RoomType PlaceFilterInRoom = RoomType.None;
    public RoomType PlaceCleanerInRoom = RoomType.None;

    public FeatureType PosterFeature = FeatureType.None;
    public PollutantType PosterSubjectPollutant = PollutantType.None;
    public PollutantType PosterMessagePollutant = PollutantType.None;
}