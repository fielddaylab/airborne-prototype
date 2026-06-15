using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioEventDispatcher : MonoBehaviour
{  
    public static event Action<RoomType, PollutantReading[]> OnPollutantUpdated;

    public void OnEnable()
    {
        InvestigationTimelineSystem.OnHourUpdated += HandleHourUpdated;
    }

    public void OnDisable()
    {
        InvestigationTimelineSystem.OnHourUpdated -= HandleHourUpdated;
    }

    private void HandleHourUpdated(int h)
    {
        ScenarioDataObject scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;

        foreach (InvestigationRoomObject roomObject in scenarioData.Rooms)
        {
            TimeSlot match = null;
            foreach (TimeSlot time in roomObject.TimeSlots) {
                if (time.Time == h) {
                    match = time; break; 
                }
            }

            PollutantReading[] readings = match != null ? match.PollutantReadings : Array.Empty<PollutantReading>();
            OnPollutantUpdated?.Invoke(roomObject.Room, readings);
        }
    }
}
