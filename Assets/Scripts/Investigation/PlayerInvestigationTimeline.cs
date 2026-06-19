using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInvestigationTimeline : MonoBehaviour
{
    // UI Stuff
    public TextMeshProUGUI TimelineText;
    public Slider TimelineSlider;
    public PlayerTimelineOverlay TimelineOverlay;


    // data stuff
    private InvestigationRoom _currentRoom;
    private ToolType _currentToolType;
    private int _currentHour;

    private RoomType _currentRoomType;
    private FeatureType _currentFeatureType;
    private CharacterType _currentCharacterType;
    private TimelineType _currentTimelineType;

    // event
    public static Action<Enum> OnTimelineRequested;

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
        OnTimelineRequested += HandleTimelineRequest;
    }

    public void OnDisable()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated -= HandleKnowledgeUpdated;
        OnTimelineRequested -= HandleTimelineRequest;

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
        
        if (_currentToolType == ToolType.Scan)
        {
            _currentRoomType = _currentRoom.RoomTypeValue;
            _currentTimelineType = TimelineType.Room;
            RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, _currentHour);
            if (slot != null) PlayerKnowledgeState.Discover(_currentRoom.RoomTypeValue, _currentHour, KnowledgeType.PollutantPresence);
        }

        UpdateTimelineVisuals(TimelineType.Room);
    }

    private void HandleKnowledgeUpdated()
    {
        UpdateTimelineVisuals(TimelineType.Room);
    }

    private void HandleTimelineRequest(Enum enumType)
    {
        if (enumType is CharacterType)
        {
            CharacterType character = (CharacterType) enumType;
            _currentTimelineType = TimelineType.NPC;
            _currentCharacterType = character;
        } 
        else if (enumType is FeatureType)
        {
            FeatureType feature = (FeatureType) enumType;
            _currentTimelineType = TimelineType.Feature;
            _currentFeatureType = feature;
        } 
        else if (enumType is RoomType)
        {
            RoomType room = (RoomType) enumType;
            _currentTimelineType = TimelineType.Room;
            _currentRoomType = room;
        }

        UpdateTimelineVisuals(_currentTimelineType);
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
                    RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoomType, actualHour);
                    TimelineOverlay.TimelineChunks[i].SetRoomGraphics(_currentRoomType, actualHour, slot);
                }

                TimelineText.text = _currentRoomType.ToString();

                Debug.Log("Current trying to display: " + _currentRoomType);

                break;
            case TimelineType.NPC:
                
                for (int i = 0; i < totalHours; i++)
                {
                    ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;
                    //InvestigationNPCObject npcObject = data.NPCs[i];

                }

                TimelineText.text = _currentCharacterType.ToString();
                
                Debug.Log("TODO!");

                break;
            case TimelineType.Feature:
                
                Debug.Log("TODO!");

                TimelineText.text = _currentFeatureType.ToString();

                break;
        }
    }
}
