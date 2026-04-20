using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlighterSlot : MonoBehaviour
{
    private RawImage _highlightTexture;
    public RawImage FillSlot;
    public SlotType SlotType;
    public bool IsSlot = false;
    
    void Start()
    {
        _highlightTexture = GetComponent<RawImage>();
        _highlightTexture.enabled = false;
        if (FillSlot != null)
        {
            FillSlot.enabled = false;
        }
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
        SlotEvent.SelectSlot(this.SlotType, this.IsSlot);
    }

    private void HandleSlotSelection(SlotType selectedType, bool isSlot)
    {
        if (selectedType == this.SlotType && IsSlot != isSlot)
        {
            _highlightTexture.enabled = true;
        }
    }

    public void HandleClear()
    {
        _highlightTexture.enabled = false;
    }
}
