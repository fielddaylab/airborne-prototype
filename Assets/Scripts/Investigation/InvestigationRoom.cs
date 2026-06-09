using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigationRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider;
    public string RoomName;
    public float Size;
    public bool PlayerOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOccupied = true;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.currentRoom = this;
            player.playerCamera.UpdateRoom(player.currentRoom);
            InvestigationTimeline.Instance.SetRoom(this);
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
