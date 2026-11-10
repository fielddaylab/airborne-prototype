using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookTimeSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TimestampText;
    [SerializeField] private Image m_SlotBG;
    [SerializeField] private Image m_EventIcon;
    [SerializeField] private TMP_Text m_BodyText;
    [SerializeField] private Image m_RoomIndicator;
    [SerializeField] private Image m_RoomIcon;

    [SerializeField] private Sprite EmptyOutline, FilledOutline;

    public void Init(int time)
    {
        m_TimestampText.text = time % 12 + ":00";
        Clear();
    }

    private void Clear()
    {
        m_EventIcon.enabled = false;
        m_BodyText.enabled = false;
        m_RoomIndicator.enabled = false;
        m_RoomIcon.enabled = false;

        m_SlotBG.sprite = EmptyOutline;
    }
}
