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
    public static Action OnToolUsed;

    public List<ToolButton> ToolButtons = new();

    public Sprite FullPip, UsePip, EmptyPip;

    public EquipmentMapObject equipmentMap;

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

        OnToolUsed += HandleToolUsed;
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

        ToolButton oldTool = ToolButtons.Find(button => button.ToolType == SelectedTool);
        if (oldTool != null && oldTool.UsedPips > 0 && oldTool.NumPips > 0) {
            oldTool.ToolPips[oldTool.UsedPips - 1].sprite = FullPip;
        }

        // backout if the tool can't support more pips
        ToolButton newTool = ToolButtons.Find(button => button.ToolType == tool);
        if (newTool != null && newTool.NumPips > 0 && newTool.UsedPips <= 0) {
            SelectedTool = EquipmentType.None;
            OnToolUpdated?.Invoke(SelectedTool);
            return;
        }
        
        if (SelectedTool != tool)
        {
            SelectedTool = tool;
        } 
        else
        {
            SelectedTool = EquipmentType.None;
        }

        if (newTool != null && newTool.UsedPips > 0 && newTool.NumPips > 0 && !EquipmentMapUtility.HasPipDialogue(equipmentMap, tool)) {
            newTool.ToolPips[newTool.UsedPips - 1].sprite = UsePip;
        }

        OnToolUpdated?.Invoke(SelectedTool);
    }

    private void HandleToolUsed()
    {
        Debug.Log("USED!");
        ToolButton currentTool = ToolButtons.Find(button => button.ToolType == SelectedTool);
        if (currentTool.UsedPips > 0 && currentTool.NumPips > 0) {
            currentTool.ToolPips[currentTool.UsedPips - 1].sprite = EmptyPip;
            currentTool.UsedPips -= 1;
        }

        if (currentTool.NumPips > 0 && currentTool.UsedPips <= 0)
        {
            ChangeTool(EquipmentType.None);
        }
    }

}
