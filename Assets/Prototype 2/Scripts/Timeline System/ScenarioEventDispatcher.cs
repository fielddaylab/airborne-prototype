using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioEventDispatcher : MonoBehaviour
{  
    public static event Action<RoomType, PollutantReading[]> OnPollutantUpdated;

    public void OnEnable()
    {
        TimelineSystem.OnHourEntered += HandleHourUpdated;
    }

    public void OnDisable()
    {
        TimelineSystem.OnHourEntered -= HandleHourUpdated;
    }

    private void HandleHourUpdated(int h)
    {
        ScenarioDataObject scenarioData = TimelineSystem.Instance.ScenarioData;

        foreach (InvestigationRoomObject roomObject in scenarioData.Rooms)
        {
            RoomTimeSlot match = null;
            foreach (RoomTimeSlot time in roomObject.TimeSlots) {
                if (time.Time == h) {
                    match = time; break; 
                }
            }

            PollutantReading[] readings = match != null ? match.PollutantReadings : Array.Empty<PollutantReading>();
            OnPollutantUpdated?.Invoke(roomObject.RoomTypeValue, readings);
        }
    }
}
