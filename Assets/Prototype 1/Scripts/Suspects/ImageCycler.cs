using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCycler : MonoBehaviour
{
    [SerializeField] private Sprite checkMark;
    [SerializeField] private Sprite xMark;

    public Image Image;

    public void Awake()
    {
        Image.enabled = false;
    }

    public void SetChecked(bool check)
    {
        Image.enabled = check;
        Image.sprite = checkMark;
    }
}
