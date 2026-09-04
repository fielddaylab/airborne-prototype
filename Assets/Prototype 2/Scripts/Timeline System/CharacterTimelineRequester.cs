using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTimelineRequester : TimelineRequester
{
    public CharacterType Character;
    
    public override void RequestTimelineDisplay()
    {
        PlayerInvestigationTimeline.OnTimelineRequested?.Invoke(Character);
    }
}
