using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationMap : MonoBehaviour
{
    public MapRoomDisplay[] MapRooms;
    public MapConnector[] MapConnectors;

    public Slider FalseSlider;

    public void Start()
    {
        foreach (var room in MapRooms)
        {
            room.gameObject.SetActive(false);
        }
        foreach (var connector in MapConnectors)
        {
            connector.gameObject.SetActive(false);
        }

        UpdateRooms(FalseSlider.value);
    }

    public void OnEnable()
    {
        FalseSlider.onValueChanged.AddListener(UpdateRooms);
    }

    public void OnDisable()
    {
        FalseSlider.onValueChanged.RemoveListener(UpdateRooms);
    }

    public void UpdateRooms(float f)
    {
        int sliderVal = Mathf.FloorToInt(f);
        int hour = InvestigationTimelineSystem.Instance.BaseHour + sliderVal;
        
        List<RoomType> knownRooms = new();

        foreach (var room in MapRooms)
        {
            if (PlayerKnowledgeState.IsKnownGenerally(room.roomType, KnowledgeType.RoomInfo))
            {
                knownRooms.Add(room.roomType);
                room.gameObject.SetActive(true);
                room.UpdateDisplay(hour);
            }
        }

        foreach (var connector in MapConnectors)
        {
            if (knownRooms.Contains(connector.FirstRoom) && knownRooms.Contains(connector.SecondRoom))
            {
                if (!connector.IsVent) {
                    connector.gameObject.SetActive(true);
                } else
                {
                    if (PlayerKnowledgeState.IsKnownID(connector.ID))
                    {
                        connector.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
