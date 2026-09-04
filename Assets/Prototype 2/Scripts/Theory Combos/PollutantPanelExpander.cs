using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutantPanelExpander : MonoBehaviour
{
    public Button ExpandButton;
    public PollutantType PollutantType;
    public Image Portrait;

    public void Setup(PollutantType pollutantType)
    {
        ExpandButton.onClick.AddListener(HandleExpand);
        Portrait.sprite = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutantType);
        PollutantType = pollutantType;
    }

    private void HandleExpand()
    {
        InvestigationPollutantsManager.OnExpandPanel?.Invoke(PollutantType);
    }
}
