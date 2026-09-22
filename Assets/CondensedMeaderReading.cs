using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CondensedMeaderReading : MonoBehaviour
{
    public Color DefaultColor;

    public Image[] ReadingTicks;
    public Image Unknown;
    public TMP_Text Label;
    
    public void UpdateDisplay(bool known, int concentration, PollutantType type)
    {
        Reset();

        Label.text = type.ToString();
        Color pollutantCol = InvestigationLookup.Instance.PollutantMap.GetColor(type);
        Label.color = pollutantCol;
        
        if (known)
        {
            for (int i = 0; i < ReadingTicks.Length; i++)
            {
                ReadingTicks[i].gameObject.SetActive(true);
                if (i < concentration)
                {
                    ReadingTicks[i].color = pollutantCol;
                } else
                {
                    ReadingTicks[i].color = DefaultColor;
                }
            }
        }
        else
        {
            Unknown.gameObject.SetActive(true);
        }
    }

    public void Reset()
    {
        foreach (var image in ReadingTicks)
        {
            image.gameObject.SetActive(false);
        }



        Unknown.gameObject.SetActive(false);
    }
}
