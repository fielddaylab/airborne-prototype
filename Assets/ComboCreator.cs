using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCreator : MonoBehaviour
{
    [HideInInspector] public HighlighterSlot SelectedSlot;
    [SerializeField] private GameObject _symptomSlot, _sourceSlot, _dialogueSlot;
    [SerializeField] private RawImage _sensorSlot;
    [SerializeField] private GameObject _cardPrefab, _cardParent;

    private void OnEnable()
    {
        SlotEvent.OnSlotSelected += HandleSlotSelection;
        //SlotEvent.OnClearHighlights += HandleClear;
        SlotEvent.OnSensorBlockSelected += FillSensorBlock;
    }

    private void OnDisable()
    {
        SlotEvent.OnSlotSelected -= HandleSlotSelection;
        //SlotEvent.OnClearHighlights -= HandleClear;
        SlotEvent.OnSensorBlockSelected -= FillSensorBlock;
    }

    private void HandleSlotSelection(HighlighterSlot highlighterSlot)
    {
        if (SelectedSlot == null)
        {
            SelectedSlot = highlighterSlot;
        } 
        else
        {
            if (SelectedSlot.SlotType == highlighterSlot.SlotType && SelectedSlot.IsSlot != highlighterSlot.IsSlot) 
            {
                Debug.Log("Slot it in!");
                if (SelectedSlot.IsSlot)
                {
                    SelectedSlot.SlotImage.texture = highlighterSlot.SlotImage.texture;
                } 
                else
                {
                    highlighterSlot.SlotImage.texture = SelectedSlot.SlotImage.texture;
                }

                switch (highlighterSlot.SlotType)
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
            } else
            {
                SelectedSlot = null;
            }
        }
    }

    private void FillSensorBlock(SensorBlock sensorBlock)
    {
        RawImage blockImage = sensorBlock.GetComponent<RawImage>();
        _sensorSlot.texture = blockImage.texture;

        CreateCombo(sensorBlock);
    }

    private void CreateCombo(SensorBlock sensorBlock)
    {
        int persuasion = 1; // have some calculation for this someday
        
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
