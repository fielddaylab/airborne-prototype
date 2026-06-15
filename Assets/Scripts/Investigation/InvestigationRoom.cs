using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider;
    public static event Action<InvestigationRoom> OnRoomUpdated;
    public string RoomName;
    public RoomType RoomTypeValue;
    public float Size;
    public bool PlayerOccupied = false;
    public bool MeterPresent = false;
    public PollutantType TrackedPollutant;

    public void OnEnable()
    {
        InvestigationTimelineSystem.OnHourUpdated += HandleHourUpdated;
    }

    public void OnDisable()
    {
        InvestigationTimelineSystem.OnHourUpdated -= HandleHourUpdated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOccupied = true;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.currentRoom = this;
            player.playerCamera.UpdateRoom(player.currentRoom);
            OnRoomUpdated?.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOccupied = false;
        }
    }

    private void HandleHourUpdated(int time)
    {
        // TODO: Move this into the meter anchor
        // Then each meter is what is actually updating information, which makes a lot more sense
        // And it fixes the issue of tracking multiple pollutants, just put it on the meters instead
        if (MeterPresent)
        {
            TimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(RoomTypeValue, time);
            if (slot != null) PlayerKnowledgeState.Discover(RoomTypeValue, time, KnowledgeType.CO2);
        }
    }
}
