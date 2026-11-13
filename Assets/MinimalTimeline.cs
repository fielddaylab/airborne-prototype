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
    public Sprite PauseSprite, FastForwardSprite, RewindSprite, CaseFileSprite;
    public CaseFileManager CaseFile;
    bool _timelinePaused = false;

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
        PauseButton.onClick.AddListener(() => HandlePause(0));
        FastForwardButton.onClick.AddListener(() => HandleFastForward(1f));
        InvestigationTimelineSystem.OnHourEntered += HandleHourUpdated;
        InvestigationTimelineSystem.OnTimePaused += HandleTimePaused;
    }

    private void OnDisable()
    {
        PauseButton.onClick.RemoveAllListeners();
        FastForwardButton.onClick.RemoveAllListeners();
        InvestigationTimelineSystem.OnHourEntered -= HandleHourUpdated;
        InvestigationTimelineSystem.OnTimePaused -= HandleTimePaused;
    }

    private void HandlePause(float speed)
    {
        if (!_timelinePaused) InvestigationTimelineSystem.Instance.TimelineSpeed = speed;
        else InvestigationTimelineSystem.Instance.PauseTime(false);
    }

    private void HandleFastForward(float speed)
    {
        if (!_timelinePaused) InvestigationTimelineSystem.Instance.TimelineSpeed = speed;
        else CaseFile.ToggleCaseFile();
    }

    private void HandleHourUpdated(int hour)
    {
        LoopCounter.text = "Loop " + (NewGameManager.Instance.Statistics.NumLoops + 1);
    }

    private void HandleTimePaused(bool paused)
    {
        _timelinePaused = paused;
        if (paused)
        {
            PauseButton.image.sprite = RewindSprite;
            FastForwardButton.image.sprite = CaseFileSprite;
        } 
        else
        {
            PauseButton.image.sprite = PauseSprite;
            FastForwardButton.image.sprite = FastForwardSprite;
        }
    }
}
