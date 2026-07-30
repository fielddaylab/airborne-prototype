using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Sprite EmptySlot;
    public InventoryDraggable Draggable;
    public Image BackgroundImage;

    public bool IsPlayerInventory = false;

    public void Awake()
    {
        Draggable.gameObject.SetActive(false);
        BackgroundImage.sprite = EmptySlot;
    }

    public void LoadEquipment(EquipmentMapObject map, EquipmentType type)
    {
        Draggable.gameObject.SetActive(true);
        Draggable.Setup(map, type);
        BackgroundImage.sprite = null;
    }

    public void SetValid()
    {
        BackgroundImage.sprite = null;
    }
}
