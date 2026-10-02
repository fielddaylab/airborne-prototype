using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuspectPickerManager : MonoBehaviour
{
    public GameObject SuspectCardPrefab;
    public Transform SuspectCardParent;
    public List<SuspectCard> Cards = new();
    public Button ConfirmButton;
    public TMP_Text ConfirmText;

    public SuspectCard PanelCard;

    private PollutantType _selectedPollutant = PollutantType.None;

    // Theory panel 
    public GameObject TheoryPiece;

    public Transform SymptomsBox;
    public Transform SourcesBox;

    public TMP_Text TheoryText;
    public Button TheorizeButton;

    private List<TheoryPiece> _symptoms = new();
    private List<TheoryPiece> _sources = new();

    void Awake()
    {
        gameObject.SetActive(false);
        ConfirmButton.onClick.AddListener(ConfirmSuspect);
        InvestigationTimelineSystem.OnLoopEnd += HandleNewLoop;
        ConfirmText.text = "Confirm";
        Setup();
    }
    
    public void OnDestroy()
    {
        foreach (var c in Cards)
        {
            c.Button.onClick.RemoveAllListeners();
        }

        ConfirmButton.onClick.RemoveAllListeners();
        InvestigationTimelineSystem.OnLoopEnd -= HandleNewLoop;
    }

    public void HandleNewLoop()
    {
        if (NewGameManager.Instance.CurrentPhase == NewGamePhase.Investigation) {
            gameObject.SetActive(true);
            UpdateInformation();
            ConfirmText.text = "Keep";
        }
    }

    public void Setup()
    {
        Clear();
        ConfirmButton.interactable = false;

        PanelCard.Clear();

        PollutantDataObject[] datas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        foreach (var d in datas)
        {
            GameObject cardObj = Instantiate(SuspectCardPrefab);
            cardObj.transform.SetParent(SuspectCardParent, false);
            SuspectCard card = cardObj.GetComponent<SuspectCard>();
            card.SetupPollutant(d.Type);
            card.Button.onClick.AddListener(() => SetSuspect(d.Type));
            Cards.Add(card);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < SuspectCardParent.transform.childCount; i++)
        {
            if (Cards.Count > i && Cards[i] != null)
            {
                Cards[i].Button.onClick.RemoveAllListeners();
                Cards.Remove(Cards[i]); 
            }

            Destroy(SuspectCardParent.GetChild(i).gameObject);
        }

        Cards = new();
    }

    public void ClearBoxes()
    {
        for (int i = 0; i < SymptomsBox.transform.childCount; i++)
        {
            Destroy(SymptomsBox.GetChild(i).gameObject);
        }

        _symptoms = new();

        for (int i = 0; i < SourcesBox.transform.childCount; i++)
        {
            Destroy(SourcesBox.GetChild(i).gameObject);
        }

        _sources = new();
    }

    public void SetSuspect(PollutantType pollutant)
    {
        _selectedPollutant = pollutant;
        ConfirmButton.interactable = true;
        ConfirmText.text = "Confirm";

        ClearBoxes();
        
        for (int i = 0; i < Cards.Count; i++)
        {
            if (Cards[i].PollutantType == pollutant) 
            {
                Cards[i].gameObject.SetActive(false);
            } else
            {
                Cards[i].gameObject.SetActive(true);
            }
        }

        PollutantDataObject[] datas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        PollutantDataObject pollutantData = datas[0];
        foreach (var d in datas) if (d.Type == pollutant) pollutantData = d;

        foreach (var symptom in pollutantData.Symptoms)
        {
            GameObject theoryPiece = Instantiate(TheoryPiece, SymptomsBox); 
            theoryPiece.name = symptom.ToString();
            theoryPiece.transform.SetParent(SymptomsBox);
            TheoryPiece piece = theoryPiece.GetComponent<TheoryPiece>();
            piece.TheoryImage.sprite = InvestigationLookup.Instance.SymptomMap.GetSprite(symptom);
            piece.RepresentedSymptom = symptom;
            piece.Tooltip.ChangeText(symptom.ToString());

            _symptoms.Add(piece);
        }

        foreach (var source in pollutantData.Sources)
        {
            GameObject theoryPiece = Instantiate(TheoryPiece, SourcesBox); 
            theoryPiece.name = source.ToString();
            theoryPiece.transform.SetParent(SourcesBox);
            TheoryPiece piece = theoryPiece.GetComponent<TheoryPiece>();
            piece.TheoryImage.sprite = FeatureSpriteMapUtility.GetOnSprite(InvestigationLookup.Instance.FeatureSpriteMap, source);
            piece.RepresentedFeature = source;
            piece.Tooltip.ChangeText(source.ToString());

            _sources.Add(piece);
        }

        PanelCard.SetupPollutant(pollutant);
        // fixing other stuff later!

        UpdateInformation();
    }

    public void UpdateInformation()
    {
        int totalInfo = 0;

        foreach (var piece in _symptoms)
        {
            if (PlayerKnowledgeState.HasSeenSymptom(piece.RepresentedSymptom))
            {
                piece.Cycler.SetChecked(true);
                totalInfo++;
            }
        }

        foreach (var piece in _sources)
        {
            if (PlayerKnowledgeState.HasSeenFeature(piece.RepresentedFeature))
            {
                piece.Cycler.SetChecked(true);
                totalInfo++;
            }
        }

        TheoryText.text = $"{totalInfo}/4";
        TheorizeButton.interactable = false;

        if (totalInfo >= 4)
        {
            TheorizeButton.interactable = true;
        }
    }

    public void ConfirmSuspect()
    {
        gameObject.SetActive(false);
        NewGameManager.ChooseSuspect(_selectedPollutant);
    }
}
