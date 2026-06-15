using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterManager : MonoBehaviour
{
    // where the object will go when not in use
    public Vector3 HiddenLocation = new Vector3(0, -100, 0);
    public static Action<Vector3> OnShowMeter;

    public Button XButton;
    public Button[] PollutantPortraits;

    void Start()
    {
        HideDialogue();
    }

    void OnEnable()
    {
        XButton.onClick.AddListener(HideDialogue);
        OnShowMeter += ShowDialogue;
    }

    void OnDisable()
    {
        XButton.onClick.RemoveAllListeners();
        OnShowMeter -= ShowDialogue;
    }
    
    public void HideDialogue()
    {
        transform.position = HiddenLocation;
    }

    public void ShowDialogue(Vector3 position)
    {
        transform.position = position;
    }
}
