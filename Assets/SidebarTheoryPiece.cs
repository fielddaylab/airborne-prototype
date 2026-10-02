using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarTheoryPiece : MonoBehaviour
{
    public Image BaseImage;
    public TMP_Text TextOverlay;
    public Image ImageOverlay;

    public enum TheoryType { PresentWhenActive, PresentFirstActive, PresentMajorSymptom, PresentWithSymptom }

    public TheoryType PieceType;

    public void Awake()
    {
        
    }
}
