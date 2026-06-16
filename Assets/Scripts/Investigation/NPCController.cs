using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public InvestigationNPCObject NPCData;
    public NavMeshAgent NavAgent;
    public NPCRoomWaypointEntry[] Waypoints;



    private RoomType _currentLocation;

    void Start()
    {
        _currentLocation = NPCData.TimeSlots[0].CurrentRoom;
    }

    public void OnEnable()
    {
        InvestigationTimelineSystem.OnHourEntered += CheckLocation;
        InvestigationTimelineSystem.OnTimeReset += ResetLocation;
    }

    public void OnDisable()
    {
        InvestigationTimelineSystem.OnHourEntered -= CheckLocation;
        InvestigationTimelineSystem.OnTimeReset -= ResetLocation;
    }

    private void CheckLocation(int hour)
    {
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;

        RoomType expectedLocation = NPCData.TimeSlots[index].CurrentRoom;
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