using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableEquipmentManager : MonoBehaviour
{
    public Vector3 HiddenLocation = new Vector3(0, -100, 0);
    public static Action<Vector3, InvestigationRoom> OnShowMeter;

    public Button YesButton, NoButton;
    public TMP_Text EquipmentDialogue;
    public GameObject WorldObject;

    public ToolManager ToolsManager;

    private InvestigationRoom _sourceRoom;
    public Sprite FullPip, UsePip, EmptyPip;

    public EquipmentMapObject EquipmentMap;
    private EquipmentType _currentTool;
    
    private bool _dialogueEnabled = false;

    public void Start()
    {
        HideDialogue();
    }

    public void OnEnable()
    {
        ToolManager.OnToolUpdated += HandleToolSelected;

        NoButton.onClick.AddListener(HideDialogue);
        YesButton.onClick.AddListener(PlaceObject);

        OnShowMeter += ShowDialogue;
    }

    public void OnDisable()
    {
        ToolManager.OnToolUpdated -= HandleToolSelected;

        NoButton.onClick.RemoveAllListeners();
        YesButton.onClick.RemoveAllListeners();

        OnShowMeter -= ShowDialogue;
    }

    public void HandleToolSelected(EquipmentType tool)
    {
        _dialogueEnabled = (EquipmentMapUtility.HasPipDialogue(EquipmentMap, tool) && tool != EquipmentType.Meter); 
        _currentTool = tool;  
    }

    public void ShowDialogue(Vector3 position, InvestigationRoom sourceRoom)
    {
        if (!_dialogueEnabled) return;

        ToolButton ToolGUI = ToolsManager.ToolButtons.Find(button => button.ToolType == _currentTool);
        ToolGUI.ToolPips[ToolGUI.UsedPips - 1].sprite = UsePip;

        transform.position = position;
        _sourceRoom = sourceRoom;

        string label = EquipmentMapUtility.GetLabel(EquipmentMap, _currentTool);

        EquipmentDialogue.text = $"Place {_currentTool}?";
    }

    public void HideDialogue()
    {
        transform.position = HiddenLocation;

        ToolButton ToolGUI = ToolsManager.ToolButtons.Find(button => button.ToolType == _currentTool);
        if (ToolGUI == null) return;
        ToolGUI.ToolPips[ToolGUI.UsedPips - 1].sprite = FullPip;
    }

    public void PlaceObject()
    {
        GameObject obj = Instantiate(WorldObject);
        obj.transform.position = transform.position;

        GenericObject gen = obj.GetComponent<GenericObject>();
        gen.spriteRenderer.sprite = EquipmentMapUtility.GetSprite(EquipmentMap, _currentTool);

        ToolManager.OnToolUsed?.Invoke();

        HideDialogue();

        if (_currentTool == EquipmentType.Filter)
        {
            NewGameManager.Instance.FinalLoopData.FilterPlacement = _sourceRoom.RoomTypeValue;
        } else if (_currentTool == EquipmentType.Fan)
        {
            NewGameManager.Instance.FinalLoopData.PlacedFans.Add(_sourceRoom.RoomTypeValue);
        } else if (_currentTool == EquipmentType.Cleaner)
        {
            NewGameManager.Instance.FinalLoopData.CleanerPlacement = _sourceRoom.RoomTypeValue;
        }

        ToolsManager.ClearTool();
    }
}
