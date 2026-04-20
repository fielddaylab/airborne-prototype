using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCreator : MonoBehaviour
{
    public HighlighterSlot SelectedSlot;

    private void OnEnable()
    {
        SlotEvent.OnSlotSelected += HandleSlotSelection;
        SlotEvent.OnClearHighlights += HandleClear;
    }

    private void OnDisable()
    {
        SlotEvent.OnSlotSelected -= HandleSlotSelection;
        SlotEvent.OnClearHighlights -= HandleClear;
    }

    private void HandleSlotSelection(HighlighterSlot highlighterSlot)
    {
        if (SelectedSlot == null)
        {
            SelectedSlot = highlighterSlot;
        } 
        else
        {
            if (SelectedSlot.SlotType == highlighterSlot.SlotType && SelectedSlot.IsSlot != highlighterSlot.IsSlot) 
            {
                Debug.Log("Slot it in!");
                if (SelectedSlot.IsSlot)
                {
                    SelectedSlot.SlotImage.texture = highlighterSlot.SlotImage.texture;
                } 
                else
                {
                    highlighterSlot.SlotImage.texture = SelectedSlot.SlotImage.texture;
                }
            } else
            {
                SelectedSlot = null;
            }
        }
    }

    public void HandleClear()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left mouse button pressed.");
        }
    }
}
