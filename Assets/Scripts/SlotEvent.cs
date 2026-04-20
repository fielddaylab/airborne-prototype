using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlotEvent 
{

    public static event Action<SlotType, bool> OnSlotSelected;
    public static event Action OnClearHighlights;

    public static void SelectSlot(SlotType type, bool isSlot)
    {
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnSlotSelected?.Invoke(type, isSlot);
    }

    public static void ClearHighlights()
    {
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnClearHighlights?.Invoke();
    }
}
