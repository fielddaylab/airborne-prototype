using System.Collections.Generic;
using UnityEngine;

public class FlowController : MonoBehaviour {
    public static FlowController Instance;
    public List<FlowRoom> Rooms;
    public List<FlowSource> ActiveSources;
    [HideInInspector] public FlowEventQueue FlowQueue;

    public FlowTimeline Timeline;

    public GameObject GasUnitPrefab;

    public void Start() {
        if (Instance == null) {
            Instance = this;
        }
    }


    [ContextMenu("Initialize Rooms")]
    private void InitializeRooms() {
        if (Instance == null) {
            Instance = this;
        }
        foreach (FlowRoom room in Instance.Rooms) {
            room.Visual.InitializeVisual();
        }
    }

    public static void FlowStep() {
        if (Instance.Timeline.AdvanceTime()) {
            ProcessRooms();
        }      
    }

    public static void ProcessRooms() {
        AddPollutants();
        foreach (FlowRoom room in Instance.Rooms) {
            //RoomOverflow(room);
            RoomDiffusion(room);
        }

        Instance.FlowQueue.ProcessEventQueue();

        // make sure we update visuals only once every room has diffused.
        foreach (FlowRoom room in Instance.Rooms) {
            room.Visual.UpdateGasUnits();
        }
    }

    private static void AddPollutants() {
        // add pollutants
        foreach (FlowSource source in Instance.ActiveSources) {
            if (source.Room.ModeledGases.Count < source.Room.RoomSize-1) { 
                    source.Room.AddGasUnitLate(source.Pollutant);
            }          
        }
    }

    // TODO: room overflow logic?
    // for now, capping transfers and making all transfers single direction.
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