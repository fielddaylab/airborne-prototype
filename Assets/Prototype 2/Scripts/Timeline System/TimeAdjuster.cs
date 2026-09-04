using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAdjuster : MonoBehaviour
{
    [SerializeField] private float m_TargetTime;
    private Button m_Button;

    public void SetTime()
    {
        InvestigationTimelineSystem.Instance.TimelineSpeed = m_TargetTime;
    }
}