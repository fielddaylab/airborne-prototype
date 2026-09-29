using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCSidePanel : MonoBehaviour
{
    public SuspectCard Card;

    public Image SymptomOverlay, SourceOverlay;
    public TMP_Text SymptomText, SourceText;
    
    public void Setup()
    {
        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;

        Card.SetupNPC(scenario.MainNpc);
    }

    public void Fill()
    {
        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;

        SymptomSpriteMapObject symptomMap = InvestigationLookup.Instance.SymptomMap;
        RoomSpriteMapObject roomMap = InvestigationLookup.Instance.RoomMap;

        SymptomOverlay.sprite = symptomMap.GetSprite(scenario.MajorSymptom);
        SymptomText.text = scenario.MajorSymptom.ToString();

        SourceOverlay.sprite = roomMap.GetSprite(scenario.MajorSymptomRoom);
        SourceText.text = scenario.MajorSymptomRoom.ToString();
    }
}
