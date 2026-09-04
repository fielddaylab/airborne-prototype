using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TimelineRequester : MonoBehaviour
{
    private Button _myButton;

    void Awake()
    {
        _myButton = GetComponent<Button>();
    }

    public void OnEnable()
    {
        _myButton.onClick.AddListener(RequestTimelineDisplay);
    }

    public void OnDisable()
    {
        _myButton.onClick.RemoveListener(RequestTimelineDisplay);
    }

    public abstract void RequestTimelineDisplay();
}
