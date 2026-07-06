using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PollutantAtSourceManager : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public InvestigationMap LockedMap;
    public Slider FalseSlider;

    void Awake()
    {
        LockedMap.gameObject.SetActive(false);
    }
    
    public void Setup(int earliestHour, PollutantType pollutant)
    {
        LockedMap.gameObject.SetActive(true);
        FalseSlider.value = earliestHour - 13;
        FalseSlider.interactable = false;

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
