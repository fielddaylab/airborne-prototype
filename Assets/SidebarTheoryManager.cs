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

    public Button RequestTheory;

    public BossPromptController BossPrompter;

    private PollutantType _Pollutant;
    private FeatureType _Source;

    public void Awake()
    {
        SourcePortrait.enabled = false;
        SourcePortraitText.text = "";
        if (RequestTheory != null) RequestTheory.onClick.AddListener(HandleTheorySubmission);
    }

    public void SetupPollutant(PollutantType pollutant)
    {
        
        PollutantPortraitText.text = InvestigationLookup.Instance.PollutantMap.GetFullName(pollutant);
        PollutantPortrait.sprite = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutant);

        _Pollutant = pollutant;

        foreach (var p in Pieces)
        {
            p.TargetPollutant = pollutant;
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

        _Source = feature;

        foreach (var p in Pieces)
        {
            p.TargetSource = feature;
            p.UpdateImage(featureSprite);
        }
    }

    private void HandleTheorySubmission()
    {
        BossPrompter.StartBossSequence(_Pollutant, _Source);
        InvestigationTimelineSystem.Instance.PauseTime(true);
        transform.parent.gameObject.SetActive(false);
    }
}
