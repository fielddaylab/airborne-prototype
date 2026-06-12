using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationTimeline : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_RoomText;
    public Slider TimelineSlider;

    private void Start()
    {
        InvestigationRoom.OnRoomUpdated += HandleRoomUpdated;
        InvestigationTimelineSystem.OnHourUpdated += HandleHourUpdated;
    }

    public void OnDestroy()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
        InvestigationTimelineSystem.OnHourUpdated -= HandleHourUpdated;
    }

    private void HandleRoomUpdated(InvestigationRoom room)
    {
        m_RoomText.text = room.RoomName;
    }

    private void HandleHourUpdated(int hour)
    {
        TimelineSlider.value = hour;
    }
}
