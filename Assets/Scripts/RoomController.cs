using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private DiffusionManager diffusionManager;
    [SerializeField] private ConnectionController[] connections;
    
    public void SelectThisRoom()
    {
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
}
