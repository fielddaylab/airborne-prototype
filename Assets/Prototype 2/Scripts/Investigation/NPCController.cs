using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public InvestigationNPCObject NPCData;
    public NavMeshAgent NavAgent;
    public NPCRoomWaypointEntry[] Waypoints;

    public SpriteRenderer SymptomIndicator;
    public GameObject DialogueObject;
    public TMP_Text DialogueText;

    private RoomType _currentLocation;

    private float _baseSpeed;

    void Start()
    {
        _currentLocation = NPCData.TimeSlots[0].CurrentRoom;
        _baseSpeed = NavAgent.speed;
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

    public void Update()
    {
        if (InvestigationTimelineSystem.Instance.TimelineSpeed == .75)
        {
            NavAgent.speed = _baseSpeed * 3f;
        } else
        {
            NavAgent.speed = _baseSpeed;
        }
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
        DialogueObject.SetActive(slot.CharacterDialogue != "");
        DialogueText.text = slot.CharacterDialogue;

        if (slot.Symptom == Symptom.LossConsciousness)
        {
            NavAgent.enabled = false; 
            
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -80f));
        } else
        {
            NavAgent.enabled = true; 
            
            transform.rotation = Quaternion.identity;
        }
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