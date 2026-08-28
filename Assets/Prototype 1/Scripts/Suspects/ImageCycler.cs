using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] private Sprite checkMark;
    [SerializeField] private Sprite xMark;

    public Image Image;
    private int cycleState = 0;
    
    void Start()
    {
        Image.enabled = false;
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
                Image.enabled = false;
                Image.sprite = null;
                break;
            case 1:
                Image.enabled = true;
                Image.sprite = checkMark;
                break;
            case 2:
                Image.sprite = xMark;
                break;
        }

    }

    public void SetCycle(int i)
    {
        cycleState = i;
        switch (cycleState)
        {
            case 0:
                Image.enabled = false;
                Image.sprite = null;
                break;
            case 1:
                Image.enabled = true;
                Image.sprite = checkMark;
                break;
            case 2:
                Image.sprite = xMark;
                break;
        }
    }
}
