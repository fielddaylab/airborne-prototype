using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    public static NewGameManager Instance;
    public static event Action<NewGamePhase> TriggerPhase;

    NewGamePhase CurrentPhase;

    public RoundStatistics Statistics;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Statistics = new RoundStatistics();
            CurrentPhase = NewGamePhase.Investigation;
        } else
        {
            Destroy(gameObject);
        }
    } 

    public void SwitchToPhase(NewGamePhase phase)
    {
        Debug.Log("Switching to phase " + phase);
        TriggerPhase?.Invoke(phase);
    }
}

public enum NewGamePhase
{
    Investigation,
    Transition,
    RescuePlanning,
    Intervention,
    PSA,
    Results
}