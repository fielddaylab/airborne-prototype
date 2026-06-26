using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceSelector : MonoBehaviour
{
    public Button CloseButton;
    
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
}
