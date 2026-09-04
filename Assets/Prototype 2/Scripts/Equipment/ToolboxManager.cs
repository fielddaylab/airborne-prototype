using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxManager : MonoBehaviour
{
    public EquipmentMapObject EquipmentMap;
    public List<EquipmentType> EquipmentList;
    public Transform InventoryParent;
    
    void Start()
    {
        LoadEquipmentSlots();
    }

    public void LoadEquipmentSlots()
    {
        for (int i = 0; i < EquipmentList.Count; i++)
        {
            InventorySlot slot = InventoryParent.GetChild(i).GetComponent<InventorySlot>();
            slot.LoadEquipment(EquipmentMap, EquipmentList[i]);
        }
    }
}
