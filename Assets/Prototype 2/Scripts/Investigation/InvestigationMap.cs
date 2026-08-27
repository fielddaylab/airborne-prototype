using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationMap : MonoBehaviour
{
    public MapRoomDisplay[] MapRooms;
    public MapConnector[] MapConnectors;
    //private int _startButton = 0;
    public GasOverlayManager GasOverlayManager;

    public Slider FalseSlider;

    private PollutantType _selectedPollutant;

    public static Action<PollutantType> OnSetPollutant;

    public void Start()
    {
        InitializeDisplay();
    }

    public void OnEnable()
    {
        FalseSlider.onValueChanged.AddListener(UpdateRooms);
        OnSetPollutant += HandleSetPollutant;

        StartCoroutine(InitializeAfterFrame());
    }

    public void HandleSetPollutant(PollutantType pollutant)
    {
        _selectedPollutant = pollutant;
        UpdateRooms(FalseSlider.value);
    }

    public void OnDisable()
    {
        FalseSlider.onValueChanged.RemoveListener(UpdateRooms);
    }

    private IEnumerator InitializeAfterFrame()
    {
        yield return null;
        InitializeDisplay();
    }

    private void InitializeDisplay()
    {
        if (MapRooms == null || MapConnectors == null)
        {
            return;
        }

        foreach (var room in MapRooms)
        {
            if (room != null)
            {
                room.gameObject.SetActive(false);
            }
        }
        foreach (var connector in MapConnectors)
        {
            if (connector != null)
            {
                connector.gameObject.SetActive(false);
            }
        }

        if (FalseSlider != null)
        {
            UpdateRooms(FalseSlider.value);
        }
    }

    public void SetOverlayForPollutant(PollutantType pollutant)
    {
        GasOverlayManager.HandleOverlayChange(pollutant);
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

    // find the room with the source in it, and enable it only if the pollutant is in there too
    public void SetupSourceSelector(int hour, PollutantType pollutant, FeatureType source)
    {
        foreach (var room in MapRooms)
        {
            if (PlayerKnowledgeState.IsKnownGenerally(room.roomType, KnowledgeType.RoomInfo))
            {
                room.UpdateSourceValidity(hour, pollutant, source);
            }
        }
    }

    // find the room with the npc in it, and enable if only if the pollutant is in there too
    public void SetupSymptomSelector(int hour, PollutantType pollutant)
    {
        foreach (var room in MapRooms)
        {
            if (PlayerKnowledgeState.IsKnownGenerally(room.roomType, KnowledgeType.RoomInfo))
            {
                room.UpdateSymptomValidity(hour, pollutant);
            }
        }
    }
}
