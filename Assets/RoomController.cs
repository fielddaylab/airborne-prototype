using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] GameObject dialogueMenu;
    [SerializeField] Vector3 dialoguePositionOffset;


    public void SpawnMenu()
    {
        dialogueMenu.SetActive(true);
        dialogueMenu.transform.position = transform.position + dialoguePositionOffset;
    }
}
