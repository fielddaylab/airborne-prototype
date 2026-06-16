using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObservableBox : MonoBehaviour
{
    public InvestigationNPCObject NPCData;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    private void OnMouseDown()
    {
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;

        NPCTimeSlot slot = NPCData.TimeSlots[index];
        RoomType room = slot.CurrentRoom;

        PlayerKnowledgeState.Discover(room, hour, KnowledgeType.NPCDialogue);
        PlayerKnowledgeState.Discover(room, hour, KnowledgeType.NPCSymptom);
    }

    private void HandleToolUpdated(ToolType type)
    {
        if (type == ToolType.Observe) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
