using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private DiffusionManager diffusionManager;
    [SerializeField] private ConnectionController[] connections;

    public static event EventHandler RoomSelected;
    
    public void SelectThisRoom()
    {
        RoomSelected?.Invoke(this, EventArgs.Empty);
        diffusionManager.SelectedRoom(this);
    }

    public RoomController[] FindAndEnableConnections()
    {
        List<RoomController> connectedRooms = new List<RoomController>();
        
        foreach (var connection in connections)
        {
            connection.HighlightConnections();
            
            foreach (RoomController room in connection.connectedRooms)
            {
                if (room != this)
                {
                    connectedRooms.Add(room);
                }
            }
        }

        return connectedRooms.ToArray();
    }

    public void DisableConnections()
    {
        foreach (var connection in connections)
        {
            connection.HideConnections();
        }
    }

    public string FindConnectionName(RoomController targetRoom)
    {
        if (targetRoom == this) return null;
        foreach (var connection in connections)
        {
            foreach (var room in connection.connectedRooms)
            {
                if (room == targetRoom)
                {
                    return connection.connectionName;
                }
            }
        }

        Debug.LogWarning("Could not find a connection to this room.");
        return "UNKNOWN";
    }
}
