using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlighterSlot : MonoBehaviour
{
    private RawImage _highlightTexture;
    public RawImage SlotImage;
    public SlotType SlotType;
    public bool IsSlot = false;
    
    void Start()
    {
        _highlightTexture = GetComponent<RawImage>();
        _highlightTexture.enabled = false;
        
    }

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

    public void OnButtonClick()
    {
        SlotEvent.ClearHighlights();
        SlotEvent.SelectSlot(this);
    }

    private void HandleSlotSelection(HighlighterSlot highlighterSlot)
    {
        if (highlighterSlot.SlotType == SlotType && highlighterSlot.IsSlot != IsSlot)
        {
            _highlightTexture.enabled = true;
        }
    }

    public void HandleClear()
    {
        _highlightTexture.enabled = false;
        Debug.Log("I attempted to clear my highlights and my name is " + gameObject.name);
    }
}
