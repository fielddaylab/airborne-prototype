using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookTimeline : MonoBehaviour
{
    [SerializeField] private Image FocusIcon;
    [SerializeField] private TMP_Text FocusTitle;
    [SerializeField] private NotebookTimeSlot[] TimeSlots;

    private void Start()
    {
        int startHour = InvestigationTimelineSystem.Instance.BaseHour;
        for (int i = 0; i < TimeSlots.Length; i++)
        {
            TimeSlots[i].Init(startHour + i);
        }
    }
}
