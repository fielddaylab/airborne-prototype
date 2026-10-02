using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarTheoryPiece : MonoBehaviour
{
    public Image BaseImage;
    public TMP_Text QuestionText;
    public Image ImageOverlay;

    public Sprite MinorSymptom;
    public Sprite MajorSymptom;

    public enum TheoryType { PresentWhenActive, PresentMinorSymptom, PresentMajorSymptom }

    public TheoryType PieceType;

    public Button SlotButton;

    public PollutantType TargetPollutant;
    public FeatureType TargetSource;

    public void Awake()
    {
        UpdateText("???");
        BaseImage.enabled = false;
        ImageOverlay.enabled = false;
        if (SlotButton != null) SlotButton.onClick.AddListener(HandleSlotClick);
        TheoryCompleterManager.OnLockout += HandleLockout;
    }

    public void UpdateText(string name)
    {
        switch (PieceType)
        {
            case TheoryType.PresentWhenActive:
                QuestionText.text = $"{name} present when source active?";
                break;
            case TheoryType.PresentMinorSymptom:
                QuestionText.text = $"{name} present for minor symptom?";
                break;
            case TheoryType.PresentMajorSymptom:
                QuestionText.text = $"{name} present for major symptom?";
                break;
        }
    }

    public void UpdateImage(Sprite sprite)
    {
        BaseImage.enabled = true;
        BaseImage.sprite = sprite;

        switch (PieceType)
        {
            case TheoryType.PresentWhenActive:
                BaseImage.sprite = sprite;
                break;
            case TheoryType.PresentMinorSymptom:
                BaseImage.sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(InvestigationTimelineSystem.Instance.ScenarioData.MainNpc);
                ImageOverlay.sprite = MinorSymptom;
                ImageOverlay.enabled = true;
                break;
            case TheoryType.PresentMajorSymptom:
                BaseImage.sprite = InvestigationLookup.Instance.CharacterMap.GetSprite(InvestigationTimelineSystem.Instance.ScenarioData.MainNpc);
                ImageOverlay.sprite = MajorSymptom;
                ImageOverlay.enabled = true;
                break;
        }
    }

    private void HandleSlotClick()
    {
        TheoryCompleterManager.Instance.RequestTimeSlot(PieceType, TargetPollutant, TargetSource);
        TheoryCompleterManager.OnFulfilledRequest += HandleFullfilled;
    }

    private void HandleFullfilled(TheoryCompleterManager.FulfillmentResult result)
    {
        TheoryCompleterManager.OnFulfilledRequest -= HandleFullfilled;
    }

    private void HandleLockout(bool locked)
    {
        if (SlotButton != null) SlotButton.interactable = !locked;
    }
}
