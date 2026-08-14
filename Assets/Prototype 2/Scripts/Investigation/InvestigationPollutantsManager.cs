using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationPollutantsManager : MonoBehaviour
{
    public PollutantButtonPair[] PollutantDisableButtons;
    private List<PollutantType> _disabledPollutants = new();

    public PollutantPanelPair[] PollutantPanels;
    public PollutantButtonPair[] PollutantEnableButtons;

    public TheoryPanel[] TheoryPanels;
    public PollutantDataObject[] PollutantDataInfo;

    public PollutantButtonPair[] PollutantTheoryButtons;

    public GameObject SuspectPanelParent, CollapsedParent;
    public TheoryManager TheoryManager;
    public Button TheoryCollapser;

    void Start()
    {
        foreach (var pair in PollutantEnableButtons)
        {
            HandlePollutantEnable(pair);
        }

        foreach (var panel in TheoryPanels)
        {
            foreach (var data in PollutantDataInfo)
            {
                if (data.Type == panel.PollutantType) {
                    panel.AssemblePanel(data);
                }
            }
        }

        TheoryManager.gameObject.SetActive(false);
    }

    public void UpdateInformation()
    {
        foreach (var panel in TheoryPanels)
        {
            panel.UpdateInformation();
        }
        
        if (TheoryManager.InTheoryMode) TheoryManager.UpdateInformation();
    }

    void OnEnable()
    {
        foreach (var pair in PollutantDisableButtons)
        {
            pair.PollutantButton.onClick.AddListener(() => HandlePollutantCollapse(pair));
        }

        foreach (var pair in PollutantEnableButtons)
        {
            pair.PollutantButton.onClick.AddListener(() => HandlePollutantEnable(pair));
        }

        foreach (var pair in PollutantTheoryButtons)
        {
            pair.PollutantButton.onClick.AddListener(() => HandlePollutantTheory(pair));
        }

        TheoryCollapser.onClick.AddListener(HandleTheoryCollapse);
    }

    void OnDisable()
    {
        foreach (var pair in PollutantDisableButtons)
        {
            pair.PollutantButton.onClick.RemoveAllListeners();
        }

        foreach (var pair in PollutantEnableButtons)
        {
            pair.PollutantButton.onClick.RemoveAllListeners();
        }

        foreach (var pair in PollutantTheoryButtons)
        {
            pair.PollutantButton.onClick.RemoveAllListeners();
        }

        TheoryCollapser.onClick.RemoveListener(HandleTheoryCollapse);
    }

    private void HandlePollutantCollapse(PollutantButtonPair pair)
    {
        PollutantType pollutant = pair.Pollutant;
        _disabledPollutants.Add(pollutant);

        foreach (var panel in PollutantPanels)
        {
            if (panel.Pollutant == pollutant) panel.Panel.SetActive(false);
        }

        foreach (var button in PollutantEnableButtons)
        {
            if (button.Pollutant == pollutant) button.PollutantButton.gameObject.SetActive(true);
        }
    }

    private void HandlePollutantEnable(PollutantButtonPair pair)
    {
        PollutantType pollutant = pair.Pollutant;
        _disabledPollutants.Remove(pollutant);

        foreach (var panel in PollutantPanels)
        {
            if (panel.Pollutant == pollutant) panel.Panel.SetActive(true);
        }

        foreach (var button in PollutantEnableButtons)
        {
            if (button.Pollutant == pollutant) button.PollutantButton.gameObject.SetActive(false);
        }
    }

    private void HandlePollutantTheory(PollutantButtonPair pair)
    {
        SuspectPanelParent.SetActive(false);
        CollapsedParent.SetActive(false);
        TheoryManager.gameObject.SetActive(true);
        foreach (var data in PollutantDataInfo)
        {
            if (data.Type == pair.Pollutant) {
                TheoryManager.AssemblePanel(data);
                break;
            }
        }

        TheoryManager.InTheoryMode = true;
    }

    private void HandleTheoryCollapse()
    {
        SuspectPanelParent.SetActive(true);
        CollapsedParent.SetActive(true);
        TheoryManager.gameObject.SetActive(false);

        TheoryManager.InTheoryMode = false;
    }

    private void Reset()
    {
        foreach (PollutantButtonPair pair in PollutantEnableButtons)
        {
            pair.PollutantButton.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class PollutantButtonPair
{
    public Button PollutantButton;
    public PollutantType Pollutant;
}

[System.Serializable]
public class PollutantPanelPair
{
    public GameObject Panel;
    public PollutantType Pollutant;
}