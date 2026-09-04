using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Feature Event")]
public class InvestigationFeatureEventObject : ScriptableObject
{
    public RoomType RoomType;
    public FeatureType FeatureType;
    public FeatureTimeSlot[] TimeSlots;
    public bool isPolluter;
}

[System.Serializable]
public class FeatureTimeSlot
{
    public int Time;
    public FeatureEvent FeatureEvent;
}