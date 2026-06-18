using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationMap : MonoBehaviour
{
    public MapRoomDisplay[] MapRooms;

    public void UpdateRooms()
    {
        foreach (var room in MapRooms)
        {
            room.UpdateDisplay(0);
        }
    }
}
