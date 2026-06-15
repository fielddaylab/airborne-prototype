

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowModel {
    public class FlowVisualsLibrary : MonoBehaviour {
        public static FlowVisualsLibrary Instance;

        [Header("Gas Units")]
        public List<GasColorPair> GasUnitColors;
        [Header("Connections")]
        public Color OpenConnectionColor;
        public List<ConnectionIcon> ConnectionIcons;
        public Color ClosedConnectionColor;

        public void Start() {
            if (Instance == null) {
                Instance = this;
            }
        }

        public static void GetConnectionVisual(FlowConnection connection, out Color connColor, out Sprite connIcon) {
            if (connection.Open) {
                connColor = Instance.OpenConnectionColor;
                connIcon = Instance.ConnectionIcons.Find(entry => entry.Open && entry.Type == connection.ConnectionType).Icon;
            } else {
                connColor = Instance.ClosedConnectionColor;
                connIcon = Instance.ConnectionIcons.Find(entry => !entry.Open && entry.Type == connection.ConnectionType).Icon;
            }
        }

        public static Color GetGasColor(PollutantType gas) {
            return Instance.GasUnitColors.Find(pair => pair.Gas.Equals(gas)).Color;
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
        public PollutantType Gas;
        public Color Color;
    }
}