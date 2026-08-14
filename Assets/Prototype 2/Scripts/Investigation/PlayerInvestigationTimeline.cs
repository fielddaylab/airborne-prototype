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
    public Image TimelineIcon;
    public Slider TimelineSlider;
    public PlayerTimelineOverlay TimelineOverlay;


    // data stuff
    private InvestigationRoom _currentRoom;
    private EquipmentType _currentToolType;
    private int _currentHour;

    private RoomType _currentRoomType;
    private FeatureType _currentFeatureType;
    private CharacterType _currentCharacterType;
    private TimelineType _currentTimelineType;

    // event
    public static Action<Enum> OnTimelineRequested;

    public static Action<FeatureType, PollutantType> OnFeatureDetailRequested;
    public static Action<Symptom, PollutantType> OnNPCDetailRequested;
    public static Action OnResetRequested;

    public enum TimelineType
    {
        Room,
        NPC,
        Feature,
        FeatureDetail
    }

    private void OnEnable()
    {
        InvestigationRoom.OnRoomUpdated += HandleRoomUpdated;
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated += HandleKnowledgeUpdated;
        OnTimelineRequested += HandleTimelineRequest;
        OnFeatureDetailRequested += HandleFeatureRequest;
        OnResetRequested += HandleReset;
        OnNPCDetailRequested += HandleNPCRequest;
    }

    public void OnDisable()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated -= HandleKnowledgeUpdated;
        OnTimelineRequested -= HandleTimelineRequest;
        OnFeatureDetailRequested -= HandleFeatureRequest;
        OnResetRequested -= HandleReset;
        OnNPCDetailRequested += HandleNPCRequest;
    }

    // handle player moving between rooms and what information they should know
    private void HandleRoomUpdated(InvestigationRoom room)
    {
        _currentRoom = room;
        _currentRoomType = _currentRoom.RoomTypeValue;
        _currentTimelineType = TimelineType.Room;
        UpdateInformation();
    }

    // handle player activating or disabling tools and what information they should know
    private void HandleToolUpdated(EquipmentType type)
    {
        _currentToolType = type;

        if (_currentRoom == null) return;
        
        if (_currentToolType == EquipmentType.Scan)
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
        
        if (_currentToolType == EquipmentType.Scan)
        {
            _currentRoomType = _currentRoom.RoomTypeValue;
            _currentTimelineType = TimelineType.Room;
            RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoom.RoomTypeValue, _currentHour);
            if (slot != null) PlayerKnowledgeState.Discover(_currentRoom.RoomTypeValue, _currentHour, KnowledgeType.PollutantPresence);
        }

        // update npc information statuses
        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;
        foreach (var npc in scenario.NPCs)
        {
            int hour = InvestigationTimelineSystem.Instance.CurrentHour;
            int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
            int index = hour - baseHour;
            
            if (npc.TimeSlots[index].CurrentRoom == _currentRoom.RoomTypeValue)
            {
                PlayerKnowledgeState.Discover(_currentRoom.RoomTypeValue, hour, KnowledgeType.NPCPresence);
            }
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

                TimelineIcon.sprite = InvestigationLookup.Instance.RoomMap.GetSprite(_currentRoomType);
                TimelineText.text = _currentRoomType.ToString();

                break;
            case TimelineType.NPC:
                
                ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;

                foreach (var npc in data.NPCs) {   
                    if (npc.Character == _currentCharacterType) {
                        for (int i = 0; i < totalHours; i++)
                        {
                            int actualHour = baseHour + i;
                            NPCTimeSlot slot = npc.TimeSlots[i];

                            bool isNewRoom = false;
                            if (i == 0 || npc.TimeSlots[i - 1].CurrentRoom != npc.TimeSlots[i].CurrentRoom)
                            {
                                isNewRoom = true;
                            }

                            TimelineOverlay.TimelineChunks[i].SetNPCGraphics(_currentRoomType, npc.Character, actualHour, isNewRoom, slot);
                        }
                    }
                }

                TimelineIcon.sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(_currentCharacterType);
                TimelineText.text = _currentCharacterType.ToString();

                break;
            case TimelineType.Feature:
                
                data = InvestigationTimelineSystem.Instance.ScenarioData;

                foreach (var feature in data.FeatureEvents)
                {
                    if (feature.FeatureType == _currentFeatureType) {
                        for (int i = 0; i < totalHours; i++)
                        {
                            int actualHour = baseHour + i;
                            FeatureTimeSlot slot = feature.TimeSlots[i];
                            TimelineOverlay.TimelineChunks[i].SetFeatureGraphics(_currentRoomType, feature.FeatureType, actualHour, slot);
                        }
                    }
                }

                TimelineIcon.sprite = InvestigationLookup.Instance.SourceImages.GetSprite(_currentFeatureType);
                TimelineText.text = _currentFeatureType.ToString();

                break;
        }
    }

    private void HandleFeatureRequest(FeatureType featureType, PollutantType pollutantType)
    {
        int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
        int totalHours = InvestigationTimelineSystem.Instance.TotalNumHours;
        
        ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;

        foreach (var feature in data.FeatureEvents)
        {
            if (feature.FeatureType == featureType) {
                foreach (var room in data.Rooms)
                {
                    if (room.RoomTypeValue == feature.RoomType)
                    {
                        for (int i = 0; i < totalHours; i++)
                        {
                            int actualHour = baseHour + i;
                            FeatureTimeSlot featureSlot = feature.TimeSlots[i];
                            RoomTimeSlot roomSlot = room.TimeSlots[i];

                            TimelineOverlay.TimelineChunks[i].SetDetailedFeatureGraphics(feature.RoomType, feature.FeatureType, actualHour, featureSlot, roomSlot, pollutantType);
                        }
                    }
                }
            }
        }

        TimelineIcon.sprite = InvestigationLookup.Instance.SourceImages.GetSprite(featureType);
        TimelineText.text = featureType.ToString();
    }

    private void HandleNPCRequest(Symptom symptom, PollutantType pollutant)
    {
        int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
        int totalHours = InvestigationTimelineSystem.Instance.TotalNumHours;
        
        ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;

        foreach (var npc in data.NPCs) {   
            if (npc.Character == data.MainNpc) {
                for (int i = 0; i < totalHours; i++)
                {
                    int actualHour = baseHour + i;
                    NPCTimeSlot slot = npc.TimeSlots[i];

                    bool isNewRoom = false;
                    if (i == 0 || npc.TimeSlots[i - 1].CurrentRoom != npc.TimeSlots[i].CurrentRoom)
                    {
                        isNewRoom = true;
                    }

                    foreach (var room in data.Rooms)
                    {
                        if (room.RoomTypeValue == npc.TimeSlots[i].CurrentRoom)
                        {
                            RoomTimeSlot roomSlot = room.TimeSlots[i];
                            TimelineOverlay.TimelineChunks[i].SetDetailedNPCGraphics(room.RoomTypeValue, npc.Character, actualHour, isNewRoom, symptom, pollutant, slot, roomSlot);
                        }
                    }
                }
            }
        }

        TimelineIcon.sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(data.MainNpc);
        TimelineText.text = data.MainNpc.ToString();
    }

    private void HandleReset()
    {
        HandleRoomUpdated(_currentRoom);
    }
}
