

using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        if (Room.IsOutside || Room.RoomSize < 0) {
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
        TitleText.SetText(Room.RoomId);
        PopulateGasUnits();
    }

    public void UpdateGasUnits() {
        if (Room.ModeledGases.Count != GasUnits.Count) {
            PopulateGasUnits();
        }
        // iterate thru FlowRoom modeled gases, color gas unit graphics according to pollutant colors
        FlowVisualsLibrary lib = FlowVisualsLibrary.Instance;
        for (int i = 0; i < Room.ModeledGases.Count; i++) {
            GasUnits[i].Graphic.color = lib.GasUnitColors.Find(entry => entry.Gas == Room.ModeledGases[i]).Color; 
        }
    }
}