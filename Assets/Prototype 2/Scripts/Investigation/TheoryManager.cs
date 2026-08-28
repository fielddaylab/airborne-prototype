using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TheoryManager : MonoBehaviour
{
    public BossPromptController bossPrompt;
    
    [Header("Pollutant Fields")]
    public Image PollutantPortrait;
    public TextMeshProUGUI PollutantText;
    public GameObject TheoryPiece;
    public Transform SymptomsBox;
    public Transform SourcesBox;
    public Image[] PollutantClouds;
    public TextMeshProUGUI[] PollutantTexts;
    private List<TheoryPiece> _symptoms = new();
    private List<TheoryPiece> _sources = new();

    [Header("Source Fields")]
    public Image[] SourcePortraits;
    public TextMeshProUGUI SourceText;
    public Button SourceButton;
    public Image SourceQuestion;
    public SourceSelector SourceSelection;

    [Header("Combo Fields")]
    public TheoryCombo[] Combos;
    public bool InTheoryMode = false;
    public Slider TheorySlider;
    public TextMeshProUGUI TheoryText;
    public Button TheorizeButton;

    // internals
    private FeatureType _sourceType;
    private PollutantType _pollutantType;
    private PollutantDataObject _pollutantData;

    private int _theoryProgress = 0;

    public void Start()
    {
        TheorySlider.value = _theoryProgress;
        TheoryText.text = $"{_theoryProgress}/4";
        TheorizeButton.interactable = false;

        if (_theoryProgress >= 4)
        {
            TheoryText.text = "Submit";
            TheorizeButton.interactable = true;
        }
    }

    public void OnEnable()
    {
        SourceButton.onClick.AddListener(HandleSourceSelection);
        SourceSelector.OnSourceSelection += HandleSourceSelected;
        InvestigationTimelineChunk.OnValidSelected += HandleValidSelection;
        TheorizeButton.onClick.AddListener(HandleTheorySubmission);
    }

    public void OnDisable()
    {
        SourceButton.onClick.RemoveListener(HandleSourceSelection);
        SourceSelector.OnSourceSelection -= HandleSourceSelected;
        InvestigationTimelineChunk.OnValidSelected -= HandleValidSelection;
        TheorizeButton.onClick.RemoveListener(HandleTheorySubmission);
    }

    public void AssemblePanel(PollutantDataObject pollutantData)
    {
        Reset();
        _pollutantData = pollutantData;
        
        Debug.Log("Assemble: " + pollutantData.Type);
        
        foreach (var symptom in pollutantData.Symptoms)
        {
            GameObject theoryPiece = Instantiate(TheoryPiece, SymptomsBox); 
            theoryPiece.name = symptom.ToString();
            theoryPiece.transform.SetParent(SymptomsBox);
            TheoryPiece piece = theoryPiece.GetComponent<TheoryPiece>();
            piece.TheoryImage.sprite = InvestigationLookup.Instance.SymptomMap.GetSprite(symptom);
            piece.RepresentedSymptom = symptom;

            _symptoms.Add(piece);
        }

        foreach (var source in pollutantData.Sources)
        {
            GameObject theoryPiece = Instantiate(TheoryPiece, SourcesBox); 
            theoryPiece.name = source.ToString();
            theoryPiece.transform.SetParent(SourcesBox);
            TheoryPiece piece = theoryPiece.GetComponent<TheoryPiece>();
            piece.TheoryImage.sprite = InvestigationLookup.Instance.SourceImages.GetSprite(source);
            piece.RepresentedFeature = source;

            _sources.Add(piece);
        }

        foreach (var image in PollutantClouds)
        {
            image.color = InvestigationLookup.Instance.PollutantMap.GetColor(pollutantData.Type);
        }

        foreach (var text in PollutantTexts)
        {
            text.text = pollutantData.Type.ToString();
        }

        PollutantPortrait.sprite = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutantData.Type);
        PollutantText.text = InvestigationLookup.Instance.PollutantMap.GetFullName(pollutantData.Type);

        foreach (var image in SourcePortraits)
        {
            image.enabled = false;
        }
        SourceText.text = "";

        UpdateInformation();
    }

    public void UpdateInformation()
    {
        int totalInfo = 0;

        foreach (var piece in _symptoms)
        {
            if (PlayerKnowledgeState.HasSeenSymptom(piece.RepresentedSymptom))
            {
                piece.Cycler.SetCycle(1);
                totalInfo++;
            }
        }

        foreach (var piece in _sources)
        {
            if (PlayerKnowledgeState.HasSeenFeature(piece.RepresentedFeature))
            {
                piece.Cycler.SetCycle(1);
                totalInfo++;
            }
        }
    }

    private void HandleSourceSelected(FeatureType feature)
    {
        ResetSelectionState();
        Sprite sourceSprite = InvestigationLookup.Instance.SourceImages.GetSprite(feature);
        Color sourceColor = SourceButton.image.color;
        sourceColor.a = 0;
        SourceButton.image.color = sourceColor;
        SourceQuestion.enabled = false;

        ScenarioDataObject scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;
        RoomType sourceRoom = ScenarioUtility.GetRoom(feature, scenarioData);

        SourceText.text = $"{sourceRoom}\n{feature}";

        foreach (var image in SourcePortraits)
        {
            image.enabled = true;
            image.sprite = sourceSprite;
        }

        _sourceType = feature;

        foreach (var combo in Combos)
        {
            combo.Setup(_sourceType, _pollutantData.Type);
        }
    }

    private void Reset()
    {
        _symptoms = new();
        _sources = new();
        
        foreach (Transform child in SymptomsBox)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in SourcesBox)
        {
            Destroy(child.gameObject);
        }

        SourceSelection.gameObject.SetActive(false);

        foreach (var combo in Combos)
        {
            combo.Reset();
        } 
    }

    private void ResetSelectionState()
    {
        SourceSelection.gameObject.SetActive(false);

        foreach (var combo in Combos)
        {
            combo.Reset();
        }
    }

    private void HandleSourceSelection()
    {
        SourceSelection.gameObject.SetActive(true);
    }

    private void HandleValidSelection()
    {
        _theoryProgress += 1;

        TheorySlider.value = _theoryProgress;
        TheoryText.text = $"{_theoryProgress}/4";
        TheorizeButton.interactable = false;

        if (_theoryProgress >= 4)
        {
            TheoryText.text = "Submit";
            TheorizeButton.interactable = true;
            
            NewGameManager.Instance.SwitchToPhase(NewGamePhase.Transition);
        }
    }

    private void HandleTheorySubmission()
    {
        bossPrompt.StartBossSequence(_pollutantData.Type, _sourceType);
        TheorySlider.gameObject.SetActive(false);
    }
}
