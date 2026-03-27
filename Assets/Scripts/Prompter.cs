using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prompter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] DiffusionManager diffusionManager;

    public void Update()
    {
        promptText.text = $"How did the <b>Carbon Monoxide</b> move through the building at <b>{diffusionManager.time + 1}PM</b>?";
    }
}
