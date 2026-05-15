

using System;
using System.Collections.Generic;
using UnityEngine;

public class FlowVisualsLibrary : MonoBehaviour {
    public static FlowVisualsLibrary Instance;

    [Header("Gas Units")]
    public List<GasColorPair> GasUnitColors;
    [Header("Connections")]
    public Color OpenConnectionColor;
    // maybe should just use a list of structs with type, bool open, and sprite
    public List<ConnectionIcon> ConnectionIcons;
    public Color ClosedConnectionColor;

    public static void GetConnectionVisual(FlowConnection connection, out Color connColor, out Sprite connIcon) {
        if (connection.Open) {
            connColor = Instance.OpenConnectionColor;
            connIcon = Instance.ConnectionIcons.Find(entry => entry.Open && entry.Type == connection.ConnectionType).Icon;
        } else {
            connColor = Instance.ClosedConnectionColor;
            connIcon = Instance.ConnectionIcons.Find(entry => !entry.Open && entry.Type == connection.ConnectionType).Icon;
        }
    } 
}

[Serializable]
public struct ConnectionIcon {
    public ConnectionType Type;
    public bool Open;
    public Sprite Icon;
}
[Serializable]
public struct GasColorPair {
    public Pollutant Gas;
    public Color Color;
}