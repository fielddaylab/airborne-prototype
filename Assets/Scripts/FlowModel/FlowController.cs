using System.Collections.Generic;
using UnityEngine;

namespace FlowModel {
    public class FlowController : MonoBehaviour {
        public static FlowController Instance;
        public List<FlowRoom> Rooms;
        public List<FlowSource> ActiveSources;
        [HideInInspector] public FlowEventQueue FlowQueue = new FlowEventQueue();

        public FlowTimeline Timeline;

        public GameObject GasUnitPrefab;

        public FlowSourceMenu SourceMenu;

        public void Start() {
            Instance = this;

        }

        public void Update() {
            FlowStep();
        }

        public static void FlowStep() {
            if (Instance.Timeline.StepTimer.Advance(Time.deltaTime)) {
                if (Instance.Timeline.Step()) {
                    ProcessRooms();
                }
            }
        }

        public static void ToggleFlow(bool toggle) {
            Instance.Timeline.StepTimer.Active = toggle;
        }


        public static void ProcessRooms() {
            Debug.Log("[FlowController] Processing Rooms...");
            AddPollutants();
            foreach (FlowRoom room in Instance.Rooms) {
                //RoomOverflow(room);
                RoomDiffusion(room);
            }

            Instance.FlowQueue.ProcessEventQueue(false);

            // make sure we update visuals only once every room has diffused.
            FlowVisualsLibrary lib = FlowVisualsLibrary.Instance;
            foreach (FlowRoom room in Instance.Rooms) {
                room.Visual.UpdateGasUnits();
            }
        }

        private static void AddPollutants() {
            // add pollutants
            Debug.Log("[FlowController] Adding Pollutants...");
            foreach (FlowSource source in Instance.ActiveSources) {
                if (source.Room.ModeledGases.Count < source.Room.RoomSize || source.Room.RemoveGasUnit(Pollutant.FreshAir)) {
                    source.Room.AddGasUnitLate(source.Pollutant); 
                } else {
                    Debug.Log("[FlowController] No space in room " + source.Room.RoomId);
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
                    FlowConnection connection = room.ChooseRankedConnection(true);
                    // move random gas unit
                    connection.MoveGasFrom(room);
                }
            }
        }

        private static void RoomDiffusion(FlowRoom room) {
            // choose random connection, swap random unit
            FlowConnection randConn = room.ChooseRankedConnection(false);
            if (randConn != null) {
                randConn.SwapGasUnit();
            }
        }
    }
}