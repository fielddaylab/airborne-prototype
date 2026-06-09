using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorBlock : MonoBehaviour
{
    private Button _button;
    public Pollutant PollutantType;
    public int Concentration;
    
    public void Start()
    {
        _button = GetComponent<Button>();
    }

    public void OnEnable()
    {
        SlotEvent.OnUnlockSensorBlocks += UnlockButton;
        SlotEvent.OnLockSensorBlocks += LockButton;
    }

    public void OnDisable()
    {
        SlotEvent.OnUnlockSensorBlocks -= UnlockButton;
        SlotEvent.OnLockSensorBlocks -= LockButton;
    }

    private void UnlockButton()
    {
        _button.interactable = true;
    }

    public void OnSelected()
    {
        SlotEvent.SelectSensorBlock(this);
    }

    private void LockButton()
    {
        _button.interactable = false;
    }
}
