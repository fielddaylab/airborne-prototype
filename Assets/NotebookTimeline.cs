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
        NPC,
        Feature,
        Scanner,
        Meter
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
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated += HandleKnowledgeUpdated;

        NPCObservableBox.OnObserveNPC += HandleNPCObservation;
        FeatureObservableBox.OnObserveFeature += HandleFeatureObservation;
    }

    private void OnDisable()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
        PlayerKnowledgeState.OnKnowledgeUpdated -= HandleKnowledgeUpdated;

        NPCObservableBox.OnObserveNPC -= HandleNPCObservation;
        FeatureObservableBox.OnObserveFeature -= HandleFeatureObservation;
    }

    private void HandleNPCObservation(CharacterType character)
    {
        _currentTimelineType = TimelineType.NPC;
        _currentCharacterType = character;
        UpdateTimelineVisuals(_currentTimelineType);
    }

    private void HandleFeatureObservation(FeatureType feature)
    {
        _currentTimelineType = TimelineType.Feature;
        _currentFeatureType = feature;
        UpdateTimelineVisuals(_currentTimelineType);
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
        //UpdateTimelineVisuals(_currentTimelineType);
    }

    private void UpdateInformation()
    {
        if (_currentRoom == null) return;
        
        if (_currentToolType == EquipmentType.Scan)
        {
            _currentRoomType = _currentRoom.RoomTypeValue;
            _currentTimelineType = TimelineType.NPC;
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
            case TimelineType.NPC:
                for (int i = 0; i < totalHours; i++)
                {
                    int actualHour = baseHour + i;
                    TimeSlots[i].SetRoomNPCGraphics(_currentCharacterType, actualHour);
                }

                FocusIcon.sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(_currentCharacterType);
                FocusTitle.text = _currentCharacterType.ToString();

                break;
            case TimelineType.Feature:
                for (int i = 0; i < totalHours; i++)
                {
                    int actualHour = baseHour + i;
                    TimeSlots[i].SetRoomFeatureGraphics(_currentFeatureType, actualHour);
                }

                var map = InvestigationLookup.Instance.FeatureSpriteMap;
                FocusIcon.sprite = FeatureSpriteMapUtility.GetOnSprite(map, _currentFeatureType);
                FocusTitle.text = _currentFeatureType.ToString();
                
                break;
        }
    }
}
