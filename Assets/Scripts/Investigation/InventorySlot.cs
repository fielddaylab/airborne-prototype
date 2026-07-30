using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Sprite EmptySlot;
    public InventoryDraggable Draggable;
    public Image BackgroundImage;

    public bool IsEmpty = true;
    public bool isOpen = false;

    public bool IsPlayerInventory = false;

    public void Awake()
    {
        Draggable.gameObject.SetActive(false);
        BackgroundImage.sprite = EmptySlot;
        Draggable.ParentSlot = this;
    }

    public void LoadEquipment(EquipmentMapObject map, EquipmentType type)
    {
        Draggable.gameObject.SetActive(true);
        Draggable.Setup(map, type);
        BackgroundImage.sprite = null;
        IsEmpty = false;
        isOpen = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!IsEmpty || !isOpen) return;
        
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out InventoryDraggable dragger))
        {
            dragger.SetNewParent(this);
            IsEmpty = false;
        }
    }

    public void SetValid()
    {
        BackgroundImage.sprite = null;
        Destroy(Draggable.gameObject);
        isOpen = true;
    }
}
