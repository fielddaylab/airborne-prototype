using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookTimeline : MonoBehaviour
{
    [SerializeField] private Image FocusIcon;
    [SerializeField] private TMP_Text FocusTitle;
    [SerializeField] private NotebookTimeSlot[] TimeSlots;

    // data stuff
    private InvestigationRoom _currentRoom;
    private EquipmentType _currentToolType;
    private int _currentHour;
    private RoomType _currentRoomType;
    private FeatureType _currentFeatureType;
    private CharacterType _currentCharacterType;
    private TimelineType _currentTimelineType;
    private bool _inSourceMode = true;

    public enum TimelineType
    {
        RoomNPC,
        RoomFeature
    }

    private void Start()
    {
        int startHour = InvestigationTimelineSystem.Instance.BaseHour;
        for (int i = 0; i < TimeSlots.Length; i++)
        {
            TimeSlots[i].Init(startHour + i);
        }
    }

    private void OnEnable()
    {
        InvestigationRoom.OnRoomUpdated += HandleRoomUpdated;
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated += HandleKnowledgeUpdated;
    }

    private void OnDisable()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated -= HandleKnowledgeUpdated;
    }

    private void HandleRoomUpdated(InvestigationRoom room)
    {
        _currentRoom = room;
        _currentRoomType = _currentRoom.RoomTypeValue;
        _currentTimelineType = TimelineType.RoomNPC;
        UpdateInformation();
    }

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

    private void HandleHourUpdated(int hour)
    {
        _currentHour = hour;
        UpdateInformation();
    }

    private void HandleKnowledgeUpdated()
    {
        UpdateTimelineVisuals(_currentTimelineType);
    }

    private void UpdateInformation()
    {
        if (_currentRoom == null) return;
        
        if (_currentToolType == EquipmentType.Scan)
        {
            _currentRoomType = _currentRoom.RoomTypeValue;
            _currentTimelineType = TimelineType.RoomNPC;
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

        UpdateTimelineVisuals(_currentTimelineType);
    }

    private void UpdateTimelineVisuals(TimelineType timelineType)
    {
        int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
        int totalHours = InvestigationTimelineSystem.Instance.TotalNumHours;
        
        switch (timelineType)
        {
            case TimelineType.RoomNPC:
                if (_currentRoom == null) return;

                for (int i = 0; i < totalHours; i++)
                {
                    int actualHour = baseHour + i;
                    RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoomType, actualHour);
                    TimeSlots[i].SetRoomNPCGraphics(_currentRoomType, actualHour, slot);
                }

                FocusIcon.sprite = InvestigationLookup.Instance.RoomMap.GetSprite(_currentRoomType);
                FocusTitle.text = _currentRoomType.ToString();

                break;
            case TimelineType.RoomFeature:
                if (_currentRoom == null) return;

                for (int i = 0; i < totalHours; i++)
                {
                    int actualHour = baseHour + i;
                    RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoomType, actualHour);
                    TimeSlots[i].SetRoomFeatureGraphics(_currentRoomType, actualHour, slot);
                }

                FocusIcon.sprite = InvestigationLookup.Instance.RoomMap.GetSprite(_currentRoomType);
                FocusTitle.text = _currentRoomType.ToString();
                
                break;
            // case TimelineType.NPC:
                
            //     ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;

            //     foreach (var npc in data.NPCs) {   
            //         if (npc.Character == _currentCharacterType) {
            //             for (int i = 0; i < totalHours; i++)
            //             {
            //                 int actualHour = baseHour + i;
            //                 NPCTimeSlot slot = npc.TimeSlots[i];

            //                 bool isNewRoom = false;
            //                 if (i == 0 || npc.TimeSlots[i - 1].CurrentRoom != npc.TimeSlots[i].CurrentRoom)
            //                 {
            //                     isNewRoom = true;
            //                 }
            //                 RoomTimeSlot roomSlot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoomType, actualHour);
            //                 TimelineOverlay.TimelineChunks[i].SetNPCGraphics(_currentRoomType, npc.Character, actualHour, isNewRoom, slot, roomSlot);
            //             }
            //         }
            //     }

            //     FocusIcon.sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(_currentCharacterType);
            //     TimelineText.text = _currentCharacterType.ToString();

            //     break;
            // case TimelineType.Feature:
                
            //     data = InvestigationTimelineSystem.Instance.ScenarioData;

            //     foreach (var feature in data.FeatureEvents)
            //     {
            //         if (feature.FeatureType == _currentFeatureType) {
            //             for (int i = 0; i < totalHours; i++)
            //             {
            //                 int actualHour = baseHour + i;
            //                 FeatureTimeSlot slot = feature.TimeSlots[i];
            //                 RoomTimeSlot roomSlot = InvestigationTimelineSystem.Instance.GetTimeSlot(_currentRoomType, actualHour);
            //                 TimelineOverlay.TimelineChunks[i].SetFeatureGraphics(_currentRoomType, feature.FeatureType, actualHour, slot, roomSlot);
            //             }
            //         }
            //     }

            //     FocusIcon.sprite = FeatureSpriteMapUtility.GetOnSprite(InvestigationLookup.Instance.FeatureSpriteMap, _currentFeatureType);
            //     TimelineText.text = _currentFeatureType.ToString();

            //     break;
        }
    }
}
