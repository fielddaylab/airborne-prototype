using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasOverlayManager : MonoBehaviour
{
    public Transform GasOverlayButtonParent;
    public GameObject OverlayPrefab;

    public static Action<PollutantType> OnOverlayChange;

    public GasOverlayButton[] OverlayButtons;

    public void Start()
    {
        OnOverlayChange += HandleOverlayChange;
        Setup();
    }

    public void Setup()
    {
        for (int i = 0; i < GasOverlayButtonParent.childCount; i++)
        {
            Destroy(GasOverlayButtonParent.GetChild(i).gameObject);
        }

        PollutantDataObject[] pollutants = InvestigationTimelineSystem.Instance.ScenarioData.SuspectedPollutants;
        OverlayButtons = new GasOverlayButton[pollutants.Length];

        for (int i = 0; i < pollutants.Length; i++)
        {
            GameObject overlayObj = Instantiate(OverlayPrefab);
            overlayObj.transform.SetParent(GasOverlayButtonParent, false);

            GasOverlayButton overlay = overlayObj.GetComponent<GasOverlayButton>();
            overlay.Setup(pollutants[i].Type);
            OverlayButtons[i] = overlay;
        }

        HandleOverlayChange(pollutants[0].Type);
    }

    public void HandleOverlayChange(PollutantType pollutant)
    {
        foreach (var overlay in OverlayButtons)
        {
            if (overlay.PollutantType == pollutant)
            {
                overlay.OverlayButton.interactable = false;
            } else {
                overlay.OverlayButton.interactable = true;
            }
        }

        InvestigationMap.OnSetPollutant?.Invoke(pollutant);
    }
}
