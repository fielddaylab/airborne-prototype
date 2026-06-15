

using UnityEngine;

namespace FlowModel {
    public class SourcePollutantOption : MonoBehaviour {
        public PollutantType Pollutant;
        public FlowSourceMenu SourceMenu;


        public void OnChoosePollutant() {
            SourceMenu.SetSourcePollutant(Pollutant);
        }
    }
} 