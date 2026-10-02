using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarTheoryManager : MonoBehaviour
{
    public TMP_Text PollutantPortraitText;
    public Image PollutantPortrait;

    public TMP_Text SourcePortraitText;
    public Image SourcePortrait;

    public SidebarTheoryPiece[] Pieces;

    public void Awake()
    {
        SourcePortrait.enabled = false;
        SourcePortraitText.text = "";
    }

    public void SetupPollutant(PollutantType pollutant)
    {
        
        PollutantPortraitText.text = InvestigationLookup.Instance.PollutantMap.GetFullName(pollutant);
        PollutantPortrait.sprite = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutant);

        foreach (var p in Pieces)
        {
            p.UpdateText(pollutant.ToString());
        }
    }

    public void SetupSource(FeatureType feature)
    {
        ScenarioDataObject scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;
        RoomType sourceRoom = ScenarioUtility.GetRoom(feature, scenarioData);

        SourcePortraitText.text = $"{sourceRoom} {feature}";
        
        Sprite featureSprite = FeatureSpriteMapUtility.GetOnSprite(InvestigationLookup.Instance.FeatureSpriteMap, feature);
        
        SourcePortrait.enabled = true;
        SourcePortrait.sprite = featureSprite;

        foreach (var p in Pieces)
        {
            p.UpdateImage(featureSprite);
        }
    }
}
