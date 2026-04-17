using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FeedbackBlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _part1, _part2;
    [SerializeField] RawImage _image;

    public void Start()
    {
        ShowFeedback(null, null, null);
    }

    public void ShowFeedback(string part1, Texture2D texture = null, string part2 = null)
    {
        Debug.Log("blargh!");
        _part1.text = part1;
        _part2.text = part2;
        _image.texture = texture;
    }
}
