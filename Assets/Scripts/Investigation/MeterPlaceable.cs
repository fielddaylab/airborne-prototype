using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterPlaceable : MonoBehaviour
{
    public InvestigationRoom ParentRoom;
    public bool IsClickable = false;
    private int metersOnMe;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 worldPosition = hit.point;
                MeterManager.OnShowMeter?.Invoke(worldPosition, ParentRoom);
            }
        }
    }

    private void HandleToolUpdated(ToolType type)
    {
        IsClickable = type == ToolType.Meter;
    }
}
