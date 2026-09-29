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

    public void SetupPollutant(PollutantType pollutant)
    {
        PollutantType = pollutant;
        PollutantKnowledgeMapObject map = InvestigationLookup.Instance.PollutantMap;

        Portrait.enabled = true;
        Portrait.sprite = PollutantKnowledgeMapUtility.GetSprite(map, pollutant);
        Label.text = PollutantKnowledgeMapUtility.GetFullName(map, pollutant);
        Description.text = PollutantKnowledgeMapUtility.GetDescription(map, pollutant);
    }

    public void SetupNPC(CharacterType c)
    {
        Portrait.enabled = true;
        CharacterSpriteMapObject map = InvestigationLookup.Instance.CharacterMap;

        Portrait.sprite = CharacterLookupUtility.GetSprite(map, c);
        Label.text = c.ToString();
        Description.text = CharacterLookupUtility.GetBlurb(map, c);
    }

    public void Clear()
    {
        Label.text = "";
        Description.text = "";
        Portrait.enabled = false;
    }
}
