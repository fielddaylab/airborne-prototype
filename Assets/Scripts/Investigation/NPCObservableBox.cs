using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObservableBox : MonoBehaviour
{
    public InvestigationNPCObject NPCData;
    private ToolType _lastToolType;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourEntered;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourEntered;
    }

    private void OnMouseDown()
    {
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;

        NPCTimeSlot slot = NPCData.TimeSlots[index];
        RoomType room = slot.CurrentRoom;

        PlayerKnowledgeState.Discover(NPCData.Character, hour, KnowledgeType.NPCDialogue);
        PlayerKnowledgeState.Discover(NPCData.Character, hour, KnowledgeType.NPCSymptom);

        PlayerKnowledgeState.Discover(slot.Symptom);

        VisibilityCheck();
    }

    private void HandleToolUpdated(ToolType type)
    {
        _lastToolType = type;
        if (type == ToolType.Observe)
        {
            VisibilityCheck();
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleHourEntered(int h)
    {
        if (_lastToolType == ToolType.Observe) VisibilityCheck();
    }

    private void VisibilityCheck()
    {
        // only show box as observable when info not known
        
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;

        NPCTimeSlot slot = NPCData.TimeSlots[index];
        RoomType room = slot.CurrentRoom;

        bool knowsDialogue = PlayerKnowledgeState.IsKnownCharacterly(NPCData.Character, hour, KnowledgeType.NPCDialogue);
        bool knowsSymptom = PlayerKnowledgeState.IsKnownCharacterly(NPCData.Character, hour, KnowledgeType.NPCSymptom);
        
        bool somethingToDisplay = false;
        if (slot.CharacterDialogue != "" || slot.Symptom != Symptom.None) somethingToDisplay = true; 

        if (somethingToDisplay && (!knowsDialogue || !knowsSymptom))
        {
            gameObject.SetActive(true);
            return;
        }

        gameObject.SetActive(false);
    }   
}
