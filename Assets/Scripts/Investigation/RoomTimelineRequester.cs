using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTimelineRequester : TimelineRequester
{
    public RoomType RoomType;
    
    public override void RequestTimelineDisplay()
    {
        Debug.Log(RoomType.ToString());
    }
}
