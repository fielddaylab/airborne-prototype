using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private BoxCollider m_BoxCollider;
    [SerializeField] private Transform m_TargetAnchor;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(player.JustTeleported) return;
            player.JustTeleported = true;

            player.PlayerCharacterController.enabled = false;
            other.transform.position = m_TargetAnchor.position;
            player.PlayerCharacterController.enabled = true;
        }
    }
}
