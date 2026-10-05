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

    public void Awake()
    {
        Reset();
    }

    public void UpdateDisplay(bool known, int concentration, PollutantType type)
    {
        Reset();

        Label.text = type.ToString();
        Label.enabled = true;
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

        if (type == PollutantType.None)
        {
            Reset();
        }
    }

    public void Reset()
    {
        foreach (var image in ReadingTicks)
        {
            image.gameObject.SetActive(false);
        }

        Label.enabled = false;

        Unknown.gameObject.SetActive(false);
    }
}
