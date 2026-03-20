using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvidencePiece : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    
    public void SetText(string text)
    {
        displayText.text = text;
    }
}
