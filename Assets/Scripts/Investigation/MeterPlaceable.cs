using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterPlaceable : MonoBehaviour
{
    public InvestigationRoom ParentRoom;
    public bool IsClickable = false;

    void OnEnable()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
    }

    void OnDisable()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    private void OnMouseDown()
    {
        if (IsClickable)
        {
            Debug.Log("Blargh!");
        }
    }

    private void HandleToolUpdated(ToolType type)
    {
        IsClickable = type == ToolType.Meter;
    }
}
