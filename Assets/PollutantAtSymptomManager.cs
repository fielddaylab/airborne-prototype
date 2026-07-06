using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PollutantAtSymptomManager : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    
    public void Setup(int unconsciousTime, PollutantType pollutant)
    {
        if (unconsciousTime < 99)
        {
            QuestionText.text = $"Roundy went unconscious at {unconsciousTime - 12}PM. Was the pollutant present in the room at the time? Click from map.";
        } 
        else
        {
            QuestionText.text = "No valid data found.";
        }
    }
}
