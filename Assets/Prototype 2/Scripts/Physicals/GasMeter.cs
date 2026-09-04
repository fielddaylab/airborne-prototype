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
        InvestigationTimelineSystem.OnHourLeft += HandleHourUpdated;
    }

    public void OnDisable()
    {
        InvestigationTimelineSystem.OnHourLeft += HandleHourUpdated;
    }

    private void HandleHourUpdated(int time)
    {
        RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(TrackedRoom.RoomTypeValue, time);
        KnowledgeType pollutantKnowledge = PlayerKnowledgeState.PollutantKnowledgeKey[TrackedPollutant];
        if (slot != null) PlayerKnowledgeState.Discover(TrackedRoom.RoomTypeValue, time, pollutantKnowledge);
    }
}
