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
    private Dictionary<(RoomType, int), TimeSlot> _timeSlotLookup = new();

    // lookup should be made before OnEanble
    private void Start()
    {
        _scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;

        foreach (var room in _scenarioData.Rooms)
        {
            foreach (var slot in room.TimeSlots)
            {
                _timeSlotLookup[(room.RoomType, slot.Time)] = slot;
            }
        }
    }

    public TimeSlot GetTimeSlot(RoomType room, int hour)
    {
        _timeSlotLookup.TryGetValue((room, hour), out var slot);
        return slot;
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
            TimeSlot slot = GetTimeSlot(_currentRoom.RoomType, _currentHour);
            if (slot != null) PlayerKnowledgeState.Discover(_currentRoom.RoomType, _currentHour, KnowledgeType.PollutantPresence);
        }

        UpdateTimeline();
    }

    private void UpdateTimeline()
    {
        int startHour = InvestigationTimelineSystem.Instance.BaseHour;
        int totalHours = startHour + InvestigationTimelineSystem.Instance.TotalNumHours;
        for (int i = startHour; i < totalHours - 1; i++)
        {
            int trueIndex = i - startHour;
            TimelineOverlay.TimelineImages[trueIndex].enabled = false;
            
            if (PlayerKnowledgeState.IsKnown(_currentRoom.RoomType, i, KnowledgeType.PollutantPresence))
            {
                TimeSlot slot = GetTimeSlot(_currentRoom.RoomType, i);
                bool pollutantsPresent = slot.PollutantReadings.Length > 0;
                
                if (pollutantsPresent)
                {
                    TimelineOverlay.TimelineImages[trueIndex].sprite = TimelineOverlay.PollutantPresent;
                } else
                {
                    TimelineOverlay.TimelineImages[trueIndex].sprite = TimelineOverlay.PollutantAbsent;
                }

                TimelineOverlay.TimelineImages[trueIndex].enabled = true;
            }
        }
    }
}
