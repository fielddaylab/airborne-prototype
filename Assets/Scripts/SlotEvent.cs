using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlotEvent 
{

    public static event Action<HighlighterSlot> OnSlotSelected;
    public static event Action OnClearHighlights;

    public static void SelectSlot(HighlighterSlot highlighterSlot)
    {
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnSlotSelected?.Invoke(highlighterSlot);
    }

    public static void ClearHighlights()
    {
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnClearHighlights?.Invoke();
    }
}
