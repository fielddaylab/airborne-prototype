using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorObservableBox : MonoBehaviour
{
    public string ConnectorID;
    private EquipmentType _lastToolType;
    public GameObject FlyerPrefab;
    public Sprite VentSprite;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourEntered;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourEntered;
    }

    private void OnMouseDown()
    {
        PlayerKnowledgeState.Discover(ConnectorID);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        RectTransform canvasRect = CaseFileManager.Instance.AnimatedItemLocation.root as RectTransform;
        Canvas canvas = canvasRect.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.worldCamera;

        GameObject flyer = Instantiate(FlyerPrefab, canvasRect);
        RectTransform flyerRect = flyer.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            uiCamera,
            out localPoint
        );

        flyerRect.anchoredPosition = localPoint;

        FlyingIcon flyerIcon = flyer.GetComponent<FlyingIcon>();
        flyerIcon.Setup(VentSprite, CaseFileManager.Instance.AnimatedItemLocation); 

        VisibilityCheck();
    }

    private void HandleToolUpdated(EquipmentType type)
    {
        _lastToolType = type;
        if (type == EquipmentType.Observe)
        {
            VisibilityCheck();
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleHourEntered(int h)
    {
        if (_lastToolType == EquipmentType.Observe) VisibilityCheck();
    }

    private void VisibilityCheck()
    {
        // only show box as observable when info not known
        
        bool known = PlayerKnowledgeState.IsKnownID(ConnectorID);

        if (!known)
        {
            gameObject.SetActive(true);
            return;
        }

        gameObject.SetActive(false);
    }   
}
