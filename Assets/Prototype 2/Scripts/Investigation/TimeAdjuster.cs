using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAdjuster : MonoBehaviour
{
    [SerializeField] private float m_TargetTime;
    private Button m_Button;

    private void OnEnable()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(SetTime);
    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

    public void SetTime()
    {
        InvestigationTimelineSystem.Instance.TimelineSpeed = m_TargetTime;
    }
}