using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBlockButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private DiffusionManager diffusionManager;

    void Start()
    {
        button.interactable = false;
    }
    
    public void ProgressToNextTimeBlock()
    {
        diffusionManager.AdvanceToNextTimeBlock();
    }

    public void AllowAdvance(bool val)
    {
        button.interactable = val;
    }
}
