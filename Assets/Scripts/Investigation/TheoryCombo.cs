using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TheoryComboType
{
    PollutantAndSource,
    PollutantAtSource,
    PollutantAndSymptom,
    PollutantAtSymptom
}

public class TheoryCombo : MonoBehaviour
{
    public TheoryComboType ComboType;
    public Button ComboButton;
    public bool ComboEnabled = false;
    private FeatureType _source;
    private PollutantType _pollutant;

    public GameObject ComboPopup;
    public Button ComboCloser;
    public GameObject UnassignedIndicator;

    public SourceAndSymptomManager SymptomManager;

    public PollutantAtSourceManager PollutantAtSource;
    public PollutantAtSymptomManager PollutantAtSymptom;

    public InvestigationMap LockedMap;

    public void Start()
    {
        if (ComboPopup != null) {
            ComboPopup.SetActive(false);
        }
    }

    public void Reset()
    {
        ComboEnabled = false;
        UnassignedIndicator.SetActive(true);
    }

    public void Setup(FeatureType source, PollutantType pollutant) 
    {
        _source = source;
        _pollutant = pollutant;
        ComboEnabled = true;
        ComboButton.enabled = true;
    }

    void OnEnable()
    {
        ComboButton.onClick.AddListener(HandleShowTheory);
        ComboButton.enabled = ComboEnabled;

        ComboCloser.onClick.AddListener(HandleHideTheory);
    }

    void OnDisable()
    {
        ComboButton.onClick.RemoveListener(HandleShowTheory);

        ComboCloser.onClick.RemoveListener(HandleHideTheory);
    }

    private void HandleShowTheory()
    {
        ComboPopup.SetActive(true);
        
        InvestigationTimelineChunk.OnValidSelected += HandleValidSelection;

        switch (ComboType)
        {
            case TheoryComboType.PollutantAndSource:
                HandlePAndSCombo();
                break;
            case TheoryComboType.PollutantAndSymptom:
                HandlePAndSymCombo();
                break;
            case TheoryComboType.PollutantAtSource:
                HandlePAtSCombo();
                break;
            case TheoryComboType.PollutantAtSymptom:
                HandlePAtSymCombo();
                break;
        }
    }

    private void HandleHideTheory()
    {
        ComboPopup.SetActive(false);
        if (PollutantAtSource != null)
        {
            PollutantAtSource.LockedMap.gameObject.SetActive(false);
            PollutantAtSource.FalseSlider.interactable = true;
        }

        if (PollutantAtSymptom != null)
        {
            PollutantAtSymptom.LockedMap.gameObject.SetActive(false);
            PollutantAtSymptom.FalseSlider.interactable = true;
        }
        
        InvestigationTimelineChunk.OnValidSelected -= HandleValidSelection;
    }

    private void HandlePAndSCombo()
    {
        PlayerInvestigationTimeline.OnFeatureDetailRequested.Invoke(_source, _pollutant);
    }

    private void HandlePAndSymCombo()
    {
        SymptomManager.Setup(_pollutant);
    }

    private void HandlePAtSCombo()
    {
        Debug.Log("Pollutant at source!");

        ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;
        
        int earliestTimeSeen = 99;
        foreach (var room in data.Rooms)
        {
            foreach (var timeSlot in room.TimeSlots)
            {
                foreach (PollutantReading reading in timeSlot.PollutantReadings)
                {
                    if (reading.Pollutant == _pollutant && reading.Concentration > 0 && timeSlot.Time < earliestTimeSeen)
                    {
                        KnowledgeType knowledge = InvestigationLookup.Instance.PollutantMap.GetKnowledge(_pollutant);
                        if (PlayerKnowledgeState.IsKnownHourly(room.RoomTypeValue, timeSlot.Time, knowledge)) {
                            earliestTimeSeen = timeSlot.Time;
                        }
                    }
                }
            }
        }

        PollutantAtSource.Setup(earliestTimeSeen, _pollutant);
        StartCoroutine(RefreshLockedMapAfterFrame(earliestTimeSeen, true));
    }

    private void HandlePAtSymCombo()
    {
        Debug.Log("Pollutant at Symptom!");

        ScenarioDataObject data = InvestigationTimelineSystem.Instance.ScenarioData;

        int unconsciousTime = 99;
        foreach (var npc in data.NPCs)
        {
            if (npc.Character == data.MainNpc)
            {
                foreach (var time in npc.TimeSlots)
                {
                    if (time.Symptom == Symptom.LossConsciousness)
                    {
                        if (PlayerKnowledgeState.IsKnownCharacterly(data.MainNpc, time.Time, KnowledgeType.NPCSymptom))
                        {
                            unconsciousTime = time.Time;
                        }
                    }
                }
            }
        }

        PollutantAtSymptom.Setup(unconsciousTime, _pollutant);
        StartCoroutine(RefreshLockedMapAfterFrame(unconsciousTime, false));
    }

    private IEnumerator RefreshLockedMapAfterFrame(int hour, bool isSourceSelector)
    {
        yield return null;
        if (LockedMap == null)
        {
            yield break;
        }

        if (isSourceSelector)
        {
            LockedMap.SetupSourceSelector(hour, _pollutant, _source);
        }
        else
        {
            LockedMap.SetupSymptomSelector(hour, _pollutant);
        }
    }

    private void HandleValidSelection()
    {
        HandleHideTheory();
        UnassignedIndicator.SetActive(false);
        PlayerInvestigationTimeline.OnResetRequested.Invoke();
        InvestigationTimelineChunk.OnValidSelected -= HandleValidSelection;
    }
}
