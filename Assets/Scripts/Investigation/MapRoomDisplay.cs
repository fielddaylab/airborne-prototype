using System;
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

    public Sprite PollutantPresent, PollutantAbsent;

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

            int metersTracked = 0;
            foreach (var meter in InvestigationTimelineSystem.Instance.Meters)
            {
                if (meter.TrackedRoom.RoomTypeValue != roomType) continue;

                MeterImages[metersTracked].enabled = true;
                MeterTexts[metersTracked].text = meter.Label.text;

                metersTracked++;
                if (metersTracked >= 2) break;
            }

            int npcTracked = 0;
            foreach (var npc in scenario.NPCs)
            {
                foreach (var npcSlot in npc.TimeSlots)
                {
                    if (npcSlot.Time == hour && npcSlot.CurrentRoom == roomType)
                    {
                        NPCImages[npcTracked].enabled = true;
                        NPCImages[npcTracked].sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(npc.Character);

                        npcTracked++;
                        if (npcTracked >= 2) break;
                    }
                }
            }
        }

        // hour specific information
        RoomTimeSlot slot = InvestigationTimelineSystem.Instance.GetTimeSlot(roomType, hour);

        bool anyKnowledgeKnown = false;
        foreach (PollutantType pollutant in Enum.GetValues(typeof(PollutantType)))
        {
            KnowledgeType knowledge = InvestigationLookup.Instance.PollutantMap.GetKnowledge(pollutant);
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledge))
            {
                anyKnowledgeKnown = true;
            }
        }

        // fill the display
        if (anyKnowledgeKnown)
        {
            foreach (Image chunk in ReadingChunks)
            {
                chunk.enabled = true;
                chunk.gameObject.SetActive(true);
            }

            int totalConcentration = 0;
            foreach (PollutantReading reading in slot.PollutantReadings)
            {
                PollutantType pollutant = reading.Pollutant;
                int concentration = reading.Concentration;
                Sprite spr = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutant);

                for (int i = totalConcentration; i < totalConcentration + concentration && i < 6; i++)
                {
                    Debug.Log("Setting image " + i + " to " + pollutant.ToString());
                    ReadingImages[i].enabled = true;
                    ReadingImages[i].sprite = spr;
                }

                totalConcentration += concentration;
                Debug.Log("Concentration: " + concentration);

                if (totalConcentration >= 6) break;
            }
        }

        // don't put the cloud in if no need
        if (!anyKnowledgeKnown)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.PollutantPresence))
            {
                AmbigiousReadingImage.enabled = true;
                AmbigiousReadingImage.gameObject.SetActive(true);
                bool pollutantsPresent = slot.PollutantReadings.Length > 0;
                    
                if (pollutantsPresent)
                {
                    AmbigiousReadingImage.sprite = PollutantPresent;
                } else
                {
                    AmbigiousReadingImage.sprite = PollutantAbsent;
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
