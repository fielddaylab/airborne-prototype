using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class TheoryCompleterManager : MonoBehaviour
{
    public static TheoryCompleterManager Instance;

    public Button ScreenDimmer;

    public static event Action<FulfillmentResult> OnFulfilledRequest;
    public static event Action<bool> OnLockout;

    public enum FulfillmentResult { Canceled, TopFailed, BottomFailed, BothFailed, Succeeded }

    private SidebarTheoryPiece.TheoryType _soughtType;
    private PollutantType _soughtPollutant;
    private FeatureType _soughtFeature;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        ScreenDimmer.onClick.AddListener(ScreenClick);
        InvestigationTimelineChunk.OnSlotSelected += HandleTimeSlot;
    }

    void Start()
    {
        ScreenDimmer.gameObject.SetActive(false);
    }

    public void RequestTimeSlot(SidebarTheoryPiece.TheoryType theoryType, PollutantType pollutant, FeatureType source)
    {
        OnLockout?.Invoke(true);
        ScreenDimmer.gameObject.SetActive(true);
        InvestigationTimelineSystem.Instance.PauseTime(true);
        _soughtType = theoryType;
        _soughtPollutant = pollutant;
        _soughtFeature = source;
    }

    private void HandleTimeSlot(TimelineData? data)
    {
        if (!(data is TimelineData d))
        {
            Failed(FulfillmentResult.BothFailed);
            return;
        }

        bool topFailed = false;
        bool bottomFailed = false;

        // pollution checks
        if (!d.Concentration.HasValue || d.Concentration <= 0)
        {
            bottomFailed = true;
        }
        if (!d.Pollutant.HasValue || d.Pollutant != _soughtPollutant)
        {
            bottomFailed = true;
        }

        CharacterType mainChar = InvestigationTimelineSystem.Instance.ScenarioData.MainNpc;

        // checks based on chars
        switch (_soughtType)
        {
            case SidebarTheoryPiece.TheoryType.PresentWhenActive:
                if (d.Features == null)
                {
                    topFailed = true;
                    break;
                }
                
                bool foundValidFeature = false;
                foreach (var f in d.Features)
                {
                    if (f == _soughtFeature)
                    {
                        // check if on or off, if yes foundSomething;
                        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;
                        
                    }
                }
                topFailed = foundValidFeature;

                break;
            case SidebarTheoryPiece.TheoryType.PresentMinorSymptom:

                if (d.Characters == null)
                {
                    topFailed = true;
                    break;
                }

                bool foundValidCharacter = false;
                foreach (var c in d.Characters)
                {
                    if (c == mainChar) foundValidCharacter = true;
                }
                if (!foundValidCharacter) { topFailed = true; break; }

                bool foundValidSymptom = false;
                foreach (var s in d.Symptoms)
                {
                    if (s != Symptom.LossConsciousness) { foundValidSymptom = true; break; }
                }
                if (!foundValidSymptom) { topFailed = true; break; }

                break;
            case SidebarTheoryPiece.TheoryType.PresentMajorSymptom:
                
                if (d.Characters == null)
                {
                    topFailed = true;
                    break;
                }

                bool foundValidCharacter1 = false;
                foreach (var c in d.Characters)
                {
                    if (c == mainChar) foundValidCharacter1 = true;
                }
                if (!foundValidCharacter1) { topFailed = true; break; }

                bool foundValidSymptom1 = false;
                foreach (var s in d.Symptoms)
                {
                    if (s == Symptom.LossConsciousness) { foundValidSymptom1 = true; break; }
                }
                if (!foundValidSymptom1) { topFailed = true; break; }

                break;
        }

        if (topFailed && bottomFailed) { Failed(FulfillmentResult.BottomFailed); return; }
        else if (topFailed) { Failed(FulfillmentResult.TopFailed); return; }
        else if (bottomFailed) { Failed(FulfillmentResult.BottomFailed); return; }
        
        Succeeded();
    }

    private void Succeeded()
    {
        OnFulfilledRequest?.Invoke(FulfillmentResult.Succeeded);
        EndTheory();
    }

    private void Failed(FulfillmentResult reason)
    {
        OnFulfilledRequest?.Invoke(reason);
        EndTheory();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnFulfilledRequest?.Invoke(FulfillmentResult.Canceled);
            EndTheory();
        }
    }

    private void ScreenClick()
    {
        OnFulfilledRequest?.Invoke(FulfillmentResult.Canceled);
        EndTheory();
    }

    public void EndTheory()
    {
        OnLockout?.Invoke(false);
        ScreenDimmer.gameObject.SetActive(false);
        InvestigationTimelineSystem.Instance.PauseTime(false);
    }
}


