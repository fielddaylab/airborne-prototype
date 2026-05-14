using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {
    public int TimeIdx;
    public List<FlowRoom> Rooms;
    public List<FlowSource> Sources;

    public static void ProcessRooms(FlowController controller) {
        AddPollutants(controller);
        foreach (FlowRoom room in controller.Rooms) {
            //RoomOverflow(room);
            RoomDiffusion(room);
        }
    }

    private static void AddPollutants(FlowController controller) {
        // add pollutants
        foreach (FlowSource source in controller.Sources) {
            if (source.SourceActive && source.Room.ModeledGases.Count < source.Room.RoomSize-1) { 
                    source.Room.AddGasUnitLate(source.Pollutant);
            }          
        }
    }

    // TODO: room overflow logic?
    private static void RoomOverflow(FlowRoom room) {
        // overflow: excess gas must be moved to adjacent rooms.
        // how to ensure that all overflows are resolved?
        while (room.ModeledGases.Count > room.RoomSize) {
            // if there's fresh air, just remove it.
            // simplification to avoid shuffling around mostly fresh air
            if (!room.ModeledGases.Remove(Pollutant.FreshAir)) {
                // otherwise, move a pollutant to connected room.
                // choose random connection
                FlowConnection connection = room.ChooseRankedConnection();
                // move random gas unit
                connection.MoveGasFrom(room);
            }          
        }
    }

    private static void RoomDiffusion(FlowRoom room) {
        // choose random connection, swap random unit
        room.ChooseRankedConnection().SwapGasUnit();
    }
}