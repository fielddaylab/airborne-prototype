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

    void Start()
    {
        foreach (var pair in PollutantEnableButtons)
        {
            HandlePollutantEnable(pair);
        }
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