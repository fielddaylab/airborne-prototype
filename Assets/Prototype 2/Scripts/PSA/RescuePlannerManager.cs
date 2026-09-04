using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescuePlannerManager : MonoBehaviour
{
    public static RescuePlannerManager Instance;
    public ToolManager ToolManagerRef;
    
    public Transform MapParent;
    [HideInInspector] public InvestigationMap Map;

    public Button AdvanceButton;
    
    public List<EquipmentType> SelectedEquipment;
    public Slider FalseSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        AdvanceButton.interactable = false;
        AdvanceButton.onClick.AddListener(HandleAdvance);
    }

    public void Setup()
    {
        GameObject mapObj = Instantiate(InvestigationTimelineSystem.Instance.ScenarioData.MapObject);
        mapObj.transform.SetParent(MapParent, false);

        Map = mapObj.GetComponent<InvestigationMap>();
        Map.Setup(FalseSlider);
        Map.ForceUpdate();
    }

    public void AddEquipment(EquipmentType type)
    {
        SelectedEquipment.Add(type);
        AdvanceButton.interactable = true;
    }

    public void RemoveEquipment(EquipmentType type)
    {
        SelectedEquipment.Remove(type);
        if (SelectedEquipment.Count <= 0)
        {
            AdvanceButton.interactable = false;
        }
    }

    public void HandleAdvance()
    {
        gameObject.SetActive(false);
        ToolManagerRef.LoadTools(SelectedEquipment);
        NewGameManager.Instance.SwitchToPhase(NewGamePhase.Intervention);
    }
}
