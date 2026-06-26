using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TheoryPiece : MonoBehaviour
{
    public Symptom RepresentedSymptom = Symptom.None;
    public FeatureType RepresentedFeature = FeatureType.None;
    
    public Image TheoryImage;
    public ImageCycler Cycler;
}
