using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SourceButton : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Label;
    public Button MyButton;
    
    public void Setup(FeatureType type)
    {
        Icon.sprite = FeatureSpriteMapUtility.GetOnSprite(InvestigationLookup.Instance.FeatureSpriteMap, type);
        Label.text = type.ToString();
    }
}
