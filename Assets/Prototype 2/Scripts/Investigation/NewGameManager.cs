using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    public static NewGameManager Instance;
    public static event Action<NewGamePhase> TriggerPhase;

    NewGamePhase CurrentPhase;
    public CaseFileManager CaseFile;
    public ToolManager ToolManager;
    public RescuePlannerManager RescuePlanner;
    public TimeSwitcher TimeSwitcher;

    public RoundStatistics Statistics;

    public PSAManager PSA;
    public ResultsManager Results;

    public FinalLoopTracker FinalLoopData;

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
        UpdatePhase(phase);
        TriggerPhase?.Invoke(phase);
    }

    private void UpdatePhase(NewGamePhase phase)
    {
        switch (phase)
        {
            case NewGamePhase.Investigation:
                break;
            case NewGamePhase.Transition:
                break;
            case NewGamePhase.RescuePlanning:
                
                CaseFile.HideCaseFileKeepTimeline();
                TimeSwitcher.SwitchTo(0);
                ToolManager.ClearTool();
                RescuePlanner.gameObject.SetActive(true);

                break;
            case NewGamePhase.Intervention:
                CaseFile.SetCaseFile(false);
                TimeSwitcher.SwitchTo(1);
                TimeSwitcher.gameObject.SetActive(false);
                break;

            case NewGamePhase.PSA:
                PSA.gameObject.SetActive(true);

                break;
            case NewGamePhase.Results:
                Results.gameObject.SetActive(true);
                Results.EvaluateResults(FinalLoopData);
                break;
        }
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