using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvestigationTimeline : MonoBehaviour
{
    public static InvestigationTimeline Instance;

    [SerializeField] private TextMeshProUGUI m_RoomText;

    private void Start()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetRoom(InvestigationRoom room)
    {
        m_RoomText.text = room.RoomName;
    }
}
