using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineButton : MonoBehaviour
{
    private FeedbackBlock _feedbackBlock;

    private string _timeSegment;
    private string _part1, _part2;
    private Texture2D _imageTexture;

    public void Setup(FeedbackBlock feedbackBlock, string timeSegment, string part1, Texture2D image = null, string part2 = null)
    {
        _feedbackBlock = feedbackBlock;
        _timeSegment = timeSegment;
        _part1 = part1;
        _imageTexture = image;
        _part2 = part2;
    }

    public void ShowFeedback()
    {
        _feedbackBlock.ShowFeedback(_timeSegment, _part1, _imageTexture, _part2);
    }
 }
