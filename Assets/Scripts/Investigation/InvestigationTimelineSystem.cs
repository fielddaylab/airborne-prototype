using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationTimelineSystem : MonoBehaviour
{
    public static InvestigationTimelineSystem Instance;
    public PlayerInvestigationTimeline UITimeline;
    public ScenarioDataObject ScenarioData;
    public InvestigationRegistry InvestigationRegistry;

    public static event Action<int> OnHourUpdated;
    
    public int BaseHour = 13; // default to 1PM based on scenario tables
    [HideInInspector] public int CurrentHour = 0;
    public int TotalHours = 9;
    public float TimelineSpeed = 1;
    private float _trueTime = 0;

    public void Start()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        
        CurrentHour = BaseHour;
        OnHourUpdated?.Invoke(CurrentHour);
    }

    public void Update()
    {
        _trueTime += TimelineSpeed * Time.deltaTime;
        
        _trueTime = _trueTime % TotalHours;

        UITimeline.TimelineSlider.value = _trueTime;

        int thisHour = Mathf.FloorToInt(_trueTime + BaseHour);

        if (thisHour != CurrentHour)
        {
            CurrentHour = thisHour;
            OnHourUpdated?.Invoke(CurrentHour);
            Debug.Log("Updated hour to: " + CurrentHour);
        }
    }
}
