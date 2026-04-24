using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCreator : MonoBehaviour
{
    [HideInInspector] public HighlighterSlot SelectedSlot;
    [HideInInspector] public HighlighterSlot FilledSlot;
    [SerializeField] private GameObject _symptomSlot, _sourceSlot, _dialogueSlot;
    [SerializeField] private RawImage _sensorSlot;
    [SerializeField] private GameObject _cardPrefab, _cardParent;

    private void OnEnable()
    {
        SlotEvent.OnSlotSelected += HandleSlotSelection;
        SlotEvent.OnSensorBlockSelected += FillSensorBlock;
    }

    private void OnDisable()
    {
        SlotEvent.OnSlotSelected -= HandleSlotSelection;
        SlotEvent.OnSensorBlockSelected -= FillSensorBlock;
    }

    private void HandleSlotSelection(HighlighterSlot newSlot)
    {
        if (newSlot.IsSlot)
        {
            FilledSlot = newSlot;
            if (SelectedSlot != null) UpdateSlot();
        }
        else
        {
            SelectedSlot = newSlot;
            if (FilledSlot != null) UpdateSlot();
        }
    }

    private void UpdateSlot()
    {
        FilledSlot.SlotImage.texture = SelectedSlot.SlotImage.texture;
        FilledSlot.SlotImage.enabled = true;

        switch (FilledSlot.SlotType)
        {
            case SlotType.Symptom:
                _sourceSlot.SetActive(false);
                _dialogueSlot.SetActive(false);
                break;
            case SlotType.Source:
                _symptomSlot.SetActive(false);
                _dialogueSlot.SetActive(false);
                break;
            case SlotType.Dialogue:
                _symptomSlot.SetActive(false);
                _sourceSlot.SetActive(false);
                break;
        }

        SlotEvent.ClearHighlights();
        SlotEvent.Locked = true;
        SlotEvent.UnlockSensorBlocks();
    }

    private void FillSensorBlock(SensorBlock sensorBlock)
    {
        CreateCombo(sensorBlock);
    }

    private void CreateCombo(SensorBlock sensorBlock)
    {
        int persuasion = 1;
        
        GameObject cardPrefab = Instantiate(_cardPrefab);
        cardPrefab.transform.SetParent(_cardParent.transform);
        cardPrefab.transform.localScale = Vector3.one;

        ComboCard comboCard = cardPrefab.GetComponent<ComboCard>();
        comboCard.Setup(persuasion, SelectedSlot.SlotImage.texture, sensorBlock.GetComponent<RawImage>().texture);

        PersuasionManager.Instance.UpdateAmount(persuasion);

        ResetSlots();
    }

    private void ResetSlots()
    {
        _sourceSlot.SetActive(true);
        _dialogueSlot.SetActive(true);
        _symptomSlot.SetActive(true);

        FilledSlot.SlotImage.enabled = false;

        FilledSlot = null;
        SelectedSlot = null;

        SlotEvent.Locked = false;
        SlotEvent.LockSensorBlocks();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left mouse button pressed.");
        }
    }
}
