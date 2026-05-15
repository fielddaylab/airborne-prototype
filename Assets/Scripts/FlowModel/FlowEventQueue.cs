

using System.Collections.Generic;
using UnityEngine;

public class FlowEventQueue : MonoBehaviour {
    public Queue<FlowChangeEvent> Queue;

    public void AddEvent(FlowChangeEventType type, FlowRoom roomA, Pollutant gasA, FlowRoom roomB = null, Pollutant gasB = Pollutant.None) {
        Queue.Enqueue(new FlowChangeEvent() {
            Type = type,
            RoomA = roomA,
            GasA = gasA,
            RoomB = roomB,
            GasB = gasB
        });
    }

    public void ProcessEventQueue(bool debug = false) {
        while (Queue.Count > 0) {
            FlowChangeEvent currEvent = Queue.Dequeue();
            ProcessFlowChangeEvent(currEvent);
            if (debug) {
                currEvent.Print();
            }
        }
    }

    private void ProcessFlowChangeEvent(FlowChangeEvent changeEvent) {
        switch (changeEvent.Type) {
            case FlowChangeEventType.Add:
                changeEvent.RoomA.ModeledGases.Add(changeEvent.GasA);
                break;
            case FlowChangeEventType.Remove:
                changeEvent.RoomA.ModeledGases.Remove(changeEvent.GasA);
                break;
            case FlowChangeEventType.Move:
                changeEvent.RoomA.ModeledGases.Remove(changeEvent.GasA);
                changeEvent.RoomB.ModeledGases.Add(changeEvent.GasA);
                break;
            case FlowChangeEventType.Swap:
                changeEvent.RoomA.ModeledGases.Remove(changeEvent.GasA);
                changeEvent.RoomB.ModeledGases.Remove(changeEvent.GasB);
                changeEvent.RoomB.ModeledGases.Add(changeEvent.GasA);
                changeEvent.RoomA.ModeledGases.Add(changeEvent.GasB);
                break;
            default:
                Debug.LogWarning("[FlowEventQueue] Encountered unimplemented event type: " + changeEvent.Type);
                break;
        }
    }
    
}

public struct FlowChangeEvent {
    public FlowChangeEventType Type;
    public Pollutant GasA;
    public FlowRoom RoomA;
    public Pollutant GasB;
    public FlowRoom RoomB;

    // expensive enum tostring and string concatenation!!! don't do this!!
    public void Print() {
        Debug.Log("[FlowChangeEvent] " + Type.ToString() + " " + GasA.ToString() + " at " + RoomA.RoomId);
    }
}