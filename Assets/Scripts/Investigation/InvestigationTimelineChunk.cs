using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationTimelineChunk : MonoBehaviour
{
    public GameObject RoomOverlay, NPCOverlay, SourceOverlay;
    
    [Header("Room Overlay")]
    public Sprite PollutantPresent;
    public Sprite PollutantAbsent;
    public Image TimelineImage;

    public TextMeshProUGUI NOText, O3Text, VOCText, COText;
    public Image[] NPCImages;

    [Header("NPC Overlay")]
    public GameObject RoomTextBG;
    public TextMeshProUGUI RoomText;
    public Image SymptomImage, DialogueImage;

    [Header("Source Overlay")]
    public Image SourceImage1;
    public Image SourceImage2;
    public Color SourceOffColor;

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

        ClearChunk();
    }

    public void SetRoomGraphics(RoomType roomType, int hour, RoomTimeSlot slot)
    {
        ClearChunk();
        RoomOverlay.SetActive(true);
        
        TimelineImage.enabled = false;
        TextEnabled(false);
        foreach (PollutantUIEntry entry in _pollutantEntries)
        {
            entry.Text.text = entry.Label + ":?";
        }

        if (slot == null) return;
        
        bool anyKnowledgeKnown = false;
        foreach (PollutantUIEntry entry in _pollutantEntries)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, entry.Knowledge))
            {
                PollutantReading reading = slot.GetReading(entry.Pollutant);
                entry.Text.text = entry.Label + ":" + (reading != null ? reading.Concentration : 0);
                anyKnowledgeKnown = true;
                TextEnabled(true);
            }
        }

        if (!anyKnowledgeKnown)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.PollutantPresence))
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

        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;

        int npcTracked = 0;
        foreach (var npc in scenario.NPCs)
        {
            foreach (var npcSlot in npc.TimeSlots)
            {
                if (npcSlot.Time == hour && npcSlot.CurrentRoom == roomType)
                {
                    NPCImages[npcTracked].gameObject.SetActive(true);
                    NPCImages[npcTracked].enabled = true;
                    NPCImages[npcTracked].sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(npc.Character);

                    npcTracked++;
                    if (npcTracked >= 3) break;
                }
            }
        }
    }

    public void SetNPCGraphics(CharacterType character, int hour, NPCTimeSlot slot)
    {
        ClearChunk();
        NPCOverlay.SetActive(true);

        
    }

    public void SetFeatureGraphics(FeatureType feature, int hour, FeatureTimeSlot slot)
    {
        ClearChunk();
        SourceOverlay.SetActive(true);


    }

    private void TextEnabled(bool enabled)
    {
        NOText.enabled = enabled;
        O3Text.enabled = enabled;
        VOCText.enabled = enabled;
        COText.enabled = enabled;
    }

    private void ClearChunk()
    {
        RoomOverlay.SetActive(false);

        foreach (var image in NPCImages) { image.enabled = false; image.gameObject.SetActive(false); }


        NPCOverlay.SetActive(false);
        SourceOverlay.SetActive(false);
    }

    
}
