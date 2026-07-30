using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDraggable : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button Draggable;
    public Image SlotImage;

    [HideInInspector] public InventorySlot ParentSlot;
    [HideInInspector] public bool Placed = false;

    private EquipmentType _equipmentType;
    private EquipmentMapObject _mapReference;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    //private Transform _lastParent;


    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(EquipmentMapObject map, EquipmentType type)
    {
        _mapReference = map;
        _equipmentType = type;
        
        SlotImage.sprite = EquipmentMapUtility.GetSprite(map, type);
    } 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolboxInfoManager.Instance.Populate(_mapReference, _equipmentType);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(InventoryDragAnchor.Instance.transform);

        ParentSlot.IsEmpty = true;
        
        Placed = false;
        SlotImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Placed) {
            SetNewParent(ParentSlot);
        }
    }

    public void SetNewParent(InventorySlot newSlot)
    {
        ParentSlot = newSlot;
        
        transform.SetParent(newSlot.transform, true);

        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        newSlot.IsEmpty = false;

        Placed = true;
        SlotImage.raycastTarget = true;
    }
}
