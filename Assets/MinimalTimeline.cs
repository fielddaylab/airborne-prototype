using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinimalTimeline : MonoBehaviour
{
    [SerializeField] private TMP_Text[] Timestamps;
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
    }

    private void OnDisable()
    {
        PauseButton.onClick.RemoveAllListeners();
        FastForwardButton.onClick.RemoveAllListeners();
    }

    private void UpdateTimespeed(float speed)
    {
         InvestigationTimelineSystem.Instance.TimelineSpeed = speed;
    }
}
