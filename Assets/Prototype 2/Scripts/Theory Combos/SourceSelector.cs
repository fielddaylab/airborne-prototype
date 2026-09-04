using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceSelector : MonoBehaviour
{
    public Button CloseButton;
    public GameObject SourceButtonPrefab;
    public Transform SourceButtonParent;

    public static event Action<FeatureType> OnSourceSelection;

    public void Start()
    {
        Setup();
    }

    public void Setup()
    {
        Debug.Log("Setup on SourceSelector was called!");
        
        ScenarioDataObject scenarioData = InvestigationTimelineSystem.Instance.ScenarioData;

        foreach (var featureEvent in scenarioData.FeatureEvents)
        {
            if (featureEvent.isPolluter)
            {
                FeatureType type = featureEvent.FeatureType;

                GameObject sourceButton = Instantiate(SourceButtonPrefab);
                sourceButton.transform.SetParent(SourceButtonParent, false);

                SourceButton source = sourceButton.GetComponent<SourceButton>();
                source.Setup(type);

                source.MyButton.onClick.AddListener(() => HandleSourceSelection(type));
            }
        }
    }
    
    void OnEnable()
    {
        CloseButton.onClick.AddListener(CloseMenu);
    }

    void OnDisable()
    {
        CloseButton.onClick.RemoveListener(CloseMenu);
    }

    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    private void HandleSourceSelection(FeatureType feature)
    {
        OnSourceSelection?.Invoke(feature);
        CloseMenu();
    }
}
