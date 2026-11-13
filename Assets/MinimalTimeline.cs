using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinimalTimeline : MonoBehaviour
{
    [SerializeField] private TMP_Text[] Timestamps;
    [SerializeField] private TMP_Text LoopCounter;
    public Slider TimelineSlider;
    [SerializeField] private Button PauseButton, FastForwardButton;

    private void Start()
    {
        int startHour = InvestigationTimelineSystem.Instance.BaseHour;
        for (int i = 0; i < Timestamps.Length; i++)
        {
            Timestamps[i].text = (startHour + i) % 12 + ":00";
        }
    }

    private void OnEnable()
    {
        PauseButton.onClick.AddListener(() => UpdateTimespeed(0));
        FastForwardButton.onClick.AddListener(() => UpdateTimespeed(1f));
        InvestigationTimelineSystem.OnHourEntered += HandleHourUpdated;
    }

    private void OnDisable()
    {
        PauseButton.onClick.RemoveAllListeners();
        FastForwardButton.onClick.RemoveAllListeners();
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
    }

    private void UpdateTimespeed(float speed)
    {
        InvestigationTimelineSystem.Instance.TimelineSpeed = speed;
    }

    private void HandleHourUpdated(int hour)
    {
        LoopCounter.text = "Loop " + (NewGameManager.Instance.Statistics.NumLoops + 1);
    }
}
