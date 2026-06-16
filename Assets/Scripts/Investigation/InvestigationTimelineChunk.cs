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

    private struct PollutantUIEntry
    {
        public KnowledgeType Knowledge;
        public PollutantType Pollutant;
        public TextMeshProUGUI Text;
        public string Label;
    }

    private List<PollutantUIEntry> _pollutantEntries;

    private void Awake()
    {
        _pollutantEntries = new List<PollutantUIEntry>
        {
            new PollutantUIEntry {Knowledge = KnowledgeType.CO2, Pollutant = PollutantType.CO2, Text = COText, Label = "CO" },
            new PollutantUIEntry { Knowledge = KnowledgeType.NO,  Pollutant = PollutantType.NO,  Text = NOText, Label = "NO" },
            new PollutantUIEntry { Knowledge = KnowledgeType.O3,  Pollutant = PollutantType.O3,  Text = O3Text, Label = "O3" },
            new PollutantUIEntry { Knowledge = KnowledgeType.VOC, Pollutant = PollutantType.VOC, Text = VOCText, Label = "VOC" }
        };

        TimelineImage.enabled = false;
        TextEnabled(false);
    }

    public void SetGraphics(RoomType type, int hour, RoomTimeSlot slot)
    {
        TimelineImage.enabled = false;
        TextEnabled(false);
        if (slot == null) return;
        
        bool anyKnowledgeKnown = false;
        foreach (PollutantUIEntry entry in _pollutantEntries)
        {
            if (PlayerKnowledgeState.IsKnown(type, hour, entry.Knowledge))
            {
                PollutantReading reading = slot.GetReading(entry.Pollutant);
                entry.Text.text = entry.Label + ":" + (reading != null ? reading.Concentration : 0);
                anyKnowledgeKnown = true;
                TextEnabled(true);
            }
        }

        if (!anyKnowledgeKnown)
        {
            if (PlayerKnowledgeState.IsKnown(type, hour, KnowledgeType.PollutantPresence))
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
    }

    private void TextEnabled(bool enabled)
    {
        NOText.enabled = enabled;
        O3Text.enabled = enabled;
        VOCText.enabled = enabled;
        COText.enabled = enabled;
    }
}
