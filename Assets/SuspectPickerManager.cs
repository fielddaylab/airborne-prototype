using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuspectPickerManager : MonoBehaviour
{
    public GameObject InfoCardPrefab;
    public TMP_Text Header;
    public Transform SuspectCardParent;
    public List<InfoCard> SuspectCards = new();
    public Button ConfirmButton;
    public TMP_Text ConfirmText;

    public InfoCard PanelCard;

    private PollutantType _selectedPollutant = PollutantType.None;

    // Theory panel 
    public GameObject TheoryPiece;

    public Transform SymptomsBox;
    public Transform SourcesBox;

    public TMP_Text TheoryText;
    public Button TheorizeButton;

    private List<TheoryPiece> _symptoms = new();
    private List<TheoryPiece> _sources = new();

    public GameObject SuspectRegion;
    public GameObject TheoryRegion;

    public List<InfoCard> SourceCards;
    public Transform SourceCardParent;

    public SidebarTheoryManager SidebarTheory, OverviewTheory;

    public Button TheoryBackButton, TheoryConfirmButton;

    void Awake()
    {
        gameObject.SetActive(false);
        ConfirmButton.onClick.AddListener(ConfirmSuspect);
        InvestigationTimelineSystem.OnLoopEnd += HandleNewLoop;
        ConfirmText.text = "Confirm";
        Setup();
        TheorizeButton.onClick.AddListener(HandleTheoryStart);
        TheoryRegion.SetActive(false);
        TheoryConfirmButton.onClick.AddListener(HandleTheoryConfirmation);
    }
    
    public void OnDestroy()
    {
        foreach (var c in SuspectCards)
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
        ClearSuspect();
        ConfirmButton.interactable = false;

        PanelCard.Clear();

        PollutantDataObject[] datas = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        foreach (var d in datas)
        {
            GameObject cardObj = Instantiate(InfoCardPrefab);
            cardObj.transform.SetParent(SuspectCardParent, false);
            InfoCard card = cardObj.GetComponent<InfoCard>();
            card.SetupPollutant(d.Type);
            card.Button.onClick.AddListener(() => SetSuspect(d.Type));
            SuspectCards.Add(card);
        }
    }

    public void ClearSuspect()
    {
        for (int i = 0; i < SuspectCardParent.transform.childCount; i++)
        {
            if (SuspectCards.Count > i && SuspectCards[i] != null)
            {
                SuspectCards[i].Button.onClick.RemoveAllListeners();
                SuspectCards.Remove(SuspectCards[i]); 
            }

            Destroy(SuspectCardParent.GetChild(i).gameObject);
        }

        SuspectCards = new();
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
        
        for (int i = 0; i < SuspectCards.Count; i++)
        {
            if (SuspectCards[i].PollutantType == pollutant) 
            {
                SuspectCards[i].gameObject.SetActive(false);
            } else
            {
                SuspectCards[i].gameObject.SetActive(true);
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

    public void ClearSources()
    {
        for (int i = 0; i < SourceCardParent.transform.childCount; i++)
        {
            if (SourceCards.Count > i && SourceCards[i] != null)
            {
                SourceCards[i].Button.onClick.RemoveAllListeners();
                SourceCards.Remove(SourceCards[i]); 
            }

            Destroy(SourceCardParent.GetChild(i).gameObject);
        }

        SourceCards = new();
    }

    private void HandleTheoryStart()
    {
        string fullName = InvestigationLookup.Instance.PollutantMap.GetFullName(_selectedPollutant);
        
        Header.text = $"Choose a <b>Source</b> of <b>{fullName}</b> to investigate in this loop.";

        SuspectRegion.SetActive(false);
        TheoryRegion.SetActive(true);

        ClearSources();

        PollutantDataObject suspectedPollutantData;
        foreach (var d in InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants)
        {
            if (d.Type == _selectedPollutant)
            {
                suspectedPollutantData = d;
                foreach (var s in d.Sources)
                {
                    GameObject cardObj = Instantiate(InfoCardPrefab);
                    cardObj.transform.SetParent(SourceCardParent, false);
                    InfoCard card = cardObj.GetComponent<InfoCard>();
                    card.SetupFeature(s);
                    card.Button.onClick.AddListener(() => HandleSourceSelection(s));
                    SourceCards.Add(card);
                }
            }
        }
        SidebarTheory.gameObject.SetActive(true);

        SidebarTheory.SetupPollutant(_selectedPollutant);
        OverviewTheory.SetupPollutant(_selectedPollutant);
    }

    private void HandleSourceSelection(FeatureType source)
    {
        SidebarTheory.SetupSource(source);
        OverviewTheory.SetupSource(source);
        TheoryConfirmButton.interactable = true;
    }

    private void HandleTheoryConfirmation()
    {
        NewGameManager.ChooseSuspect(_selectedPollutant);
        gameObject.SetActive(false);
    }
}
