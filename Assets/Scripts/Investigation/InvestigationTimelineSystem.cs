using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationTimelineSystem : MonoBehaviour
{
    public static InvestigationTimelineSystem Instance;

    public static event Action<int> OnHourUpdated;
    
    public int CurrentHour = 0;
    public int TotalHours = 9;
    public float TimelineSpeed = 1;
    private float _trueTime = 0;

    public void Start()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        
        OnHourUpdated?.Invoke(CurrentHour);
    }

    public void Update()
    {
        _trueTime += TimelineSpeed * Time.deltaTime;
        
        _trueTime = _trueTime % TotalHours;

        int thisHour = Mathf.FloorToInt(_trueTime);

        if (thisHour != CurrentHour)
        {
            CurrentHour = thisHour;
            OnHourUpdated?.Invoke(CurrentHour);
        }
    }
}
