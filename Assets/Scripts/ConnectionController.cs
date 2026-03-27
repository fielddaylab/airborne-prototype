using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviour
{
    [SerializeField] public RoomController[] connectedRooms;
    [SerializeField] private Image[] connectionHighlights;
    [SerializeField] private Button[] buttons;
    public string connectionName = "door";


    void Start()
    {
        HideConnections();
    }

    public void HighlightConnections()
    {
        for (int i = 0; i < connectionHighlights.Length; i++)
        {
            Color highlightColor = connectionHighlights[i].color;
            highlightColor.a = 255;

            connectionHighlights[i].color = highlightColor;

            buttons[i].interactable = true;
        }

    }

    public void HideConnections()
    {
        for (int i = 0; i < connectionHighlights.Length; i++)
        {
            Color highlightColor = connectionHighlights[i].color;
            highlightColor.a = 0;

            connectionHighlights[i].color = highlightColor;
            buttons[i].interactable = false;
        }

    }
}
