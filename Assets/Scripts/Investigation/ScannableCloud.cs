using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableCloud : MonoBehaviour
{
    public int cloudIndex;
    public InvestigationRoom parentRoom;
    public MeshRenderer meshRenderer;
    private bool _nothingToRender = false;
    private bool _scannerActive = false;

    public void Start()
    {
        meshRenderer.enabled = false;
    }

    public void OnEnable()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        ScenarioEventDispatcher.OnPollutantUpdated += HandlePollutantUpdated;
    }

    public void OnDisable()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        ScenarioEventDispatcher.OnPollutantUpdated -= HandlePollutantUpdated;
    }

    private void HandleToolUpdated(ToolType type)
    {
        _scannerActive = type == ToolType.Scan;

        UpdateVisibility();
    }
    
    private void HandlePollutantUpdated(RoomType type, PollutantReading[] readings)
    {
        if (parentRoom.roomType == type)
        {
            if (readings.Length > cloudIndex)
            {
                PollutantType pollutantType = readings[cloudIndex].Pollutant;
                _nothingToRender = pollutantType == PollutantType.None;
                if (_nothingToRender)
                {
                    return;
                }

                // this is a mess and a half
                transform.localScale = Vector3.one * readings[cloudIndex].Concentration;
                meshRenderer.material = InvestigationTimelineSystem.Instance.InvestigationRegistry.PollutantMaterials.GetMaterial(pollutantType);
            } else
            {
                _nothingToRender = true;
            }
        }

        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        meshRenderer.enabled = _scannerActive && !_nothingToRender;
    }
}
