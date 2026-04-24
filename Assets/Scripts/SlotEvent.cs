using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlotEvent 
{
    public static event Action<HighlighterSlot> OnSlotSelected;
    public static event Action OnClearHighlights;

    public static bool Locked = false;

    public static event Action OnUnlockSensorBlocks;
    public static event Action<SensorBlock> OnSensorBlockSelected;
    public static event Action OnLockSensorBlocks;

    public static void SelectSlot(HighlighterSlot highlighterSlot)
    {
        if (Locked) return;
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnSlotSelected?.Invoke(highlighterSlot);
    }

    public static void ClearHighlights()
    {
        if (Locked) return;
        if (GameManager.Instance.GamePhase != GamePhase.ArguingPollutant) return;
        OnClearHighlights?.Invoke();
    }

    public static void ForceClearHighlights()
    {
        OnClearHighlights?.Invoke(); // debug testing
    }

    public static void SelectSensorBlock(SensorBlock block)
    {
        OnSensorBlockSelected?.Invoke(block);
    }

    public static void UnlockSensorBlocks()
    {
        OnUnlockSensorBlocks?.Invoke();
    }

    public static void LockSensorBlocks()
    {
        OnLockSensorBlocks?.Invoke();
    }
}
