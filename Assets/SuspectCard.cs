using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectCard : MonoBehaviour
{
    public Image Portrait;
    public TMP_Text Label, Description;
    public Button Button;
    [NonSerialized] public PollutantType PollutantType;

    public void Setup(PollutantType pollutant)
    {
        PollutantType = pollutant;
        PollutantKnowledgeMapObject map = InvestigationLookup.Instance.PollutantMap;

        Portrait.sprite = PollutantKnowledgeMapUtility.GetSprite(map, pollutant);
        Label.text = PollutantKnowledgeMapUtility.GetFullName(map, pollutant);
        Description.text = PollutantKnowledgeMapUtility.GetDescription(map, pollutant);
    }
}
