using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorObservableBox : MonoBehaviour
{
    public string ConnectorID;
    private EquipmentType _lastToolType;

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
