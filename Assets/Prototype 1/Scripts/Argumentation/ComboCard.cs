using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboCard : MonoBehaviour
{
    [SerializeField] private RawImage _topImage, _bottomImage;
    
    public int PersuasionValue = 1;

    public void Setup(int value, Texture symptomImage, Texture sourceImage)
    {
        PersuasionValue = value;
        _topImage.texture = symptomImage;
        _bottomImage.texture = sourceImage;
    }

    public void Remove()
    {
        PersuasionManager.Instance.UpdateAmount(-PersuasionValue);
        Destroy(gameObject);
    }
}
