using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SuspectPickerManager : MonoBehaviour
{
    public GameObject SuspectCardPrefab;
    public Transform SuspectCardParent;
    public List<SuspectCard> Cards = new();
    public Button ConfirmButton;

    public SuspectCard PanelCard;

    private PollutantType _selectedPollutant = PollutantType.None;

    // Theory panel 
    public GameObject TheoryPiece;

    public Transform SymptomsBox;
    public Transform SourcesBox;

    private List<TheoryPiece> _symptoms = new();
    private List<TheoryPiece> _sources = new();

    void Awake()
    {
        //gameObject.SetActive(false);
        ConfirmButton.onClick.AddListener(ConfirmSuspect);
    }

    void Start()
    {
        Setup();
    }
    
    public void OnDestroy()
    {
        foreach (var c in Cards)
        {
            c.Button.onClick.RemoveAllListeners();
        }

        ConfirmButton.onClick.RemoveAllListeners();
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
            card.Setup(d.Type);
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
    }

    public void ClearBoxes()
    {
        for (int i = 0; i < SymptomsBox.transform.childCount; i++)
        {
            Destroy(SymptomsBox.GetChild(i).gameObject);
        }

        for (int i = 0; i < SourcesBox.transform.childCount; i++)
        {
            Destroy(SourcesBox.GetChild(i).gameObject);
        }
    }

    public void SetSuspect(PollutantType pollutant)
    {
        _selectedPollutant = pollutant;
        ConfirmButton.interactable = true;

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

        PanelCard.Setup(pollutant);
        // fixing other stuff later!
    }

    public void ConfirmSuspect()
    {
        gameObject.SetActive(false);
    }
}
