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

    public Image Check;

    private bool _ignoreLocks = false;

    public SidebarTheoryManager MyManager;

    public Image TopStatus, BottomStatus;

    public Sprite Valid, Fail, Unknown;

    public void Awake()
    {
        UpdateText("???");
        BaseImage.enabled = false;
        ImageOverlay.enabled = false;
        if (SlotButton != null) SlotButton.onClick.AddListener(HandleSlotClick);
        TheoryCompleterManager.OnLockout += HandleLockout;
        Check.gameObject.SetActive(false);
        TopStatus.enabled = false;
        BottomStatus.enabled = false;
    }

    public void Reset()
    {
        SlotButton.interactable = true;
        _ignoreLocks = false;
        Check.gameObject.SetActive(false);
        if (MyManager != null) MyManager.IncrementEvidence(-1);
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

        if (result == TheoryCompleterManager.FulfillmentResult.Succeeded)
        {
            SlotButton.interactable = false;
            _ignoreLocks = true;
            Check.gameObject.SetActive(true);
            MyManager.IncrementEvidence(1);
        } 
        else
        {
            StartCoroutine(FlashFail(result));
        }
    }

    IEnumerator FlashFail(TheoryCompleterManager.FulfillmentResult result)
    {
        Time.timeScale = 0;
        
        switch (result)
        {
            case TheoryCompleterManager.FulfillmentResult.BothFailed:
                TopStatus.sprite = Fail;
                BottomStatus.sprite = Fail;
                break;
            case TheoryCompleterManager.FulfillmentResult.TopFailed:
                TopStatus.sprite = Fail;
                BottomStatus.sprite = Valid;
                break;
            case TheoryCompleterManager.FulfillmentResult.BottomFailed:
                TopStatus.sprite = Valid;
                BottomStatus.sprite = Fail;
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            TopStatus.enabled = true;
            BottomStatus.enabled = true;

            yield return new WaitForSecondsRealtime(0.33f);

            TopStatus.enabled = false;
            BottomStatus.enabled = false;

            yield return new WaitForSecondsRealtime(0.33f);
        }

        Time.timeScale = 1;
    }

    private void HandleLockout(bool locked)
    {
        if (SlotButton != null && !_ignoreLocks) SlotButton.interactable = !locked;
    }
}
