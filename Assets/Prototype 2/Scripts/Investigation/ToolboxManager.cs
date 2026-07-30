using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxManager : MonoBehaviour
{
    public EquipmentMapObject EquipmentMap;
    public Transform InventoryParent;
    
    void Start()
    {
        LoadEquipmentSlots();
    }

    public void LoadEquipmentSlots()
    {
        for (int i = 0; i < EquipmentMap.Sets.Length; i++)
        {
            InventorySlot slot = InventoryParent.GetChild(i).GetComponent<InventorySlot>();
            slot.LoadEquipment(EquipmentMap, EquipmentMap.Sets[i].Type);
        }
    }
}
