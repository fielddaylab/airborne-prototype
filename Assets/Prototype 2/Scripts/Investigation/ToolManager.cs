using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ToolType
{
    None,
    Observe,
    Scan,
    Meter
}

public class ToolManager : MonoBehaviour
{
    public Button ObserveButton, ScannerButton, MeterButton;
    public ToolType SelectedTool = ToolType.None;

    public static event Action<ToolType> OnToolUpdated;

    void Start()
    {
        ObserveButton.onClick.AddListener(() => ChangeTool(ToolType.Observe));
        ScannerButton.onClick.AddListener(() => ChangeTool(ToolType.Scan));
        MeterButton.onClick.AddListener(() => ChangeTool(ToolType.Meter));

        ChangeTool(ToolType.None);
    }

    private void ChangeTool(ToolType tool)
    {
        if (SelectedTool != tool)
        {
            SelectedTool = tool;
        } else
        {
            SelectedTool = ToolType.None;
        }

        OnToolUpdated?.Invoke(SelectedTool);
    }

}
