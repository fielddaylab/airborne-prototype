using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationTimelineChunk : MonoBehaviour
{
    /*
    This code is horribly structured and should get refactored later
    For some ideas, pass in a data type that asks for x to be overlayed, and then this becomes
    a modular system, rather than specifiying each timeline type.
    */

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
    public Image FeatureImage;
    public Color FeatureOffColor;

    [Header("Clickables")]
    public Image InvalidImage;
    public Button ValidImage;

    public static Action OnValidSelected;

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
            new PollutantUIEntry {Knowledge = KnowledgeType.CO, Pollutant = PollutantType.CO, Text = COText, Label = "CO" },
            new PollutantUIEntry { Knowledge = KnowledgeType.NO,  Pollutant = PollutantType.NOx,  Text = NOText, Label = "NO" },
            new PollutantUIEntry { Knowledge = KnowledgeType.O3,  Pollutant = PollutantType.O3,  Text = O3Text, Label = "O3" },
            new PollutantUIEntry { Knowledge = KnowledgeType.VOC, Pollutant = PollutantType.VOC, Text = VOCText, Label = "VOC" }
        };

        ClearChunk();
    }

    private void OnEnable()
    {
        ValidImage.onClick.AddListener(HandleTimelineClick);
    }

    private void OnDisable()
    {
        ValidImage.onClick.RemoveListener(HandleTimelineClick);
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
                    if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.NPCPresence)) 
                    {
                        NPCImages[npcTracked].gameObject.SetActive(true);
                        NPCImages[npcTracked].enabled = true;
                        NPCImages[npcTracked].sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(npc.Character);
                    }
                    
                    npcTracked++;
                    if (npcTracked >= 3) break;
                }
            }
        }
    }

    public void SetNPCGraphics(RoomType room, CharacterType character, int hour, bool isNewRoom, NPCTimeSlot slot)
    {
        ClearChunk();

        if (PlayerKnowledgeState.IsKnownCharacterly(character, hour, KnowledgeType.NPCSymptom))
        {
            NPCOverlay.SetActive(true);
            if (slot.Symptom != Symptom.None)
            {
                SymptomImage.sprite = InvestigationLookup.Instance.SymptomMap.GetSprite(slot.Symptom);
                SymptomImage.enabled = true;
                SymptomImage.gameObject.SetActive(true);
            }
        }

        if (PlayerKnowledgeState.IsKnownCharacterly(character, hour, KnowledgeType.NPCDialogue))
        {
            NPCOverlay.SetActive(true);
            if (slot.CharacterDialogue != "")
            {
                DialogueImage.enabled = true;
                DialogueImage.gameObject.SetActive(true);
            }
        }

        // Need to run over this and check for dialogue and symptoms, and put on timeline if they exist
        // you then also need to check for room changes, in which case the title of the room they have entered should show up
        if (PlayerKnowledgeState.IsKnownHourly(room, hour, KnowledgeType.NPCPresence))
        {
            NPCOverlay.SetActive(true);
            if (isNewRoom)
            {
                RoomTextBG.SetActive(true);
                RoomText.text = slot.CurrentRoom.ToString();
            }
        }
    }

    public void SetFeatureGraphics(RoomType room, FeatureType feature, int hour, FeatureTimeSlot slot)
    {
        ClearChunk();
        SourceOverlay.SetActive(true);

        // just need to show the features if the players have discovered them
        // and change the lightness/darkness depending on that status

        KnowledgeType knowledgeType = InvestigationLookup.Instance.FeatureMap.GetKnowledgeType(feature);

        if (PlayerKnowledgeState.IsKnownHourly(room, hour, knowledgeType)) {
            FeatureImage.enabled = true;
            FeatureImage.sprite = InvestigationLookup.Instance.SourceImages.GetSprite(feature);
            FeatureImage.color = (slot.FeatureEvent == FeatureEvent.On) ? Color.white : FeatureOffColor;
        }
    }

    public void SetDetailedFeatureGraphics(RoomType roomType, FeatureType feature, int hour, FeatureTimeSlot featureSlot, RoomTimeSlot roomSlot, PollutantType targetPollutant)
    {
        ClearChunk();
        SourceOverlay.SetActive(true);

        bool valid = false;

        // just need to show the features if the players have discovered them
        // and change the lightness/darkness depending on that status

        KnowledgeType knowledgeType = InvestigationLookup.Instance.FeatureMap.GetKnowledgeType(feature);

        bool featureOn = false;
        if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledgeType)) {
            FeatureImage.enabled = true;
            FeatureImage.sprite = InvestigationLookup.Instance.SourceImages.GetSprite(feature);
            FeatureImage.color = (featureSlot.FeatureEvent == FeatureEvent.On) ? Color.white : FeatureOffColor;
            featureOn = featureSlot.FeatureEvent == FeatureEvent.On;
        }

        RoomOverlay.SetActive(true);
        
        TimelineImage.enabled = false;
        TextEnabled(false);
        foreach (PollutantUIEntry entry in _pollutantEntries)
        {
            entry.Text.text = entry.Label + ":?";
        }

        if (featureSlot == null) return;
        
        bool anyKnowledgeKnown = false;
        foreach (PollutantUIEntry entry in _pollutantEntries)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, entry.Knowledge))
            {
                PollutantReading reading = roomSlot.GetReading(entry.Pollutant);
                entry.Text.text = entry.Label + ":" + (reading != null ? reading.Concentration : 0);
                anyKnowledgeKnown = true;
                TextEnabled(true);

                if (reading != null && reading.Pollutant == targetPollutant && reading.Concentration > 0 && featureOn)
                {
                    valid = true;
                }
            }
        }

        if (!anyKnowledgeKnown)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.PollutantPresence))
            {
                TimelineImage.enabled = true;
                bool pollutantsPresent = roomSlot.PollutantReadings.Length > 0;
                    
                if (pollutantsPresent)
                {
                    TimelineImage.sprite = PollutantPresent;
                } else
                {
                    TimelineImage.sprite = PollutantAbsent;
                }
            }
        }

        if (valid)
        {
            ValidImage.gameObject.SetActive(true);
        } else
        {
            InvalidImage.gameObject.SetActive(true);
        }
    }

    public void SetDetailedNPCGraphics(RoomType roomType, CharacterType character, int hour, bool isNewRoom, Symptom targetSymptom, PollutantType targetPollutant, NPCTimeSlot NPCSlot, RoomTimeSlot roomSlot)
    {
        ClearChunk();
        NPCOverlay.SetActive(true);
        RoomOverlay.SetActive(true);

        Symptom blockSymptom = Symptom.None;

        if (PlayerKnowledgeState.IsKnownCharacterly(character, hour, KnowledgeType.NPCSymptom))
        {
            NPCOverlay.SetActive(true);
            if (NPCSlot.Symptom != Symptom.None)
            {
                SymptomImage.sprite = InvestigationLookup.Instance.SymptomMap.GetSprite(NPCSlot.Symptom);
                SymptomImage.enabled = true;
                SymptomImage.gameObject.SetActive(true);
                blockSymptom = NPCSlot.Symptom;
            }
        }

        bool valid = false;

        // Need to run over this and check for dialogue and symptoms, and put on timeline if they exist
        // you then also need to check for room changes, in which case the title of the room they have entered should show up
        if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.NPCPresence))
        {
            NPCOverlay.SetActive(true);
            if (isNewRoom)
            {
                RoomTextBG.SetActive(true);
                RoomText.text = NPCSlot.CurrentRoom.ToString();
            }
        }

        bool anyKnowledgeKnown = false;
        foreach (PollutantUIEntry entry in _pollutantEntries)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, entry.Knowledge))
            {
                PollutantReading reading = roomSlot.GetReading(entry.Pollutant);
                entry.Text.text = entry.Label + ":" + (reading != null ? reading.Concentration : 0);
                anyKnowledgeKnown = true;
                TextEnabled(true);

                if (reading != null && reading.Pollutant == targetPollutant && reading.Concentration > 0 && blockSymptom == targetSymptom)
                {
                    valid = true;
                }
            }
        }

        if (!anyKnowledgeKnown)
        {
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, KnowledgeType.PollutantPresence))
            {
                TimelineImage.enabled = true;
                bool pollutantsPresent = roomSlot.PollutantReadings.Length > 0;
                    
                if (pollutantsPresent)
                {
                    TimelineImage.sprite = PollutantPresent;
                } else
                {
                    TimelineImage.sprite = PollutantAbsent;
                }
            }
        }

        if (valid)
        {
            ValidImage.gameObject.SetActive(true);
        } else
        {
            InvalidImage.gameObject.SetActive(true);
        }
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

        DialogueImage.enabled = false;
        DialogueImage.gameObject.SetActive(false);

        SymptomImage.enabled = false;
        SymptomImage.gameObject.SetActive(false);

        RoomTextBG.SetActive(false);
        RoomText.text = "";

        SourceOverlay.SetActive(false);

        FeatureImage.enabled = false;
        FeatureImage.color = Color.white;

        ValidImage.gameObject.SetActive(false);
        InvalidImage.gameObject.SetActive(false);
    }

    private void HandleTimelineClick()
    {
        OnValidSelected?.Invoke();
    }
}
