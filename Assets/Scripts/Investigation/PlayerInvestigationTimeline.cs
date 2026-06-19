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

    public enum TimelineType
    {
        Room,
        NPC,
        Feature
    }

    private void OnEnable()
    {
        InvestigationRoom.OnRoomUpdated += HandleRoomUpdated;
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated += HandleKnowledgeUpdated;
    }

    public void OnDisable()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated -= HandleKnowledgeUpdated;
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

        if (_currentRoom == null) return;
        
        if (_currentToolType == ToolType.Scan)
        {
            RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, _currentHour);
            if (slot != null) PlayerKnowledgeState.Discover(_currentRoom.RoomTypeValue, _currentHour, KnowledgeType.PollutantPresence);
        }
        
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

        RoomText.text = _currentRoom.RoomName;
        
        if (_currentToolType == ToolType.Scan)
        {
            RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, _currentHour);
            if (slot != null) PlayerKnowledgeState.Discover(_currentRoom.RoomTypeValue, _currentHour, KnowledgeType.PollutantPresence);
        }

        UpdateTimelineVisuals(TimelineType.Room);
    }

    private void HandleKnowledgeUpdated()
    {
        UpdateTimelineVisuals(TimelineType.Room);
    }

    private void UpdateTimelineVisuals(TimelineType timelineType)
    {
        int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
        int totalHours = InvestigationTimelineSystem.Instance.TotalNumHours;
        
        switch (timelineType)
        {
            case TimelineType.Room:
                if (_currentRoom == null) return;

                for (int i = 0; i < totalHours; i++)
                {
                    int actualHour = baseHour + i;
                    RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, actualHour);
                    TimelineOverlay.TimelineChunks[i].SetRoomGraphics(_currentRoom.RoomTypeValue, actualHour, slot);
                }

                break;
            case TimelineType.NPC:
                
                for (int i = 0; i < totalHours; i++)
                {
                    ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;
                    InvestigationNPCObject npcObject = data.NPCs[i];

                }
                

                break;
            case TimelineType.Feature:
                
                break;
        }
    }
}
