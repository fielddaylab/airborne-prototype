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
        //_highlightTexture.enabled = true;
    }

    

    private void HandleSlotSelection(HighlighterSlot highlighterSlot)
    {
        if (highlighterSlot.SlotType == this.SlotType && highlighterSlot.IsSlot != IsSlot)
        {
            _highlightTexture.enabled = true;
        }
    }

    public void HandleClear()
    {
        _highlightTexture.enabled = false;
    }
}
