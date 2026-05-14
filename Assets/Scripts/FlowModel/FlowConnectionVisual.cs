

using UnityEngine;
using UnityEngine.UI;

public class FlowConnectionVisual : MonoBehaviour {
    FlowConnection Connection;
    [SerializeField] private Graphic ConnectionBackground;
    [SerializeField] private Image ConnectionIcon;

    public void UpdateConnectionVisual() {
        FlowVisualsLibrary.GetConnectionVisual(Connection, out Color connColor, out Sprite connSprite);
        ConnectionBackground.color = connColor;
        ConnectionIcon.sprite = connSprite;
    }
}