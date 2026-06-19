using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    // where the object will go when not in use
    public Vector3 HiddenLocation = new Vector3(0, -100, 0);
    public static Action<Vector3, InvestigationRoom> OnShowMeter;

    public Button XButton;
    public Button COButton, VOCButton, NOButton, O3Button;

    public GameObject MeterObject;

    public int numMeters = 4;

    private InvestigationRoom _sourceRoom;

    public Image[] meterPips;
    public Sprite FullPip, UsePip, EmptyPip;

    void Start()
    {
        HideDialogue();
    }

    void OnEnable()
    {
        XButton.onClick.AddListener(HideDialogue);
        OnShowMeter += ShowDialogue;

        COButton.onClick.AddListener(() => PlaceMeter(PollutantType.CO2));
        VOCButton.onClick.AddListener(() => PlaceMeter(PollutantType.VOC));
        NOButton.onClick.AddListener(() => PlaceMeter(PollutantType.NO));
        O3Button.onClick.AddListener(() => PlaceMeter(PollutantType.O3));
    }

    void OnDisable()
    {
        XButton.onClick.RemoveAllListeners();
        OnShowMeter -= ShowDialogue;

        COButton.onClick.RemoveAllListeners();
        VOCButton.onClick.RemoveAllListeners();
        NOButton.onClick.RemoveAllListeners();
        O3Button.onClick.RemoveAllListeners();
    }
    
    public void HideDialogue()
    {
        transform.position = HiddenLocation;
        if (numMeters <= 0) return;
        meterPips[numMeters - 1].sprite = FullPip;
    }

    public void ShowDialogue(Vector3 position, InvestigationRoom sourceRoom)
    {
        if (numMeters <= 0) return;
        if (sourceRoom.NumMeters >= 2) return;

        meterPips[numMeters - 1].sprite = UsePip;

        transform.position = position;
        _sourceRoom = sourceRoom;
    }

    public void PlaceMeter(PollutantType pollutantType)
    {
        if (numMeters <= 0) return;
        meterPips[numMeters - 1].sprite = EmptyPip;
        numMeters--;
        _sourceRoom.NumMeters++;
        
        GameObject meter = Instantiate(MeterObject);
        meter.transform.position = transform.position;
        
        GasMeter gasMeter = meter.GetComponent<GasMeter>();

        gasMeter.TrackedRoom = _sourceRoom;
        gasMeter.TrackedPollutant = pollutantType;
        gasMeter.Label.text = pollutantType.ToString();

        InvestigationTimelineSystem.Instance.RegisterMeter(gasMeter);

        HideDialogue();
    }
}
