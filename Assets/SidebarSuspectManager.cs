using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarSuspectManager : MonoBehaviour
{
    public SuspectCard Card;

    public GameObject TheoryPiece;

    public Transform SymptomsBox;
    public Transform SourcesBox;

    private List<TheoryPiece> _symptoms = new();
    private List<TheoryPiece> _sources = new();
    
    public void Start()
    {
        NewGameManager.ChooseSuspect += HandleSuspect;
        Card.Clear();

        PlayerKnowledgeState.OnKnowledgeUpdated += UpdateInformation;
    }

    private void HandleSuspect(PollutantType pollutant)
    {
        Card.Setup(pollutant);

        PollutantDataObject pollutantData = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants[0];
        foreach (var p in InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants)
        {
            if (p.Type == pollutant) pollutantData = p;
        }

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

        // TheorySlider.value = totalInfo;
        // TheoryText.text = $"{totalInfo}/4";
        // TheorizeButton.interactable = false;

        // if (totalInfo >= 4)
        // {
        //     TheoryText.text = "Theorize";
        //     TheorizeButton.interactable = true;
        // }
    }
}
