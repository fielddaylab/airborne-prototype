using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    // where the object will go when not in use
    public Vector3 HiddenLocation = new Vector3(0, -100, 0);
    public static Action<Vector3, InvestigationRoom> OnShowMeter;

    public Button XButton;

    public GameObject MeterButtonPrefab;
    public Transform MeterButtonParent;

    public GameObject MeterObject;

    // public int numMeters = 4;

    public ToolManager ToolsManager;

    private InvestigationRoom _sourceRoom;

    public static Action<PollutantType> OnMeterButton;

    //public Image[] meterPips;
    public Sprite FullPip, UsePip, EmptyPip;

    void Start()
    {
        Setup();
        
        HideDialogue();
    }

    private void Setup()
    {
        PollutantDataObject[] pollutantDatas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;

        for (int i = 0; i < MeterButtonParent.transform.childCount; i++)
        {
            Destroy(MeterButtonParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < pollutantDatas.Length; i++)
        {
            GameObject meterButtonObj = Instantiate(MeterButtonPrefab);
            meterButtonObj.transform.SetParent(MeterButtonParent);

            MeterButton meterButton = meterButtonObj.GetComponent<MeterButton>();
            meterButton.Setup(pollutantDatas[i].Type);
        }
    }

    void OnEnable()
    {
        XButton.onClick.AddListener(HideDialogue);
        OnShowMeter += ShowDialogue;

        OnMeterButton += PlaceMeter;

        ToolManager.OnToolUpdated += HandleToolUpdated;
    }

    private void HandleToolUpdated(EquipmentType type)
    {
        if (type != EquipmentType.Meter) HideDialogue();
    }

    void OnDisable()
    {
        XButton.onClick.RemoveAllListeners();
        OnShowMeter -= ShowDialogue;

        OnMeterButton -= PlaceMeter;

        ToolManager.OnToolUpdated -= HandleToolUpdated;
    }

    public void HideDialogue()
    {
        transform.position = HiddenLocation;
        //if (numMeters <= 0) return;

        ToolButton ToolGUI = ToolsManager.ToolButtons.Find(button => button.ToolType == EquipmentType.Meter);
        if (ToolGUI == null)
        {
            return;
        }

        int targetPip = ToolGUI.UsedPips - 1;
        if (targetPip >= 0) ToolGUI.ToolPips[ToolGUI.UsedPips - 1].sprite = FullPip;
    }

    public void ShowDialogue(Vector3 position, InvestigationRoom sourceRoom)
    {
        if (ToolsManager.SelectedTool != EquipmentType.Meter) return;
        
        //if (numMeters <= 0) return;
        if (sourceRoom.NumMeters >= 2) return;
        
        ToolButton ToolGUI = ToolsManager.ToolButtons.Find(button => button.ToolType == EquipmentType.Meter);
        ToolGUI.ToolPips[ToolGUI.UsedPips - 1].sprite = UsePip;

        transform.position = position;
        _sourceRoom = sourceRoom;
    }

    public void PlaceMeter(PollutantType pollutantType)
    {
        //if (numMeters <= 0) return;
        
        // ToolButton ToolGUI = ToolsManager.ToolButtons.Find(button => button.ToolType == EquipmentType.Meter);
        // ToolGUI.ToolPips[numMeters - 1].sprite = EmptyPip;
        //numMeters--;
        _sourceRoom.NumMeters++;
        
        GameObject meter = Instantiate(MeterObject);
        meter.transform.position = transform.position;
        
        GasMeter gasMeter = meter.GetComponent<GasMeter>();

        gasMeter.TrackedRoom = _sourceRoom;
        gasMeter.TrackedPollutant = pollutantType;
        gasMeter.Label.text = pollutantType.ToString();
        //gasMeter.Label.color = InvestigationLookup.Instance.PollutantMap.GetMaterial(pollutantType);

        InvestigationTimelineSystem.Instance.RegisterMeter(gasMeter);

        ToolManager.OnToolUsed?.Invoke();

        HideDialogue();

        ToolsManager.ClearTool();
    }
}
