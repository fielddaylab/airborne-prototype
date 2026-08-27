using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterButton : MonoBehaviour
{
    public Button MyButton;
    public Image MyImage;
    public PollutantType PollutantType;

    public void Setup(PollutantType pollutant)
    {
        MyButton.onClick.AddListener(RequestPlaceMeter);
        PollutantType = pollutant;

        MyImage.sprite = InvestigationLookup.Instance.PollutantMap.GetSprite(pollutant);
    }

    public void RequestPlaceMeter()
    {
        MeterManager.OnMeterButton?.Invoke(PollutantType);
    }
}
