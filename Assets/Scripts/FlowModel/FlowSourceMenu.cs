using UnityEngine;

namespace FlowModel {
    public class FlowSourceMenu : MonoBehaviour {
        private FlowSource CurrentSource;

        public void MoveMenuTo(FlowSource source) {
            if (source.Equals(CurrentSource)) {
                gameObject.SetActive(false);
                CurrentSource = null;
            } else {
                gameObject.SetActive(true);
                CurrentSource = source;
                transform.SetParent(source.transform, false);
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }             
        }

        public void SetSourcePollutant(Pollutant pollutant) {
            if (CurrentSource != null) {
                CurrentSource.SetSourceGas(pollutant);
                MoveMenuTo(CurrentSource);
            }
        }
    }
}