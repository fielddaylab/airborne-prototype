using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanFeatureObject : MonoBehaviour
{
    public InvestigationFeatureEventObject FanData;
    public float FanSpeed = 1f;
    public float FanSmoothing = 0.5f;
    private float _currentFanSpeed = 0;
    private bool _isOn;
    public Transform FanBlades;

    void OnEnable()
    {
        InvestigationTimelineSystem.OnHourEntered += CheckIfOn;
    }

    void OnDisable()
    {
        InvestigationTimelineSystem.OnHourEntered -= CheckIfOn;
    }

    private void CheckIfOn(int hour)
    {
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;
        FeatureEvent status = FanData.TimeSlots[index].FeatureEvent;
        _isOn = status == FeatureEvent.On;
    }

    private void Update()
    {
        float targetSpeed = _isOn? FanSpeed : 0;
        _currentFanSpeed = Mathf.Lerp(_currentFanSpeed, targetSpeed, FanSmoothing * Time.deltaTime); 
        if (Mathf.Abs(_currentFanSpeed) < 0.01f) _currentFanSpeed = 0;
        FanBlades.transform.Rotate(0, 0, _currentFanSpeed * Time.deltaTime);
    }
}
