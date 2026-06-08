using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InferenceRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider;
    public bool PlayerOccupied = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerOccupied = true;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.currentRoom = this;
            player.playerCamera.UpdateRoom(player.currentRoom);
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
