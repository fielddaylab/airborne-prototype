using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadingDialogue : MonoBehaviour
{
    [SerializeField] private Vector3 positionOffest;
    [SerializeField] private DiffusionManager diffusionManager;

    public void UpdatePosition(RoomController room)
    {
        transform.position = room.transform.position + positionOffest;
    }
}
