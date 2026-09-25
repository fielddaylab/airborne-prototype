using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TooltipText;
    public static TooltipHoverable CurrentHovered;
    [NonSerialized] public int Version;
    
    public void ChangeText(string newText)
    {
        if (TooltipText != newText)
        {
            TooltipText = newText;
            Version++;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentHovered != this)
        {
            CurrentHovered = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentHovered == this)
        {
            CurrentHovered = null;
        }
    }

    public void OnDisable()
    {
        if (CurrentHovered == this)
        {
            CurrentHovered = null;
        }
    }

}
