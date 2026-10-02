using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimelineGasOverlay : MonoBehaviour
{
    public Transform GasOverlayButtonParent;
    public GameObject OverlayPrefab;

    public static Action<PollutantType> OnOverlayChange;

    [HideInInspector] public TimelineGasOverlayButton[] OverlayButtons;

    PollutantType _currentPollutant;
    bool _inSpecificMode = false;

    public Color UnselectedColor;

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
        OverlayButtons = new TimelineGasOverlayButton[pollutants.Length];

        for (int i = 0; i < pollutants.Length; i++)
        {
            GameObject overlayObj = Instantiate(OverlayPrefab);
            overlayObj.transform.SetParent(GasOverlayButtonParent, false);

            TimelineGasOverlayButton overlay = overlayObj.GetComponent<TimelineGasOverlayButton>();
            overlay.Setup(pollutants[i].Type);
            OverlayButtons[i] = overlay;
        }
    }

    public void HandleOverlayChange(PollutantType pollutant)
    {
        Debug.Log(pollutant);
        Debug.Log(_currentPollutant);
       
        if (pollutant == _currentPollutant)
        {
            if (_inSpecificMode) {
                foreach (var overlay in OverlayButtons)
                {
                    overlay.OverlayImage.color = UnselectedColor;
                }
                _inSpecificMode = false;
            } else
            {
                foreach (var overlay in OverlayButtons)
                {
                    if (overlay.PollutantType == pollutant)
                    {
                        overlay.OverlayImage.color = Color.white;
                    } else {
                        overlay.OverlayImage.color = UnselectedColor;
                    }
                }
                _inSpecificMode = true;
            }
        } 
        else 
        {
            foreach (var overlay in OverlayButtons)
            {
                if (overlay.PollutantType == pollutant)
                {
                    overlay.OverlayImage.color = Color.white;
                } else {
                    overlay.OverlayImage.color = UnselectedColor;
                }
            }
            _inSpecificMode = true;
        }

        _currentPollutant = pollutant;

        InvestigationTimelineChunk.OnOverlayChange?.Invoke((_inSpecificMode, _currentPollutant));
    }
}
