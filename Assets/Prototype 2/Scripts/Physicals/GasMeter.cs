using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GasMeter : MonoBehaviour
{
    public InvestigationRoom TrackedRoom;
    public PollutantType TrackedPollutant;
    public TextMeshProUGUI Label;

    public void OnEnable()
    {
        TimelineSystem.OnHourLeft += HandleHourUpdated;
    }

    public void OnDisable()
    {
        TimelineSystem.OnHourLeft += HandleHourUpdated;
    }

    private void HandleHourUpdated(int time)
    {
        RoomTimeSlot slot = TimelineSystem.Instance.GetTimeSlot(TrackedRoom.RoomTypeValue, time);
        KnowledgeType pollutantKnowledge = PlayerKnowledgeState.PollutantKnowledgeKey[TrackedPollutant];
        if (slot != null) PlayerKnowledgeState.Discover(TrackedRoom.RoomTypeValue, time, pollutantKnowledge);
    }
}
