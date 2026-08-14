using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SourceAndSymptomManager : MonoBehaviour
{
    public GameObject SymptomButton;
    public GameObject SymptomPanel;

    public TextMeshProUGUI SymptomSelectText, TimelineQuestionText;

    public static Action<Symptom> OnSelectSymptom;

    public GameObject SymptomList, TimelineQuestion;

    private Symptom _symptom;
    private PollutantType _pollutant;

    enum Phase
    {
        SymptomSelect,
        TimelineSelect
    }

    void Start()
    {
        SymptomList.gameObject.SetActive(true);
        TimelineQuestion.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        OnSelectSymptom += OnSymptomSelected;
    }

    void OnDisable()
    {
        OnSelectSymptom -= OnSymptomSelected;
    }

    public void Setup(PollutantType pollutant)
    {
        SymptomList.gameObject.SetActive(true);
        TimelineQuestion.gameObject.SetActive(false);
        
        for (int i = 0; i  < SymptomPanel.transform.childCount; i++)
        {
            Destroy(SymptomPanel.transform.GetChild(i).gameObject);
        }
        
        PollutantDataObject pollutantData = null;
        _pollutant = pollutant;

        foreach (var map in InvestigationLookup.Instance.PollutantSymptomMaps)
        {
            if (map.Type == pollutant)
            {
                pollutantData = map;
            }
        }

        ScenarioDataObject scenario = InvestigationTimelineSystem.Instance.ScenarioData;
        List<Symptom> symptoms = new();
        foreach (InvestigationNPCObject npc in scenario.NPCs) {
            if (npc.Character == scenario.MainNpc)
            {
                foreach (var slot in npc.TimeSlots)
                {
                    symptoms.Add(slot.Symptom);
                }
            }
        }

        foreach (var symptom in pollutantData.Symptoms)
        {
            if (symptoms.Contains(symptom)) {
                GameObject buttonObj = Instantiate(SymptomButton, SymptomPanel.transform);
                SymptomButton symptomButton = buttonObj.GetComponent<SymptomButton>();
                symptomButton.SymptomImage.sprite = InvestigationLookup.Instance.SymptomMap.GetSprite(symptom);
                symptomButton.Symptom = symptom;
            }
        }

        PlayerInvestigationTimeline.OnTimelineRequested?.Invoke(scenario.MainNpc);

        SymptomSelectText.text = $"Select a symptom Roundy experienced that matches with {pollutant}:";
    }

    private void OnSymptomSelected(Symptom symptom)
    {
        _symptom = symptom;
        SymptomList.SetActive(false);
        TimelineQuestion.SetActive(true);

        TimelineQuestionText.text = $"Does the {_symptom} occur while {_pollutant} is present?\n\nSelect from timeline:";

        PlayerInvestigationTimeline.OnNPCDetailRequested.Invoke(_symptom, _pollutant);
    }
}
