using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationMap : MonoBehaviour
{
    public MapRoomDisplay[] MapRooms;
    public MapConnector[] MapConnectors;
    public Button[] OverlayButtons;
    public PollutantType[] OverlayButtonPollutants;
    private int _startButton = 0;

    public Slider FalseSlider;

    private PollutantType _selectedPollutant = PollutantType.CO2;

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

        SwitchTo(_startButton);
    }

    public void OnEnable()
    {
        FalseSlider.onValueChanged.AddListener(UpdateRooms);
        
        for (int i = 0; i < OverlayButtons.Length; i++) 
        {
            int index = i; 
            OverlayButtons[i].onClick.AddListener(() => SwitchTo(index));
        }
    }

    public void OnDisable()
    {
        FalseSlider.onValueChanged.RemoveListener(UpdateRooms);

        foreach (var button in OverlayButtons)
        {
            button.onClick.RemoveAllListeners();
        }
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
                room.UpdateDisplay(hour, _selectedPollutant);
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

    public void SwitchTo(int t)
    {
        for (int i = 0; i < OverlayButtons.Length; i++)
        {
            if (t == i) 
            { 
                _selectedPollutant = OverlayButtonPollutants[i];
                OverlayButtons[i].interactable = false;
            } else
            {
                OverlayButtons[i].interactable = true;
            }
        }

        UpdateRooms(FalseSlider.value);
    }
}
