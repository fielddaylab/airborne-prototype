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
    public List<GasMeter> Meters;

    public static event Action<int> OnHourLeft;
    public static event Action<int> OnHourEntered;
    public static event Action OnTimeReset;
    public static event Action<bool> OnTimePaused;
    
    public int BaseHour = 13; // default to 1PM based on scenario tables
    [HideInInspector] public int CurrentHour = 0;
    public int TotalNumHours = 9;
    public float TimelineSpeed = 1;
    private float _trueTime = 0;
    public bool IsPaused { get; private set; }
    private bool _isFinalLoop = false;

    public Dictionary<(RoomType, int), RoomTimeSlot> TimeSlotLookup = new();

    public void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Start()
    {
        NewGameManager.TriggerPhase += HandlePhaseChange;
        
        CurrentHour = BaseHour;
        OnHourEntered?.Invoke(CurrentHour);

        foreach (var room in ScenarioData.Rooms)
        {
            foreach (var slot in room.TimeSlots)
            {
                TimeSlotLookup[(room.RoomTypeValue, slot.Time)] = slot;
            }
        }
    }

    public void SetTime(int indexHour) // 0 for start
    {
        
    }

    public RoomTimeSlot GetTimeSlot(RoomType room, int hour)
    {
        TimeSlotLookup.TryGetValue((room, hour), out var slot);
        return slot;
    }

    public void Update()
    {
        if (IsPaused) return;
        
        _trueTime += TimelineSpeed * Time.deltaTime;
        
        float previous = _trueTime;
        _trueTime = _trueTime % TotalNumHours;
        if (_trueTime < previous)
        {
            OnTimeReset?.Invoke();
            NewGameManager.Instance.Statistics.NumLoops++;

            if (_isFinalLoop)
            {
                IsPaused = true;
                NewGameManager.Instance.SwitchToPhase(NewGamePhase.PSA);
            }
        }

        UITimeline.TimelineSlider.value = _trueTime;

        int thisHour = Mathf.FloorToInt(_trueTime + BaseHour);

        if (thisHour != CurrentHour)
        {
            OnHourLeft?.Invoke(CurrentHour);
            CurrentHour = thisHour;
            OnHourEntered?.Invoke(CurrentHour);
            Debug.Log("Updated hour to: " + CurrentHour);
        }
    }

    public void PauseTime(bool pause)
    {
        IsPaused = pause;
        OnTimePaused?.Invoke(IsPaused);
    }

    public void RegisterMeter(GasMeter meter)
    {
        Meters.Add(meter); // just to use for the maps mostly
    }

    public void HandlePhaseChange(NewGamePhase phase)
    {
        if (phase == NewGamePhase.RescuePlanning)
        {
            OnTimeReset?.Invoke();
            _trueTime = 0;
        }
        
        if (phase == NewGamePhase.Intervention)
        {
            _isFinalLoop = true;
            _trueTime = 0;
            IsPaused = false;
        }
    }
}
