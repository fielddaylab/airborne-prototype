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
    public Image[] MeterIndicators;
    public Image[] FeatureImages;
    public Image AmbigiousReadingImage;

    public Image GasOverlay;

    public Sprite PollutantPresent, PollutantAbsent;

    public void Awake()
    {
        ClearDisplay();
        RoomText.text = gameObject.name;
    }

    public void UpdateDisplay(int hour, PollutantType overlayedPollutant)
    {
        ClearDisplay();

        RoomTimelineRequester roomTimelineRqstr = GetComponent<RoomTimelineRequester>();
        roomTimelineRqstr.RoomType = roomType;
        
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
                    FeatureTimelineRequester requester = FeatureImages[featuresTracked].GetComponent<FeatureTimelineRequester>();
                    requester.Feature = feature.FeatureType;

                    PlayerKnowledgeState.Discover(feature.FeatureType); // move somewhere else later
                    
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
                        if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.NPCPresence)) {
                            NPCImages[npcTracked].enabled = true;
                            NPCImages[npcTracked].sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(npc.Character);
                            CharacterTimelineRequester requester = NPCImages[npcTracked].GetComponent<CharacterTimelineRequester>();
                            requester.Character = npc.Character;
                        }

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

                if (pollutant == overlayedPollutant)
                {
                    Color overlayColor = InvestigationLookup.Instance.PollutantMap.GetMaterial(pollutant);
                    PollutantReading reading = slot.GetReading(pollutant);
                    overlayColor.a = reading != null ? reading.Concentration / 4f : 0; // magic number, but just set to max it can possible be later
                    GasOverlay.enabled = true;
                    GasOverlay.color = overlayColor;
                }
            }
        }

        // meter indicators
        if (anyKnowledgeKnown)
        {
            int metersTracked = 0;
            foreach (var meter in InvestigationTimelineSystem.Instance.Meters)
            {
                if (meter.TrackedRoom.RoomTypeValue != roomType) continue;
                
                if (meter.TrackedPollutant != overlayedPollutant) 
                {
                    PollutantType meterPollutant = meter.TrackedPollutant;
                    PollutantReading reading = slot.GetReading(meterPollutant);
                    if (reading != null)
                    {
                        if (reading.Concentration > 0)
                        {
                            MeterIndicators[metersTracked].enabled = true;
                            Color indicatorColor = InvestigationLookup.Instance.PollutantMap.GetMaterial(meterPollutant);
                            MeterIndicators[metersTracked].color = indicatorColor;
                        }
                    }
                }

                metersTracked++;
                if (metersTracked >= 2) break;
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
        GasOverlay.enabled = false;
        
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

        foreach (Image image in MeterIndicators)
        {
            image.enabled = false;
        }

        foreach (Image image in FeatureImages)
        {
            image.enabled = false;
        }

        AmbigiousReadingImage.gameObject.SetActive(false); 
    }

}
