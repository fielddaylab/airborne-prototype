using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PollutantAtSourceManager : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    
    public void Setup(int earliestHour, PollutantType pollutant)
    {
        if (earliestHour < 99)
        {
            QuestionText.text = $"You first saw {pollutant} at {earliestHour - 12}PM. Was the source on in the same room at that time? Click from map.";
        } 
        else
        {
            QuestionText.text = "No valid data found.";
        }
    }
}
