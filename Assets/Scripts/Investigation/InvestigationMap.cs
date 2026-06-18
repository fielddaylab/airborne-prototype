using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationMap : MonoBehaviour
{
    public MapRoomDisplay[] MapRooms;
    public Slider FalseSlider;

    public void OnEnable()
    {
        FalseSlider.onValueChanged.AddListener(UpdateRooms);
    }

    public void OnDisable()
    {
        FalseSlider.onValueChanged.AddListener(UpdateRooms);
    }

    public void UpdateRooms(float f)
    {
        int sliderVal = Mathf.FloorToInt(f);
        int hour = InvestigationTimelineSystem.Instance.BaseHour + sliderVal;
        
        foreach (var room in MapRooms)
        {
            room.UpdateDisplay(hour);
        }
    }
}
