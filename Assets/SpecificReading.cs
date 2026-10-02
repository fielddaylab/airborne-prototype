using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecificReading : MonoBehaviour
{
    public Image[] Images;
    public Color EmptyColor;

    public void SetPollutant(int Concentration, PollutantType pollutant)
    {
        Color pollutantColor = InvestigationLookup.Instance.PollutantMap.GetColor(pollutant);
        
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].enabled = true;
            
            if (i < Concentration) Images[i].color = pollutantColor;
            else Images[i].color = EmptyColor;
        }
    }

    public void Clear()
    {
        foreach (var i in Images)
        {
            i.enabled = false;
        }
    }
}
