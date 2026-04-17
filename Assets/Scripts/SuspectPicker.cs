using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuspectPicker : MonoBehaviour
{
    [SerializeField] private RawImage[] pollutantPanels;
    [SerializeField] private Button nextStepButton;
    [SerializeField] private ComboAreaManager comboArea;
    private int selectedPanel = -1;

    void Start()
    {
        nextStepButton.interactable = false;
        foreach (RawImage panel in pollutantPanels)
        {
            panel.enabled = false;
        }        
    }

    public void SelectPanel(int i)
    {
        if (selectedPanel == i)
        {
            pollutantPanels[i].enabled = false;
            selectedPanel = -1;
            nextStepButton.interactable = false;
        }
        else
        {
            if (selectedPanel != -1) pollutantPanels[selectedPanel].enabled = false;
            nextStepButton.interactable = true;
            pollutantPanels[i].enabled = true;
            selectedPanel = i;
        }
    }

    public void LoadPollutant()
    {
        comboArea.gameObject.SetActive(true);
        comboArea.LoadPollutant(pollutantPanels[selectedPanel].gameObject);
        gameObject.SetActive(false);
        nextStepButton.interactable = false;
        pollutantPanels[selectedPanel].enabled = false;
    }
}
