using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TheoryPanel : MonoBehaviour
{
    public PollutantType PollutantType;
    public Slider TheorySlider;
    public TextMeshProUGUI TheoryText;
    public GameObject TheoryPiece;

    public Transform SymptomsBox;
    public Transform SourcesBox;

    private List<TheoryPiece> _symptoms = new();
    private List<TheoryPiece> _sources = new();

    public Button TheorizeButton;

    public void AssemblePanel(PollutantDataObject pollutantData)
    {
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

        TheorySlider.value = totalInfo;
        TheoryText.text = $"{totalInfo}/4";
        TheorizeButton.interactable = false;

        if (totalInfo >= 4)
        {
            TheoryText.text = "Theorize";
            TheorizeButton.interactable = true;
        }
    }
}
