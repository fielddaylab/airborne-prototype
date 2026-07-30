using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public InvestigationNPCObject NPCData;
    public NavMeshAgent NavAgent;
    public NPCRoomWaypointEntry[] Waypoints;

    public SpriteRenderer SymptomIndicator, DialogueIndicator;

    private RoomType _currentLocation;

    void Start()
    {
        _currentLocation = NPCData.TimeSlots[0].CurrentRoom;
    }

    public void OnEnable()
    {
        InvestigationTimelineSystem.OnHourEntered += CheckLocationAndIndicator;
        InvestigationTimelineSystem.OnTimeReset += ResetLocation;
    }

    public void OnDisable()
    {
        InvestigationTimelineSystem.OnHourEntered -= CheckLocationAndIndicator;
        InvestigationTimelineSystem.OnTimeReset -= ResetLocation;
    }

    private void CheckLocationAndIndicator(int hour)
    {
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;
        NPCTimeSlot slot = NPCData.TimeSlots[index];

        RoomType expectedLocation = slot.CurrentRoom;
        if (expectedLocation != _currentLocation)
        {
            foreach (NPCRoomWaypointEntry entry in Waypoints)
            {
                if (entry.Room == expectedLocation)
                {
                    NavAgent.SetDestination(entry.Waypoint.position);
                    break;
                }
            }
        }

        _currentLocation = expectedLocation;

        SymptomIndicator.enabled = slot.Symptom != Symptom.None;
        DialogueIndicator.enabled = slot.CharacterDialogue != "";
    }

    private void ResetLocation()
    {
        RoomType expectedLocation = NPCData.TimeSlots[0].CurrentRoom;
        foreach (NPCRoomWaypointEntry entry in Waypoints)
        {
            if (entry.Room == expectedLocation)
            {
                NavAgent.Warp(entry.Waypoint.position);
                break;
            }
        }
    }
}

[System.Serializable]
public class NPCRoomWaypointEntry
{
    public RoomType Room;
    public Transform Waypoint;
}