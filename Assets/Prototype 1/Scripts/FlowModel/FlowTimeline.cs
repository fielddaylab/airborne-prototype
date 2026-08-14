
using UnityEngine;
using UnityEngine.UI;

namespace FlowModel {
    public class FlowTimeline : MonoBehaviour {
        [HideInInspector] public int TimeIdx = 0;
        public int TimeSteps;
        public Timer StepTimer;
        public Slider ProgressSlider;
        public FlowPlayToggle PlayToggle;

        public void Start() {
            ProgressSlider.maxValue = TimeSteps;
            ProgressSlider.minValue = 0;
            ProgressSlider.interactable = false;
        }

        public bool Step() {
            if (TimeIdx + 1 < TimeSteps) {
                TimeIdx++;
                Debug.Log("[FlowTimeline] Step " + TimeIdx);
                ProgressSlider.value = TimeIdx;
                return true;
            }
            PlayToggle.TogglePlaying(false);
            return false;
        }
    }
}