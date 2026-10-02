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

    public enum FulfillmentResult { Canceled, Failed, Succeeded }

    private SidebarTheoryPiece.TheoryType _soughtType;
    private PollutantType _soughtPollutant;
    private FeatureType _soughtFeature;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        ScreenDimmer.onClick.AddListener(ScreenClick);
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

    public void HandleTimeSlot(int hour)
    {
        // FINAL SCAFFOLD
    }

    private void Succeeded()
    {
        OnFulfilledRequest?.Invoke(FulfillmentResult.Succeeded);
        EndTheory();
    }

    private void Failed()
    {
        OnFulfilledRequest?.Invoke(FulfillmentResult.Failed);
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


