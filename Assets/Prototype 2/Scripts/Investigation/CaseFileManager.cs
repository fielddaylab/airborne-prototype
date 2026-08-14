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
    public static bool CaseFileOpen;

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
        CaseFileOpen = !CaseFileOpen;
        CaseFilePanel.gameObject.SetActive(CaseFileOpen);

        FalseTimelineSlider.gameObject.SetActive(CaseFileOpen);
        FalseTimelineSlider.value = InvestigationTimelineSystem.Instance.CurrentHour - InvestigationTimelineSystem.Instance.BaseHour;

        TrueTimelineSlider.interactable = !CaseFileOpen;

        InvestigationTimelineSystem.Instance.PauseTime(CaseFileOpen);

        if (CaseFileOpen)
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
