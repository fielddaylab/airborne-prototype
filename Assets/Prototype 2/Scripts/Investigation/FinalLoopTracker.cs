using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLoopTracker : MonoBehaviour
{
    public bool ReplacedFurnace = false;
    public bool ReplacedStove = false;
    
    public List<RoomType> PlacedFans = new();
    public RoomType FilterPlacement = RoomType.None;
    public RoomType CleanerPlacement = RoomType.None;

    public FeatureType PosterFeature = FeatureType.None;
    public PollutantType PosterSubjectPollutant = PollutantType.None;
    public PollutantType PosterMessagePollutant = PollutantType.None;
}