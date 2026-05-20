

using UnityEngine;

namespace FlowModel {
    public class SourcePollutantOption : MonoBehaviour {
        public Pollutant Pollutant;
        public FlowSourceMenu SourceMenu;


        public void OnChoosePollutant() {
            SourceMenu.SetSourcePollutant(Pollutant);
        }
    }
} 