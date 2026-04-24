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
        HandleClear();
    }

    private void OnEnable()
    {
        SlotEvent.OnHighlighted += HandleSlotHighlighted;
        SlotEvent.OnClearHighlights += HandleClear;
    }

    private void OnDisable()
    {
        SlotEvent.OnHighlighted -= HandleSlotHighlighted;
        SlotEvent.OnClearHighlights -= HandleClear;
    }

    public void OnButtonClick()
    {
        SlotEvent.ClearHighlights();
        SlotEvent.SelectSlot(this);
    }

    private void HandleSlotHighlighted(SlotType slotType, bool isSlot)
    {
        if (slotType == SlotType && isSlot == IsSlot)
        {
            Color color = _highlightTexture.color;
            color.a = 255;
            _highlightTexture.color = color;
        }
    }

    public void HandleClear()
    {
        Color color = _highlightTexture.color;
        color.a = 0;
        _highlightTexture.color = color;
        Debug.Log("I attempted to clear my highlights and my name is " + gameObject.name);
    }
}
