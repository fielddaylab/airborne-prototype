using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Scenario Data")]
public class ScenarioDataObject : ScriptableObject
{
    public string ScenarioName;
    public InvestigationRoomObject[] Rooms;
    public InvestigationNPCObject[] NPCs;
    public InvestigationFeatureEventObject[] FeatureEvents;
}
