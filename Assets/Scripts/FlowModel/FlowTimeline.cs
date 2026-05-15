
using UnityEngine;

public class FlowTimeline : MonoBehaviour {
    [HideInInspector] public int TimeIdx = 0;
    public int TimeSteps;
    public bool Play;
    public Timer StepTimer;

    public void Update() {
        if (StepTimer.Advance(Time.deltaTime)) {
            AdvanceTime();
        }
    }

    public bool AdvanceTime() {
        if (TimeIdx + 1 < TimeSteps)  {
            TimeIdx++;
            return true;
        } 
        return false;
    }
}