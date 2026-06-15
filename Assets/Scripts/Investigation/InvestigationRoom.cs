using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider;
    public static event Action<InvestigationRoom> OnRoomUpdated;
    public string RoomName;
    public RoomType RoomTypeValue;
    public float Size;
    public bool PlayerOccupied = false;
    public bool MeterPresent = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOccupied = true;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.currentRoom = this;
            player.playerCamera.UpdateRoom(player.currentRoom);
            OnRoomUpdated?.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOccupied = false;
        }
    }
}
