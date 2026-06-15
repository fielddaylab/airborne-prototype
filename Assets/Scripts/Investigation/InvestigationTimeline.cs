using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationTimeline : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_RoomText;
    public Slider TimelineSlider;

    private void Start()
    {
        InvestigationRoom.OnRoomUpdated += HandleRoomUpdated;
    }

    public void OnDestroy()
    {
        InvestigationRoom.OnRoomUpdated -= HandleRoomUpdated;
    }

    private void HandleRoomUpdated(InvestigationRoom room)
    {
        m_RoomText.text = room.RoomName;
    }
}
