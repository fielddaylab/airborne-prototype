using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapConnector : MonoBehaviour
{
    public RoomType FirstRoom, SecondRoom;
    public bool IsVent;
    public string ID;
    public Image[] VentOverlays;

    public void Start()
    {
        Reset();
    }

    public void VentUpdate(int hour, PollutantType overlayedPollutant)
    {
        if (!IsVent) return;
        Reset();

        KnowledgeType knowledge = InvestigationLookup.Instance.PollutantMap.GetKnowledge(overlayedPollutant);
        if (!PlayerKnowledgeState.IsKnownHourly(FirstRoom, hour, knowledge)
            || !PlayerKnowledgeState.IsKnownHourly(SecondRoom, hour, knowledge))
        {
            return;
        }

        RoomTimeSlot firstSlot = InvestigationTimelineSystem.Instance.GetTimeSlot(FirstRoom, hour);
        RoomTimeSlot secondSlot = InvestigationTimelineSystem.Instance.GetTimeSlot(SecondRoom, hour);

        Color overlayColor = InvestigationLookup.Instance.PollutantMap.GetColor(overlayedPollutant);
        PollutantReading fReading = firstSlot.GetReading(overlayedPollutant);
        PollutantReading sReading = secondSlot.GetReading(overlayedPollutant);
        float fConcentration = fReading != null ? fReading.Concentration / 4f : 0;
        float sConcentration = sReading != null ? sReading.Concentration / 4f : 0;
        float finalConcentration = fConcentration + sConcentration / 2f;

        overlayColor.a = finalConcentration / 4f; // magic number, but just set to max it can possible be later

        foreach (var image in VentOverlays)
        {
            image.color = overlayColor;
            image.enabled = true;
        } 
    }

    private void Reset()
    {
        foreach (var image in VentOverlays)
        {
            image.enabled = false;
        }
    }
}
