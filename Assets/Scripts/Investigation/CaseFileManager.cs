using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseFileManager : MonoBehaviour
{
    public Button CaseFileButton;
    public GameObject CaseFilePanel;
    public Slider FalseTimelineSlider;
    public Slider TrueTimelineSlider;
    public InvestigationMap Map;
    public InvestigationPollutantsManager Pollutants;
    private bool _caseFileOpen;

    public static event Action OnCaseFileClosed;

    public void Start()
    {
        CaseFilePanel.SetActive(false);
    }

    public void OnEnable()
    {
        CaseFileButton.onClick.AddListener(ToggleCaseFile);
    }

    public void OnDisable()
    {
        CaseFileButton.onClick.RemoveListener(ToggleCaseFile);
    }

    public void ToggleCaseFile()
    {
        _caseFileOpen = !_caseFileOpen;
        CaseFilePanel.gameObject.SetActive(_caseFileOpen);

        FalseTimelineSlider.gameObject.SetActive(_caseFileOpen);
        FalseTimelineSlider.value = InvestigationTimelineSystem.Instance.CurrentHour - InvestigationTimelineSystem.Instance.BaseHour;

        TrueTimelineSlider.interactable = !_caseFileOpen;

        InvestigationTimelineSystem.Instance.PauseTime(_caseFileOpen);

        if (_caseFileOpen)
        {
            Map.UpdateRooms(TrueTimelineSlider.value);
            Pollutants.UpdateInformation();
        } else
        {
            OnCaseFileClosed?.Invoke();
            PlayerInvestigationTimeline.OnResetRequested.Invoke();
        }
    }
}
