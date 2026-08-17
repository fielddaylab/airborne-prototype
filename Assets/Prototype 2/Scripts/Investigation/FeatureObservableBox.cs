using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureObservableBox : MonoBehaviour
{
    public KnowledgeType FeatureKnowledge;
    public FeatureType FeatureType;
    public RoomType FeatureRoom;
    private EquipmentType _lastToolType;

    public GameObject FlyingObject;

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
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        int baseHour = InvestigationTimelineSystem.Instance.BaseHour;
        int maxHour = baseHour + InvestigationTimelineSystem.Instance.TotalNumHours;

        for (int i = hour - 1; i <= hour + 1; i++)
        {
            if (i >= baseHour && i <= maxHour)
            {
                PlayerKnowledgeState.Discover(FeatureRoom, i, FeatureKnowledge); // learn 3 chunks of time locally

                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
                RectTransform canvasRect = CaseFileManager.Instance.AnimatedItemLocation.root as RectTransform;

                GameObject flyer = Instantiate(FlyingObject, canvasRect);
                RectTransform flyerRect = flyer.GetComponent<RectTransform>();

                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPoint,
                    Camera.main,
                    out localPoint
                );

                flyerRect.anchoredPosition = localPoint;

                FlyingIcon flyerIcon = flyer.GetComponent<FlyingIcon>();
                Sprite flyerSprite = InvestigationLookup.Instance.SourceImages.GetSprite(FeatureType);
                flyerIcon.Setup(flyerSprite, CaseFileManager.Instance.AnimatedItemLocation); 
            }
        }
        
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
        
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        bool knowsFeature = PlayerKnowledgeState.IsKnownHourly(FeatureRoom, hour, FeatureKnowledge);

        if (!knowsFeature)
        {
            gameObject.SetActive(true);
            return;
        }

        gameObject.SetActive(false);
    }  
}
