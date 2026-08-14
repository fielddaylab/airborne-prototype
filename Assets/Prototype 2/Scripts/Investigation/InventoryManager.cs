using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int ValidSlots = 4;
    
    public Transform InventoryParent;
    
    void Start()
    {
        EnableSlots();
    }

    public void EnableSlots()
    {
        for (int i = 0; i < ValidSlots; i++)
        {
            InventorySlot slot = InventoryParent.GetChild(i).GetComponent<InventorySlot>();
            slot.SetValid();
        }
    }
}
