using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpreadingDialogue : MonoBehaviour
{
    [SerializeField] private Vector3 positionOffest;
    [SerializeField] private DiffusionManager diffusionManager;
    [SerializeField] private TextMeshProUGUI timeText;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePosition(RoomController room)
    {
        //transform.position = room.transform.position + positionOffest;
        timeText.text = $"{room.name}, {diffusionManager.time + 1}PM";
    }
}
