using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableBox : MonoBehaviour
{
    public KnowledgeType FeatureKnowledge;
    public RoomType FeatureRoom;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    private void OnMouseDown()
    {
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        PlayerKnowledgeState.Discover(FeatureRoom, hour, FeatureKnowledge);
        Debug.Log("Recorded information!");
    }

    private void HandleToolUpdated(ToolType type)
    {
        if (type == ToolType.Observe) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
