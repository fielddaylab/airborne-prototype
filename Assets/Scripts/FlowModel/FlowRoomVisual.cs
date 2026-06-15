

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FlowModel {
    public class FlowRoomVisual : MonoBehaviour {
        [SerializeField] private FlowRoom Room;

        [SerializeField] private TextMeshProUGUI TitleText;
        [SerializeField] private List<FlowGasUnitVisual> GasUnits;

        [SerializeField] private Transform GasContainer;
        // TODO: list of RoomObjects?

        public void Start() {
            InitializeVisual();
        }

        public void PopulateGasUnits() {
            if (Room.RoomSize <= 0) {
                GasUnits.Clear();
                return;
            }
            while (GasUnits.Count < Room.RoomSize) {
                GasUnits.Add(Instantiate(FlowController.Instance.GasUnitPrefab, GasContainer).GetComponent<FlowGasUnitVisual>());
            }
            while (GasUnits.Count > Room.RoomSize) {
                DestroyImmediate(GasUnits[0].gameObject);
                GasUnits.RemoveAt(0);
            }
        }

        public void InitializeVisual() {
            Room = GetComponent<FlowRoom>();
            if (Room.IsOutside) return;
            TitleText.SetText(Room.RoomId);
            PopulateGasUnits();
        }

        public void UpdateGasUnits() {     
            if (Room.ModeledGases.Count != GasUnits.Count) {
                PopulateGasUnits();
            }
            // iterate thru FlowRoom modeled gases, color gas unit graphics according to pollutant colors
            FlowVisualsLibrary lib = FlowVisualsLibrary.Instance;
            for (int i = 0; i < GasUnits.Count; i++) {
                GasUnits[i].Type = Room.ModeledGases[i];
                GasUnits[i].Graphic.color = lib.GasUnitColors.Find(entry => entry.Gas == Room.ModeledGases[i]).Color;
                if (GasUnits[i].Type == PollutantType.FreshAir) {
                    GasUnits[i].transform.SetSiblingIndex(0);
                }
            }
        }
    }
}