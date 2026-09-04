using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterPlaceable : MonoBehaviour
{
    public InvestigationRoom ParentRoom;
    public bool IsClickable = false;

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPosition = hit.point;

            // gate internally
            MeterManager.OnShowMeter?.Invoke(worldPosition, ParentRoom);
            PlaceableEquipmentManager.OnShowMeter?.Invoke(worldPosition, ParentRoom);
        }
    }
}
