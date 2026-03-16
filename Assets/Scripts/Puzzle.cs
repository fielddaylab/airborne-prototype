using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private PointerDetector[] icons;
    [SerializeField] private PointerDetector[] rooms;
    [SerializeField] private EvidenceBucket evidenceBucket;
    [SerializeField] private TextMeshProUGUI promptText;
    
    void Start()
    {
        foreach (var pointer in rooms)
        {
            pointer.enabled = false;
        }

        foreach (var pointer in icons)
        {
            pointer.enabled = true;
        }
    }

    public void SourceSet()
    {
        foreach (var pointer in icons)
        {
            pointer.enabled = false;
        }

        foreach (var pointer in rooms)
        {
            pointer.enabled = true;
        }

        evidenceBucket.ClearText();
        promptText.text = "How did the Carbon Monoxide move through the building? Let's start from 4PM.";
    }
}
