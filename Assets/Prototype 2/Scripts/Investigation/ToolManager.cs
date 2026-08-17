using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ToolbarMode
{
    Investigation,
    Intervention
}

public class ToolManager : MonoBehaviour
{
    public GameObject ToolButtonPrefab;
    
    public ToolbarMode CurrentMode = ToolbarMode.Investigation;

    public EquipmentType SelectedTool = EquipmentType.None;

    public static event Action<EquipmentType> OnToolUpdated;

    public List<ToolButton> ToolButtons = new();

    List<EquipmentType> StarterTools = new List<EquipmentType>
    {
        EquipmentType.Observe,
        EquipmentType.Scan,
        EquipmentType.Meter
    };

    private Dictionary<Button, EquipmentType> ButtonTools;

    void Start()
    {
        LoadTools(StarterTools);

        ChangeTool(EquipmentType.None);
    }

    public void ClearTool()
    {
        ChangeTool(EquipmentType.None);
    }

    public void LoadTools(List<EquipmentType> toolsToLoad)
    {
        ToolButtons.Clear();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject toolObj = transform.GetChild(i).gameObject;
            ToolButton toolButton = toolObj.GetComponent<ToolButton>();
            toolButton.MyButton.onClick.RemoveAllListeners();

            Destroy(toolObj);
        }

        for (int i = 0; i < toolsToLoad.Count; i++)
        {
            EquipmentType tool = toolsToLoad[i];
            
            GameObject toolPrefab = Instantiate(ToolButtonPrefab);
            toolPrefab.transform.SetParent(transform, false);

            ToolButton button = toolPrefab.GetComponent<ToolButton>();
            button.Setup(tool);

            button.MyButton.onClick.AddListener(() => ChangeTool(tool));

            ToolButtons.Add(button);
        }
    }

    private void ChangeTool(EquipmentType tool)
    {
        Debug.Log("Changing tool to " + tool);
        
        if (SelectedTool != tool)
        {
            SelectedTool = tool;
        } else
        {
            SelectedTool = EquipmentType.None;
        }

        OnToolUpdated?.Invoke(SelectedTool);
    }

}
