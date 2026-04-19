using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlotEvent 
{

    public static event Action<SlotType> OnSlotSelected;
    public static event Action OnClearHighlights;

    public static void SelectSlot(SlotType type)
    {
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnSlotSelected?.Invoke(type);
    }

    public static void ClearHighlights()
    {
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnClearHighlights?.Invoke();
    }
}
