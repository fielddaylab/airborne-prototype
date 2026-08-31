using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterventionReplaceBox : MonoBehaviour
{
    public EquipmentType TargetEquipment;
    private bool _AlreadyReplaced = false;


    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        gameObject.SetActive(false);
    }

    private void HandleToolUpdated(EquipmentType type)
    {
        if (type == TargetEquipment && !_AlreadyReplaced)
        {
            gameObject.SetActive(true);
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        _AlreadyReplaced = true;
        gameObject.SetActive(false);
        ToolManager.OnToolUsed?.Invoke();
        // this will need updating as well for potentially replacing the model and tracking this state. TODO!

        if (TargetEquipment == EquipmentType.HeatPump) NewGameManager.Instance.FinalLoopData.ReplacedFurnace = true;
        if (TargetEquipment == EquipmentType.ElectricStove) NewGameManager.Instance.FinalLoopData.ReplacedStove = true;
    }
}
