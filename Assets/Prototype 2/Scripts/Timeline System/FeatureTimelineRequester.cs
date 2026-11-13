using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureTimelineRequester : TimelineRequester
{
    public FeatureType Feature;
    
    public override void RequestTimelineDisplay()
    {
        PlayerTimeline.OnTimelineRequested?.Invoke(Feature);
    }
}
