using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowRoom : MonoBehaviour {
    public string RoomId;
    public int RoomSize;   // capacity for gas units
    public bool IsOutside; // "outside" room has unlimited fresh air

    private List<Pollutant> ObservedGases;                      // gathered sensor readings
    [HideInInspector] public List<Pollutant> ModeledGases;      // gases predicted to be present based on model
    [HideInInspector] public List<FlowSource> RoomObjects;        // objects present in this room
    [HideInInspector] public List<FlowConnection> Connections;    // connections to other rooms

    public FlowRoomVisual Visual;
    

    #region Add Gas
    public void AddGasUnitLate(Pollutant gasType) {
        FlowController.Instance.FlowQueue.AddEvent(FlowChangeEventType.Add, this, gasType);
    }

    public void AddGasUnitInstant(Pollutant gasType) {
        ModeledGases.Add(gasType);
    }

    #endregion// Add Gas

    #region Remove Gas
    public Pollutant RemoveGasUnitAt(int idx) {
        Pollutant gasOut = ModeledGases[idx];
        ModeledGases.RemoveAt(idx);
        return gasOut;
    }

    public bool RemoveGasUnit(Pollutant gas) {
        return ModeledGases.Remove(gas);
    }
    #endregion // Remove Gas

    // might be better with a sorted list?
    // not worrying about it right now
    public FlowConnection ChooseRankedConnection() {
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
        } else {
            // third rank: closed connections
            return Connections[Random.Range(0, Connections.Count)];
        }
    }
}
