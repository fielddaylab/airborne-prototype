using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Source of pollutants in the Flow model simulation.
/// </summary>
/// 
namespace FlowModel {
    [RequireComponent(typeof(Button))]
    public class FlowSource : MonoBehaviour {
        //public bool SourceVisible = false;
        public bool SourceActive = false;
        public Pollutant Pollutant;
        [SerializeField] private PollutionSource ObjectType;
        public FlowRoom Room;

        public Button ObjectButton;

        [Header("Visuals")]
        [SerializeField] private Image Icon;
        [SerializeField] private Image Background;

        // when pollutant set to non fresh air, add to active sources
        public void AddToSourceList() {
            FlowController.Instance.ActiveSources.Add(this);
        }

        public void OnPressButton() {
            FlowController.Instance.SourceMenu.MoveMenuTo(this);
        }

        public void SetSourceGas(Pollutant gas) {
            Pollutant = gas;
            Background.color = FlowVisualsLibrary.GetGasColor(gas);
            if (gas == Pollutant.FreshAir || gas == Pollutant.None) {
                FlowController.Instance.ActiveSources.Remove(this);
            } else {
                FlowController.Instance.ActiveSources.Add(this);
            }
        }
    }
}