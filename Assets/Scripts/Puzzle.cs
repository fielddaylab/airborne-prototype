using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private EvidenceBucket evidenceBucket;
    [SerializeField] private TextMeshProUGUI promptText;
    
    void Start()
    {
        
    }

    public void SourceSet()
    {
        evidenceBucket.ClearText();
        promptText.text = "How did the Carbon Monoxide move through the building? Let's start from 4PM.";
    }
}
