using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] private Sprite checkMark;
    [SerializeField] private Sprite xMark;

    private Image rawImage;
    private int cycleState = 0;
    
    void Awake()
    {
        rawImage = GetComponent<Image>();
        rawImage.enabled = false;
    }

    public void IncrementCycle()
    {
        if (GameManager.Instance != null) {
            if (GameManager.Instance.GamePhase != GamePhase.SelectingPollutant) return;
        }
        
        cycleState = (cycleState + 1) % 3;
        switch (cycleState)
        {
            case 0:
                rawImage.enabled = false;
                rawImage.sprite = null;
                break;
            case 1:
                rawImage.enabled = true;
                rawImage.sprite = checkMark;
                break;
            case 2:
                rawImage.sprite = xMark;
                break;
        }

    }

    public void SetCycle(int i)
    {
        cycleState = i;
        switch (cycleState)
        {
            case 0:
                rawImage.enabled = false;
                rawImage.sprite = null;
                break;
            case 1:
                rawImage.enabled = true;
                rawImage.sprite = checkMark;
                break;
            case 2:
                rawImage.sprite = xMark;
                break;
        }
    }
}
