using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Win Condition Data")]
public class WinConditionDataObject : ScriptableObject
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
