using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInvestigationTimeline : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_RoomText;

    private InvestigationRoom _currentRoom;
    private ToolType _currentToolType;
    private int _currentHour;

    public Slider TimelineSlider;

    // storing the information the player currently has
    private RoomTimeline[] _roomTimelines;

    private void Start()
    {
        ScenarioDataObject scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;
        _roomTimelines = new RoomTimeline[scenarioData.Rooms.Length];
    }

    private void OnEnable()
    {
        InvestigationRoom.OnRoomUpdated += HandleRoomUpdated;
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourUpdated += HandleHourUpdated;
    }

    public void OnDisable()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    // handle player moving between rooms and what information they should know
    private void HandleRoomUpdated(InvestigationRoom room)
    {
        m_RoomText.text = room.RoomName;
        _currentRoom = room;
        UpdateTimeline();
    }

    // handle player activating or disabling tools and what information they should know
    private void HandleToolUpdated(ToolType type)
    {
        _currentToolType = type;
        UpdateTimeline();
    }

    // handle time advancing for rooms and what info player should gain
    private void HandleHourUpdated(int hour)
    {
        _currentHour = hour;
        UpdateTimeline();
    }

    private void UpdateTimeline()
    {
        
    }
}

public class RoomTimeline
{
    
}