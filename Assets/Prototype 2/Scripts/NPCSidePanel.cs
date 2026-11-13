using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCSidePanel : MonoBehaviour
{
    public InfoCard Card;

    public Image SymptomOverlay, SourceOverlay;
    public TMP_Text SymptomText, SourceText;
    
    public void Setup()
    {
        ScenarioDataObject scenario = TimelineSystem.Instance.ScenarioData;

        Card.SetupNPC(scenario.MainNpc);
    }

    public void Fill()
    {
        ScenarioDataObject scenario = TimelineSystem.Instance.ScenarioData;

        SymptomSpriteMapObject symptomMap = AssetLookup.Instance.SymptomMap;
        RoomSpriteMapObject roomMap = AssetLookup.Instance.RoomMap;

        SymptomOverlay.sprite = symptomMap.GetSprite(scenario.MajorSymptom);
        SymptomText.text = scenario.MajorSymptom.ToString();

        SourceOverlay.sprite = roomMap.GetSprite(scenario.MajorSymptomRoom);
        SourceText.text = scenario.MajorSymptomRoom.ToString();
    }
}
