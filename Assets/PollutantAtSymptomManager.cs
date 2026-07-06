using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PollutantAtSymptomManager : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public InvestigationMap LockedMap;
    public Slider FalseSlider;

    void Awake()
    {
        LockedMap.gameObject.SetActive(false);
    }

    public void Setup(int unconsciousTime, PollutantType pollutant)
    {
        LockedMap.gameObject.SetActive(true);
        FalseSlider.value = unconsciousTime - 13;
        FalseSlider.interactable = false;

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
