using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FlowModel {

    public class FlowConnection : MonoBehaviour {
        public FlowRoom Origin;
        public FlowRoom Destination;
        public ConnectionType ConnectionType;
        public bool Open;
        public bool Unidirectional;

        [Header("Visuals")]
        public Image Icon;
        public Image Background;

        public void Start() {
            if (Origin != null) {
                Origin.Connections.Add(this);
            }
            if (Destination != null) {
                Destination.Connections.Add(this);
            }
            ToggleConnection(true);
        }

        public void MoveGasFrom(FlowRoom room, PollutantType gasType = PollutantType.None) {
            int transferIdx = -1;
            if (gasType == PollutantType.None) {
                room.ChooseRandGasUnit(out transferIdx);
            } else {
                // otherwise, find idx
                transferIdx = room.ModeledGases.IndexOf(gasType);
            }
            MoveGasFrom(room, transferIdx);
        }

        public void ToggleConnection(bool toggle) {
            Open = toggle;
            FlowVisualsLibrary.GetConnectionVisual(this, out Color col, out Sprite icon);
            Background.color = col;
            Icon.sprite = icon;
        }

        public void MoveGasFrom(FlowRoom room, int transferIdx) {
            if (room.Equals(Origin)) {
                MoveGasUnitForward(transferIdx);
            } else if (room.Equals(Destination)) {
                MoveGasUnitReverse(transferIdx, true);
            } else {
                Debug.LogWarning("[FlowConnection] Room '" + room.RoomId + "' not in this connection");
                return;
            }
        }
        /// <summary>
        /// Move gas unit from this connection's origin to its destinantion.
        /// </summary>
        /// <param name="gasIdx">Index of gas unit to remove from origin</param>
        public void MoveGasUnitForward(int gasIdx) {
            Destination.AddGasUnitLate(Origin.RemoveGasUnitAt(gasIdx));
        }

        /// <summary>
        /// Move gas unit from this connection's destination to its origin, if allowed
        /// </summary>
        /// <param name="gasIdx">Index of gas unit to remove from destination</param>
        /// <param name="overrideDirectionality">set true to transfer regardless of this connection's directionality setting</param>
        public void MoveGasUnitReverse(int gasIdx, bool overrideDirectionality = false) {
            if (!Unidirectional || overrideDirectionality) {
                Origin.AddGasUnitLate(Destination.RemoveGasUnitAt(gasIdx));
            }
        }

        //public bool TryMoveGasUnitTo(Pollutant gasType, FlowRoom destination) {
        //    if (ModeledGases.Remove(gasType)) {
        //        destination.AddGasUnitLate(gasType);
        //        return true;
        //    }
        //    return false;
        //}

        //public void MoveGasUnitTo(int moveIdx, FlowRoom destination) {
        //    destination.AddGasUnitLate(RemoveGasUnitAt(moveIdx));
        //}

        public void SwapGasUnit(PollutantType originUnit = PollutantType.None, PollutantType destUnit = PollutantType.None) {
            int destIdx;
            int originIdx;
            if (originUnit == PollutantType.None) {
                // if no pollutants specified, choose random unit           
                originUnit = Origin.ChooseRandGasUnit(out originIdx);
            } else {              
                originIdx = Origin.ModeledGases.IndexOf(originUnit);
            }
            if (destUnit == PollutantType.None) {
                destUnit = Destination.ChooseRandGasUnit(out destIdx);
            } else {
                destIdx = Destination.ModeledGases.IndexOf(destUnit);
            }
            if (originUnit == destUnit) {
                //Debug.Log("[FlowConnection.SwapGasUnit] Identical units chosen, skipping...");
                return;
            }
            MoveGasUnitForward(originIdx);
            MoveGasUnitReverse(destIdx, true);
        }
    }
}
