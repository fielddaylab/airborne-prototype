using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/Room Object")]
public class InvestigationRoomObject : ScriptableObject
{
    public RoomType RoomTypeValue;
    public RoomTimeSlot[] TimeSlots;
}

[System.Serializable]
public class RoomTimeSlot
{
    public int Time;
    public PollutantReading[] PollutantReadings;

    private Dictionary<PollutantType, PollutantReading> _readingLookup;

    public PollutantReading GetReading(PollutantType type)
    {
        if (_readingLookup == null)
        {
            _readingLookup = new Dictionary<PollutantType, PollutantReading>();
            foreach (PollutantReading reading in PollutantReadings)
            {
                _readingLookup[reading.Pollutant] = reading;
            }
        }

        PollutantReading result;
        if (_readingLookup.TryGetValue(type, out result))
        {
            return result;
        }
        return null;
    }
}

[System.Serializable]
public class PollutantReading
{
    public PollutantType Pollutant;
    public int Concentration;
}