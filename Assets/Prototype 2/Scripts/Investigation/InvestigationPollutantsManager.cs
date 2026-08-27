using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestigationPollutantsManager : MonoBehaviour
{
    //public PollutantButtonPair[] PollutantDisableButtons;
    private List<PollutantType> _disabledPollutants = new();

    //public PollutantPanelPair[] PollutantPanels;
    //public PollutantButtonPair[] PollutantEnableButtons;

    private TheoryPanel[] TheoryPanels;
    private PollutantPanelExpander[] Expanders;
    private PollutantDataObject[] _PollutantDataInfo;

    //public PollutantButtonPair[] PollutantTheoryButtons;
    public GameObject PollutantPanelPrefab, PollutantExpandPrefab;

    public GameObject SuspectPanelParent, CollapsedParent;
    public TheoryManager TheoryManager;
    public Button TheoryCollapser;

    public static Action<PollutantType> OnTheoryStart;
    public static Action<PollutantType> OnCollapsePanel;
    public static Action<PollutantType> OnExpandPanel;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        // pollutant panels
        for (int i = 0; i < SuspectPanelParent.transform.childCount; i++)
        {
            Destroy(SuspectPanelParent.transform.GetChild(i).gameObject);
        }

        _PollutantDataInfo = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        TheoryPanels = new TheoryPanel[_PollutantDataInfo.Length];

        for (int i = 0; i < TheoryPanels.Length; i++)
        {
            GameObject panel = Instantiate(PollutantPanelPrefab);
            panel.transform.SetParent(SuspectPanelParent.transform, false);

            TheoryPanel theoryPanel = panel.GetComponent<TheoryPanel>();
            theoryPanel.AssemblePanel(_PollutantDataInfo[i]);

            TheoryPanels[i] = theoryPanel;
        }

        // enable buttons
        for (int i = 0; i < CollapsedParent.transform.childCount; i++)
        {
            Destroy(CollapsedParent.transform.GetChild(i).gameObject);
        }

        Expanders = new PollutantPanelExpander[_PollutantDataInfo.Length];

        for (int i = 0; i < TheoryPanels.Length; i++)
        {
            GameObject expander = Instantiate(PollutantExpandPrefab);
            expander.transform.SetParent(CollapsedParent.transform, false);

            PollutantPanelExpander panelExpander = expander.GetComponent<PollutantPanelExpander>();
            panelExpander.Setup(_PollutantDataInfo[i].Type);

            Expanders[i] = panelExpander;
            expander.SetActive(false);
        }

        TheoryManager.gameObject.SetActive(false);
    }

    public void UpdateInformation()
    {
        Setup();
        
        foreach (var panel in TheoryPanels)
        {
            panel.UpdateInformation();
        }
        
        if (TheoryManager.InTheoryMode) TheoryManager.UpdateInformation();
    }

    void OnEnable()
    {
        OnTheoryStart += HandleTheoryStart;
        OnCollapsePanel += HandleCollapsePanel;
        OnExpandPanel += HandleExpandPanel;

        TheoryCollapser.onClick.AddListener(HandleTheoryCollapse);
    }

    void OnDisable()
    {
        OnTheoryStart -= HandleTheoryStart;
        OnCollapsePanel -= HandleCollapsePanel;
        OnExpandPanel -= HandleExpandPanel;

        TheoryCollapser.onClick.RemoveListener(HandleTheoryCollapse);
    }

    private void HandleTheoryStart(PollutantType pollutant)
    {
        SuspectPanelParent.SetActive(false);
        CollapsedParent.SetActive(false);
        TheoryManager.gameObject.SetActive(true);

        foreach (var data in _PollutantDataInfo)
        {
            if (data.Type == pollutant) {
                TheoryManager.AssemblePanel(data);
                break;
            }
        }

        TheoryManager.InTheoryMode = true;
    }
    
    private void HandleCollapsePanel(PollutantType pollutant)
    {
        _disabledPollutants.Add(pollutant);

        foreach (var panel in TheoryPanels)
        {
            if (panel.PollutantType == pollutant) panel.gameObject.SetActive(false);
        }

        foreach (var expander in Expanders)
        {
            if (expander.PollutantType == pollutant) expander.gameObject.SetActive(true);
        }
    }

    private void HandleExpandPanel(PollutantType pollutant)
    {
        _disabledPollutants.Remove(pollutant);

        foreach (var panel in TheoryPanels)
        {
            if (panel.PollutantType == pollutant) panel.gameObject.SetActive(true);
        }

        foreach (var expander in Expanders)
        {
            if (expander.PollutantType == pollutant) expander.gameObject.SetActive(false);
        }
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
        // foreach (PollutantButtonPair pair in PollutantEnableButtons)
        // {
        //     pair.PollutantButton.gameObject.SetActive(false);
        // }
    }
}