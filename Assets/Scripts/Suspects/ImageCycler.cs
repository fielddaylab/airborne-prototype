using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] private Texture checkMark;
    [SerializeField] private Texture xMark;

    private RawImage rawImage;
    private int cycleState = 0;
    
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.enabled = false;
    }

    public void IncrementCycle()
    {
        if (GameManager.Instance.GamePhase != GamePhase.SelectingPollutant) return;
        
        cycleState = (cycleState + 1) % 3;
        switch (cycleState)
        {
            case 0:
                rawImage.enabled = false;
                rawImage.texture = null;
                break;
            case 1:
                rawImage.enabled = true;
                rawImage.texture = checkMark;
                break;
            case 2:
                rawImage.texture = xMark;
                break;
        }

    }
}
