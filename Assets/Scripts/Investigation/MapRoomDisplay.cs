using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapRoomDisplay : MonoBehaviour
{
    public RoomType roomType;
    public TextMeshProUGUI RoomText;
    public Image[] NPCImages;
    public Image[] MeterImages;
    public TextMeshProUGUI[] MeterTexts;
    public Image[] FeatureImages;
    public Image[] ReadingChunks;
    public Image[] ReadingImages;
    public Image AmbigiousReadingImage;

    public void Start()
    {
        ClearDisplay();
        RoomText.text = gameObject.name;
    }

    public void UpdateDisplay(int hour)
    {
        ClearDisplay();
        
        // displaying basic room features
        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;
        if (PlayerKnowledgeState.IsKnownGenerally(roomType, KnowledgeType.RoomInfo))
        {
            int featuresTracked = 0;
            foreach (var feature in scenario.FeatureEvents)
            {
                if (feature.RoomType == roomType)
                {
                    FeatureImages[featuresTracked].sprite = InvestigationLookup.Instance.SourceImages.GetSprite(feature.FeatureType);
                    FeatureImages[featuresTracked].enabled = true;
                    
                    featuresTracked++;
                    if (featuresTracked >= 2) break;
                }
            }
        }
    }

    private void ClearDisplay()
    {
        foreach (Image image in NPCImages)
        {
            image.enabled = false;
        }

        foreach (Image image in MeterImages)
        {
            image.enabled = false;
        }

        foreach (TextMeshProUGUI text in MeterTexts)
        {
            text.text = "";
        }

        foreach (Image image in FeatureImages)
        {
            image.enabled = false;
        }

        foreach (Image image in ReadingChunks)
        {
            image.enabled = false;
            image.gameObject.SetActive(false);
        }

        foreach (Image image in ReadingImages)
        {
            image.enabled = false;
        }

        AmbigiousReadingImage.gameObject.SetActive(false); 
    }

}
