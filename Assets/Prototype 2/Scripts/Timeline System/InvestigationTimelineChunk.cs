using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InvestigationTimelineChunk : MonoBehaviour
{
    /*
    This code is horribly structured and should get refactored later
    For some ideas, pass in a data type that asks for x to be overlayed, and then this becomes
    a modular system, rather than specifiying each timeline type.
    */

    public GameObject RoomOverlay, NPCOverlay;
    
    [Header("Room Overlay")]
    public Sprite PollutantPresent;
    public Sprite PollutantAbsent;
    public Image TimelineImage;

    public CondensedMeaderReading[] CondensedReadings;
    public SpecificReading Specific;

    private bool _inSpecificMode = false;
    private PollutantType _targetedPollutant;

    public static Action<(bool, PollutantType)> OnOverlayChange;

    public Image[] NPCImages;
    public Image[] NPCSymptomImages;

    [Header("NPC Overlay")]
    public GameObject RoomTextBG;
    public TextMeshProUGUI RoomText;
    public Image SymptomImage, DialogueImage;

    [Header("Source Overlay")]
    public Image[] FeatureImages;

    [Header("Clickables")]
    public Button ValidImage;

    public static Action OnValidSelected;

    public static Action<int> OnSlotSelected;
    private int RepresentedHour;

    private struct PollutantUIEntry
    {
        public KnowledgeType Knowledge;
        public PollutantType Pollutant;
        public TextMeshProUGUI Text;
        public string Label;
    }

    //private List<PollutantUIEntry> _pollutantEntries;

    private Action _rebuild;

    private void Awake()
    {
        // _pollutantEntries = new List<PollutantUIEntry>
        // {
        //     new PollutantUIEntry {Knowledge = KnowledgeType.CO, Pollutant = PollutantType.CO, Text = COText, Label = "CO" },
        //     new PollutantUIEntry { Knowledge = KnowledgeType.NO,  Pollutant = PollutantType.NOx,  Text = NOText, Label = "NO" },
        //     new PollutantUIEntry { Knowledge = KnowledgeType.O3,  Pollutant = PollutantType.O3,  Text = O3Text, Label = "O3" },
        //     new PollutantUIEntry { Knowledge = KnowledgeType.VOC, Pollutant = PollutantType.VOC, Text = VOCText, Label = "VOC" }
        // };

        ClearChunk();
    }

    private void OnEnable()
    {
        ValidImage.onClick.AddListener(HandleTimelineClick);
        OnOverlayChange += HandleOverlayChange;
    }

    private void OnDisable()
    {
        ValidImage.onClick.RemoveListener(HandleTimelineClick);
        OnOverlayChange -= HandleOverlayChange;
    }

    private void HandleOverlayChange((bool, PollutantType) tuple)
    {
        _inSpecificMode = tuple.Item1;
        _targetedPollutant = tuple.Item2;
        _rebuild?.Invoke();
    }

    public void SetRoomNPCGraphics(RoomType roomType, int hour, RoomTimeSlot slot)
    {
        RepresentedHour = hour;
        _rebuild = () => SetRoomNPCGraphics(roomType, hour, slot);
        ClearChunk();
        RoomOverlay.SetActive(true);
        SetPollutantDisplay(roomType, hour, slot);

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

                        if (PlayerKnowledgeState.IsKnownCharacterly(npc.Character, hour, KnowledgeType.NPCSymptom))
                            {
                                if (npcSlot.Symptom != Symptom.None)
                                {
                                    Sprite sympSprite = InvestigationLookup.Instance.SymptomMap.GetSprite(npcSlot.Symptom);
                                    NPCSymptomImages[npcTracked].sprite = sympSprite;
                                    NPCSymptomImages[npcTracked].enabled = true;
                                } else
                                {
                                    NPCSymptomImages[npcTracked].enabled = false;
                                }
                            } else
                            {
                                NPCSymptomImages[npcTracked].enabled = false;
                            }
                    }
                    
                    npcTracked++;
                    if (npcTracked >= 3) break;
                }
            }
        }
    }

    public void SetPollutantDisplay(RoomType roomType, int hour, RoomTimeSlot slot)
    {
        RepresentedHour = hour;
        PollutantDataObject[] pollutantDatas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        
        TimelineImage.enabled = false;
        TextEnabled(false);

        for (int i = 0; i < pollutantDatas.Length; i++)
        {
            CondensedReadings[i].UpdateDisplay(false, 0, pollutantDatas[i].Type);
        }
        Specific.Clear();

        if (slot == null) return;
        
        bool anyKnowledgeKnown = false;
        for (int i = 0; i < pollutantDatas.Length; i++)
        {
            KnowledgeType knowledge = InvestigationLookup.Instance.PollutantMap.GetKnowledge(pollutantDatas[i].Type);
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledge))
            {
                PollutantReading reading = slot.GetReading(pollutantDatas[i].Type);

                if (reading!= null) {
                    if (!_inSpecificMode) CondensedReadings[i].UpdateDisplay(true, reading.Concentration, pollutantDatas[i].Type);
                    else if (pollutantDatas[i].Type == _targetedPollutant) Specific.SetPollutant(reading.Concentration, pollutantDatas[i].Type);
                } 
                else
                {
                    if (!_inSpecificMode) CondensedReadings[i].UpdateDisplay(true, 0, pollutantDatas[i].Type);
                    else if (pollutantDatas[i].Type == _targetedPollutant) Specific.SetPollutant(0, pollutantDatas[i].Type);
                }
                anyKnowledgeKnown = true;
                if (!_inSpecificMode) TextEnabled(true);
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
    }

    public void SetRoomFeatureGraphics(RoomType roomType, int hour, RoomTimeSlot slot)
    {
        RepresentedHour = hour;
        _rebuild = () => SetRoomFeatureGraphics(roomType, hour, slot);
        ClearChunk();
        RoomOverlay.SetActive(true);
        SetPollutantDisplay(roomType, hour, slot);

        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;

        int featuresTracked = 0;
        foreach (var featureEvent in scenario.FeatureEvents)
        {
            foreach (var featureSlot in featureEvent.TimeSlots)
            {
                if (featureSlot.Time == hour && featureEvent.RoomType == roomType)
                {
                    Debug.Log(featureEvent.FeatureType);

                    KnowledgeType knowledgeType = InvestigationLookup.Instance.FeatureMap.GetKnowledgeType(featureEvent.FeatureType);
                    FeatureImages[featuresTracked].gameObject.SetActive(true);
                    FeatureImages[featuresTracked].enabled = true;
                    FeatureSpriteMapObject featureMap = InvestigationLookup.Instance.FeatureSpriteMap;

                    if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledgeType)) 
                    {
                       bool featureOn = featureSlot.FeatureEvent == FeatureEvent.On;
                        if (featureOn)
                        {
                            FeatureImages[featuresTracked].sprite = FeatureSpriteMapUtility.GetOnSprite(featureMap, featureEvent.FeatureType);
                        } 
                        else
                        {
                            FeatureImages[featuresTracked].sprite = FeatureSpriteMapUtility.GetOffSprite(featureMap, featureEvent.FeatureType);
                        }
                    } else
                    {
                        FeatureImages[featuresTracked].sprite = FeatureSpriteMapUtility.GetUnkownSprite(featureMap, featureEvent.FeatureType);
                    }
                    
                    featuresTracked++;
                    if (featuresTracked >= 3) break;
                }
            }
        }
    }

    public void SetNPCGraphics(RoomType room, CharacterType character, int hour, bool isNewRoom, NPCTimeSlot slot)
    {
        RepresentedHour = hour;
        _rebuild = () => SetNPCGraphics(room, character, hour, isNewRoom, slot);
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
        RepresentedHour = hour;
        _rebuild = () => SetFeatureGraphics(room, feature, hour, slot);
        ClearChunk();
        RoomOverlay.SetActive(true);
        //SourceOverlay.SetActive(true);

        // just need to show the features if the players have discovered them
        // and change the lightness/darkness depending on that status

        KnowledgeType knowledgeType = InvestigationLookup.Instance.FeatureMap.GetKnowledgeType(feature);
        FeatureImages[0].enabled = true;
        FeatureImages[0].gameObject.SetActive(true);
        FeatureSpriteMapObject featureMap = InvestigationLookup.Instance.FeatureSpriteMap;

        if (PlayerKnowledgeState.IsKnownHourly(room, hour, knowledgeType)) {
            
            bool featureOn = slot.FeatureEvent == FeatureEvent.On;
            
            if (featureOn)
            {
                FeatureImages[0].sprite = FeatureSpriteMapUtility.GetOnSprite(featureMap, feature);
            } 
            else
            {
                FeatureImages[0].sprite = FeatureSpriteMapUtility.GetOffSprite(featureMap, feature);
            }
        } else
        {
            FeatureImages[0].sprite = FeatureSpriteMapUtility.GetUnkownSprite(featureMap, feature);
        }
    }

    public void SetDetailedFeatureGraphics(RoomType roomType, FeatureType feature, int hour, FeatureTimeSlot featureSlot, RoomTimeSlot roomSlot, PollutantType targetPollutant)
    {
        RepresentedHour = hour;
        _rebuild = () => SetDetailedFeatureGraphics(roomType, feature, hour, featureSlot, roomSlot, targetPollutant);
        ClearChunk();
        //SourceOverlay.SetActive(true);

        bool valid = false;

        // just need to show the features if the players have discovered them
        // and change the lightness/darkness depending on that status

        KnowledgeType knowledgeType = InvestigationLookup.Instance.FeatureMap.GetKnowledgeType(feature);

        bool featureOn = false;
        if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledgeType)) {
            FeatureImages[0].enabled = true;
            FeatureImages[0].gameObject.SetActive(true);
            featureOn = featureSlot.FeatureEvent == FeatureEvent.On;
            FeatureSpriteMapObject featureMap = InvestigationLookup.Instance.FeatureSpriteMap;
            
            if (featureOn)
            {
                FeatureImages[0].sprite = FeatureSpriteMapUtility.GetOnSprite(featureMap, feature);
            } 
            else
            {
                FeatureImages[0].sprite = FeatureSpriteMapUtility.GetOffSprite(featureMap, feature);
            }
        }

        RoomOverlay.SetActive(true);
        
        TimelineImage.enabled = false;
        TextEnabled(false);

        PollutantDataObject[] pollutantDatas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        for (int i = 0; i < pollutantDatas.Length; i++)
        {
            CondensedReadings[i].UpdateDisplay(false, 0, pollutantDatas[i].Type);
        }
        Specific.Clear();

        if (featureSlot == null) return;
        
        bool anyKnowledgeKnown = false;
        for (int i = 0; i < pollutantDatas.Length; i++)
        {
            KnowledgeType knowledge = InvestigationLookup.Instance.PollutantMap.GetKnowledge(pollutantDatas[i].Type);
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledge))
            {
                PollutantReading reading = roomSlot.GetReading(pollutantDatas[i].Type);
                
                if (reading!= null) {
                    if (!_inSpecificMode) CondensedReadings[i].UpdateDisplay(true, reading.Concentration, pollutantDatas[i].Type);
                    else if (pollutantDatas[i].Type == _targetedPollutant) Specific.SetPollutant(reading.Concentration, pollutantDatas[i].Type);
                } 
                else
                {
                    if (!_inSpecificMode) CondensedReadings[i].UpdateDisplay(true, 0, pollutantDatas[i].Type);
                }

                anyKnowledgeKnown = true;
                TextEnabled(true);
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
    }

    public void SetDetailedNPCGraphics(RoomType roomType, CharacterType character, int hour, bool isNewRoom, Symptom targetSymptom, PollutantType targetPollutant, NPCTimeSlot NPCSlot, RoomTimeSlot roomSlot)
    {
        RepresentedHour = hour;
        _rebuild = () => SetDetailedNPCGraphics(roomType, character, hour, isNewRoom, targetSymptom, targetPollutant, NPCSlot, roomSlot);
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
        
        PollutantDataObject[] pollutantDatas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;

        for (int i = 0; i < CondensedReadings.Length; i++)
        {
            CondensedReadings[i].UpdateDisplay(false, 0, pollutantDatas[i].Type);
        }
        Specific.Clear();

        for (int i = 0; i < pollutantDatas.Length; i++)
        {
            KnowledgeType knowledge = InvestigationLookup.Instance.PollutantMap.GetKnowledge(pollutantDatas[i].Type);
            if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledge))
            {
                PollutantReading reading = roomSlot.GetReading(pollutantDatas[i].Type);
                if (reading!= null) {
                    if (!_inSpecificMode) CondensedReadings[i].UpdateDisplay(true, reading.Concentration, pollutantDatas[i].Type);
                    else if (pollutantDatas[i].Type == _targetedPollutant) Specific.SetPollutant(reading.Concentration, pollutantDatas[i].Type);
                } 
                else
                {
                    if (!_inSpecificMode) CondensedReadings[i].UpdateDisplay(true, 0, pollutantDatas[i].Type);
                }
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
    }

    private void TextEnabled(bool enabled)
    {
        foreach (var c in CondensedReadings)
        {
            c.gameObject.SetActive(enabled);
        }
        Specific.Clear();
    }

    private void ClearChunk()
    {
        RoomOverlay.SetActive(false);

        foreach (var image in NPCImages) { image.enabled = false; image.gameObject.SetActive(false); }
        foreach (var image in NPCSymptomImages) { image.enabled = false;}


        NPCOverlay.SetActive(false);

        DialogueImage.enabled = false;
        DialogueImage.gameObject.SetActive(false);

        SymptomImage.enabled = false;
        SymptomImage.gameObject.SetActive(false);

        RoomTextBG.SetActive(false);
        RoomText.text = "";

        //SourceOverlay.SetActive(false);

        foreach (var image in FeatureImages) { image.enabled = false; image.gameObject.SetActive(false); }
    }

    private void HandleTimelineClick()
    {
        OnValidSelected?.Invoke();
        OnSlotSelected?.Invoke(RepresentedHour);
    }
}

public struct TimelineData
{
    public PollutantType Pollutant;
    public int Concentration;
    public CharacterType[] Characters;
    public Symptom[] Symptoms;
    public FeatureType[] Features;
}