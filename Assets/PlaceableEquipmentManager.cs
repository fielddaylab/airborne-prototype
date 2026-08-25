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
        YesButton.onClick.AddListener(PlaceMeter);

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

        ToolButton ToolGUI = ToolsManager.ToolButtons.Find(button => button.ToolType == EquipmentType.Meter);
        ToolGUI.ToolPips[ToolGUI.UsedPips - 1].sprite = FullPip;
    }

    public void PlaceMeter()
    {
        GameObject meter = Instantiate(WorldObject);
        meter.transform.position = transform.position;
        
        // GasMeter gasMeter = meter.GetComponent<GasMeter>();

        // gasMeter.TrackedRoom = _sourceRoom;
        // gasMeter.TrackedPollutant = pollutantType;
        // gasMeter.Label.text = pollutantType.ToString();

        // InvestigationTimelineSystem.Instance.RegisterMeter(gasMeter);

        ToolManager.OnToolUsed?.Invoke();

        HideDialogue();

        ToolsManager.ClearTool();
    }
}
