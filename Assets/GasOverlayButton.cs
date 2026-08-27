using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasOverlayButton : MonoBehaviour
{
    public Image OverlayImage;
    public Button OverlayButton;
    public PollutantType PollutantType;

    public void Setup(PollutantType pollutant)
    {
        PollutantType = pollutant;
        OverlayImage.sprite = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutant);
        OverlayButton.onClick.AddListener(HandleSwitch);
    } 

    public void HandleSwitch()
    {
        GasOverlayManager.OnOverlayChange?.Invoke(PollutantType);
    }
}
