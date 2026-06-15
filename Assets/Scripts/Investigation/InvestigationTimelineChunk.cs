using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationTimelineChunk : MonoBehaviour
{
    public Sprite PollutantPresent, PollutantAbsent;
    public Image TimelineImage;

    public TextMeshProUGUI NOText, O3Text, VOCText, COText;

    public void SetGraphics(RoomType type, int hour, TimeSlot slot)
    {
        TimelineImage.enabled = false;
        TextEnabled(false);
        if (slot == null) return;
        
        if (PlayerKnowledgeState.IsKnown(type, hour, KnowledgeType.CO2))
        {
            TextEnabled(true);
            if (slot.PollutantReadings.Length > 0)
            {
                COText.text = "CO:" + slot.PollutantReadings[0].Concentration;
            } else
            {
                COText.text = "CO:0";
            }
        } 
        else if (PlayerKnowledgeState.IsKnown(type, hour, KnowledgeType.PollutantPresence))
        {
            TimelineImage.enabled = true;
            bool pollutantsPresent = slot.PollutantReadings.Length > 0;
                
            if (pollutantsPresent)
            {
                TimelineImage.sprite = PollutantPresent;
            } else
            {
                TimelineImage.sprite = PollutantAbsent;
            }
        }
    }

    private void TextEnabled(bool enabled)
    {
        NOText.enabled = enabled;
        O3Text.enabled = enabled;
        VOCText.enabled = enabled;
        COText.enabled = enabled;
    }
}
