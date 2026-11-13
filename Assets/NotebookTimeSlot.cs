using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookTimeSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TimestampText;
    [SerializeField] private Image m_SlotBG;
    [SerializeField] private Image m_EventIcon;
    [SerializeField] private TMP_Text m_BodyText;
    [SerializeField] private Image m_RoomIndicator;
    [SerializeField] private Image m_RoomIcon;

    [SerializeField] private Sprite EmptyOutline, FilledOutline;

    public void Init(int time)
    {
        m_TimestampText.text = time % 12 + ":00";
        Clear();
    }

    private void Clear()
    {
        m_EventIcon.enabled = false;
        m_BodyText.enabled = false;
        m_RoomIndicator.enabled = false;
        m_RoomIcon.enabled = false;

        m_SlotBG.sprite = EmptyOutline;
    }

    private void MarkFilled()
    {
        m_SlotBG.sprite = FilledOutline;
    }

    public void SetRoomNPCGraphics(RoomType roomType, int hour, RoomTimeSlot slot)
    {
        Clear();

        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;
        InvestigationNPCObject npc = scenario.NPCs[0];
        var npcSlot = npc.TimeSlots[0];
        foreach (var npcTimeSlot in npc.TimeSlots)
        {
            if (npcTimeSlot.Time == hour && npcTimeSlot.CurrentRoom == roomType)
            {
                npcSlot = npcTimeSlot;
            }
        }

        if (PlayerKnowledgeState.IsKnownCharacterly(npc.Character, hour, KnowledgeType.NPCSymptom))
        {
            if (npcSlot.Symptom != Symptom.None)
            {
                Sprite sympSprite = InvestigationLookup.Instance.SymptomMap.GetSprite(npcSlot.Symptom);
                m_EventIcon.sprite = sympSprite;
                m_EventIcon.enabled = true;
                m_BodyText.text = npcSlot.Symptom + "";
                m_BodyText.enabled = true;
            }

            MarkFilled();
        }
    }

    public void SetRoomFeatureGraphics(RoomType roomType, int hour, RoomTimeSlot slot)
    {
        Clear();

        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;
        InvestigationFeatureEventObject feature = scenario.FeatureEvents[0];
        var featureSlot = feature.TimeSlots[0];
        foreach (var featureTimeSlot in feature.TimeSlots)
        {
            if (featureTimeSlot.Time == hour && feature.RoomType == roomType)
            {
                featureSlot = featureTimeSlot;
            }
        }
        KnowledgeType knowledgeType = InvestigationLookup.Instance.FeatureMap.GetKnowledgeType(feature.FeatureType);
        if (PlayerKnowledgeState.IsKnownHourly(roomType, hour, knowledgeType)) {
            m_EventIcon.enabled = true; 
            m_BodyText.enabled = true;
            bool featureOn = featureSlot.FeatureEvent == FeatureEvent.On;
            FeatureSpriteMapObject featureMap = InvestigationLookup.Instance.FeatureSpriteMap;
            if (featureOn)
            {
                m_EventIcon.sprite = FeatureSpriteMapUtility.GetOnSprite(featureMap, feature.FeatureType);
                m_BodyText.text = "On";
            } 
            else
            {
                m_EventIcon.sprite = FeatureSpriteMapUtility.GetOffSprite(featureMap, feature.FeatureType);
                m_BodyText.text = "Off";
            }

            MarkFilled();
        }
    }
}
