using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDraggable : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Button Draggable;
    public Image SlotImage;
    private EquipmentType _equipmentType;
    private EquipmentMapObject _mapReference;

    private RectTransform rectTransform;
    private Canvas canvas;
    Transform _lastParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
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
        _lastParent = transform.parent;
        transform.SetParent(InventoryDragAnchor.Instance.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_lastParent, true);

        if (transform is RectTransform rectTransform)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
