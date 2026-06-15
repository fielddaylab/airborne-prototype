using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInvestigationTimeline : MonoBehaviour
{
    // UI Stuff
    public TextMeshProUGUI RoomText;
    public Slider TimelineSlider;
    public PlayerTimelineOverlay TimelineOverlay;


    // data stuff
    private InvestigationRoom _currentRoom;
    private ToolType _currentToolType;
    private int _currentHour;
    private ScenarioDataObject _scenarioData;

    // lookup should be made before OnEanble
    private void Start()
    {
        _scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;
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
        InvestigationTimelineSystem.OnHourUpdated -= HandleHourUpdated;
    }

    // handle player moving between rooms and what information they should know
    private void HandleRoomUpdated(InvestigationRoom room)
    {
        _currentRoom = room;
        UpdateInformation();
    }

    // handle player activating or disabling tools and what information they should know
    private void HandleToolUpdated(ToolType type)
    {
        _currentToolType = type;
        UpdateInformation();
    }

    // handle time advancing for rooms and what info player should gain
    private void HandleHourUpdated(int hour)
    {
        _currentHour = hour;
        UpdateInformation();
    }

    private void UpdateInformation()
    {
        // for now, check if they should know if a pollutant is present in a room
        if (_currentRoom == null) return;
        
        if (_currentToolType == ToolType.Scan)
        {
            TimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, _currentHour);
            if (slot != null) PlayerKnowledgeState.Discover(_currentRoom.RoomTypeValue, _currentHour, KnowledgeType.PollutantPresence);
        }

        UpdateTimeline();
    }

    private void UpdateTimeline()
    {
        RoomText.text = _currentRoom.RoomName;

        int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
        int totalHours = InvestigationTimelineSystem.Instance.TotalNumHours;

        for (int i = 0; i < totalHours; i++)
        {
            int actualHour = baseHour + i;
            TimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, actualHour);
            TimelineOverlay.TimelineChunks[i].SetGraphics(_currentRoom.RoomTypeValue, actualHour, slot);
        }
    }
}
