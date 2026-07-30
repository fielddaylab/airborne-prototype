using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlowModel {
    public class FlowPlayToggle : MonoBehaviour {

        public bool IsPlaying = false;
        public Toggle Toggle;
        public TextMeshProUGUI ButtonText;

        public void TogglePlaying(bool playing) {
            if (playing) {
                ButtonText.text = "Pause";
                FlowController.ToggleFlow(playing);
            } else {
                ButtonText.text = "Play";
                FlowController.ToggleFlow(playing);
            }
        }
    }
}
