using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceSelector : MonoBehaviour
{
    public Button CloseButton;
    public FeatureButtonPair[] FeatureButtons;

    public static event Action<FeatureType> OnSourceSelection;
    
    void OnEnable()
    {
        CloseButton.onClick.AddListener(CloseMenu);

        foreach (var pair in FeatureButtons)
        {
            pair.FeatureButton.onClick.AddListener(() => HandleSourceSelection(pair));
        }
    }

    void OnDisable()
    {
        CloseButton.onClick.RemoveListener(CloseMenu);

        foreach (var pair in FeatureButtons)
        {
            pair.FeatureButton.onClick.RemoveAllListeners();
        }
    }

    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    private void HandleSourceSelection(FeatureButtonPair pair)
    {
        OnSourceSelection?.Invoke(pair.Feature);
        CloseMenu();
    }
}

[System.Serializable]
public class FeatureButtonPair
{
    public Button FeatureButton;
    public FeatureType Feature;
}