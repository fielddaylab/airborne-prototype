using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlowModel {

    public class FlowRoom : MonoBehaviour {
        public string RoomId;
        public int RoomSize;   // capacity for gas units
        public bool IsOutside; // "outside" room has unlimited fresh air

        private List<PollutantType> ObservedGases;                      // gathered sensor readings
        [HideInInspector] public List<PollutantType> ModeledGases = new List<PollutantType>();      // gases predicted to be present based on model
        [HideInInspector] public List<FlowSource> RoomObjects;        // objects present in this room
        [HideInInspector] public List<FlowConnection> Connections;    // connections to other rooms

        public FlowRoomVisual Visual;

        public void Start() {
            for (int i = 0; i < RoomSize; i++) {
                ModeledGases.Add(PollutantType.FreshAir);
            }
            Visual.InitializeVisual();
        }

        #region Add Gas
        public void AddGasUnitLate(PollutantType gasType) {
            if (IsOutside) {
                return;
            } else if (gasType == PollutantType.None) {
                Debug.Log("[FlowRoom.AddGasUnitLate] 'None' added, skipping...");
            }
                FlowController.Instance.FlowQueue.AddEvent(FlowChangeEventType.Add, this, gasType);
        }

        public void AddGasUnitInstant(PollutantType gasType) {
            if (IsOutside) {
                return;
            }
            ModeledGases.Add(gasType);
        }

        #endregion// Add Gas

        #region Remove Gas
        public PollutantType RemoveGasUnitAt(int idx) {
            if (IsOutside) {
                return PollutantType.FreshAir;
            }
            if (idx < 0) {
                return PollutantType.None;
            }
            PollutantType gasOut = ModeledGases[idx];
            ModeledGases.RemoveAt(idx);
            return gasOut;
        }

        public bool RemoveGasUnit(PollutantType gas) {
            if (IsOutside) {
                return true;
            }
            return ModeledGases.Remove(gas);
        }
        #endregion // Remove Gas

        // might be better with a sorted list?
        // not worrying about it right now
        public FlowConnection ChooseRankedConnection(bool includeClosed = false) {
            if (Connections.Count <= 0) {
                return null;
            }
            List<FlowConnection> openConnections = Connections.FindAll(r => r.Open);
            if (openConnections.Count > 0) {
                // first rank: open unidirectional connections
                List<FlowConnection> openUniConnections = openConnections.FindAll(r => r.Unidirectional);
                if (openUniConnections.Count > 0) {
                    return openUniConnections[Random.Range(0, openUniConnections.Count)];
                } else {
                    // second rank: all open connections
                    return openConnections[Random.Range(0, openConnections.Count)];
                }
            } else if (includeClosed) {
                // third rank: closed connections
                return Connections[Random.Range(0, Connections.Count)];
            }
            return null;
        }

        public PollutantType ChooseRandGasUnit(out int idx) {
            idx = -1;
            if (IsOutside) {
                idx = 0;
                return PollutantType.FreshAir;
            }
            if (ModeledGases.Count <= 0) {
                return PollutantType.None;
            }
            idx = Random.Range(0, ModeledGases.Count);
            return ModeledGases[idx];
        }
    }
}
