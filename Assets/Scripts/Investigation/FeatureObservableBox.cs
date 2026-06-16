using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureObservableBox : MonoBehaviour
{
    public KnowledgeType FeatureKnowledge;
    public RoomType FeatureRoom;

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
        PlayerKnowledgeState.Discover(FeatureRoom, hour, FeatureKnowledge);
        
        VisibilityCheck();
    }

    private void HandleToolUpdated(ToolType type)
    {
        if (type == ToolType.Observe)
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
        VisibilityCheck();
    }

    private void VisibilityCheck()
    {
        // only show box as observable when info not known
        
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        bool knowsFeature = PlayerKnowledgeState.IsKnown(FeatureRoom, hour, FeatureKnowledge);

        if (!knowsFeature)
        {
            gameObject.SetActive(true);
            return;
        }

        gameObject.SetActive(false);
    }  
}
