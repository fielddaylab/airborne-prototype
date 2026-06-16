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
        // TODO: Move this into the meter anchor
        // Then each meter is what is actually updating information, which makes a lot more sense
        // And it fixes the issue of tracking multiple pollutants, just put it on the meters instead
        
        TimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(TrackedRoom.RoomTypeValue, time);
        KnowledgeType pollutantKnowledge = PlayerKnowledgeState.PollutantKnowledgeKey[TrackedPollutant];
        if (slot != null) PlayerKnowledgeState.Discover(TrackedRoom.RoomTypeValue, time, pollutantKnowledge);
    }
}
