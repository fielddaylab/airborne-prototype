

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlowRoomVisual : MonoBehaviour {
    [SerializeField] private FlowRoom Room;

    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private List<FlowGasUnitVisual> GasUnits;
    // TODO: list of RoomObjects?

    public void Start() {
        Room = GetComponent<FlowRoom>();
        TitleText.SetText(Room.RoomId);
    }

    // TODO
    public void UpdateGasUnits() {
        // ensure gas unit objects = room capacity
        // iterate thru FlowRoom modeled gases, color gas unit graphics according to pollutant colors
    }
}