using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prompter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;

    public void Start()
    {
        UpdatePrompt(0);
    }

    public void UpdatePrompt(int time)
    {
        promptText.text = $"How did the <b>Carbon Monoxide</b> move through the building at <b>{time + 1}PM</b>?";
    }
}
