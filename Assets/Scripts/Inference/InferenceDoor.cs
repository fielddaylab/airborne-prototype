using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    Horizontal,
    Vertical
}

public class InferenceDoor : MonoBehaviour
{
    // [SerializeField] private InferenceRoom m_Room1, m_Room2;
    // [SerializeField] private DoorType m_DoorType;
    // [SerializeField] private BoxCollider m_BoxCollider;

    // private float _horizontalDistance;
    // private bool _inContact = false;

    // private void Start()
    // {
    //     _horizontalDistance = m_BoxCollider.bounds.extents.x * 2;
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         if (_inContact) return;
    //         _inContact = true;
            
    //         PlayerController player = other.gameObject.GetComponent<PlayerController>();

    //         player.currentRoom = (player.currentRoom == m_Room1) ? m_Room2 : m_Room1;
    //         player.playerCamera.UpdateRoom(player.currentRoom);

    //         switch (m_DoorType)
    //         {
    //             case DoorType.Horizontal:
    //                 if (other.gameObject.transform.position.x > transform.position.x + _horizontalDistance / 2)
    //                 {
    //                     //player.PlayerCharacterController.enabled = false;
    //                     other.gameObject.transform.position -= new Vector3(_horizontalDistance, 0, 0); 
    //                    // player.PlayerCharacterController.enabled = true;
    //                 } else
    //                 {
    //                     //player.PlayerCharacterController.enabled = false;
    //                     other.gameObject.transform.position += new Vector3(_horizontalDistance, 0, 0); 
    //                     //player.PlayerCharacterController.enabled = true;
    //                 }
    //                 break;
    //             case DoorType.Vertical:
    //                 if (other.gameObject.transform.position.y > 0)
    //                 {
    //                    // player.PlayerCharacterController.enabled = false;
    //                     other.gameObject.transform.position -= new Vector3(0, 8, 0);
    //                     //player.PlayerCharacterController.enabled = true;
    //                 }
    //                 else
    //                 {
    //                     //player.PlayerCharacterController.enabled = false;
    //                     other.gameObject.transform.position += new Vector3(0, -8, 0);
    //                     //player.PlayerCharacterController.enabled = true;
    //                 }
    //                 break;
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         _inContact = false;
    //     }
    // }
}
