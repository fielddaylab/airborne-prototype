using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TheoryComboType
{
    PollutantAndSource,
    PollutantAtSource,
    PollutantAndSymptom,
    PollutantAtSymptom
}

public class TheoryCombo : MonoBehaviour
{
    public TheoryComboType ComboType;
    public Button ComboButton;
    public bool ComboEnabled = false;
    private FeatureType _source;
    private PollutantType _pollutant;

    public GameObject ComboPopup;
    public Button ComboCloser;
    public GameObject UnassignedIndicator;



    public void Setup(FeatureType source, PollutantType pollutant) 
    {
        _source = source;
        _pollutant = pollutant;
        ComboEnabled = true;
        ComboButton.enabled = true;
        InvestigationTimelineChunk.OnValidSelected += HandleValidSelection;
    }

    void OnEnable()
    {
        ComboButton.onClick.AddListener(HandleShowTheory);
        ComboButton.enabled = ComboEnabled;

        if (ComboCloser != null)
        {
            ComboCloser.onClick.AddListener(HandleHideTheory);
        }
    }

    void OnDisable()
    {
        ComboButton.onClick.RemoveListener(HandleShowTheory);

        if (ComboCloser != null)
        {
            ComboCloser.onClick.RemoveListener(HandleHideTheory);
        }
    }

    private void HandleShowTheory()
    {
        ComboPopup.SetActive(true);

        switch (ComboType)
        {
            case TheoryComboType.PollutantAndSource:
                HandlePAndSCombo();
                break;
        }
    }

    private void HandleHideTheory()
    {
        ComboPopup.SetActive(false);
    }

    private void HandlePAndSCombo()
    {
        PlayerInvestigationTimeline.OnFeatureDetailRequested.Invoke(_source, _pollutant);
    }

    private void HandleValidSelection()
    {
        HandleHideTheory();
        UnassignedIndicator.SetActive(false);
        InvestigationTimelineChunk.OnValidSelected -= HandleValidSelection;
    }
}
