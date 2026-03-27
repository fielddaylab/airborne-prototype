using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementDialogue : MonoBehaviour
{
    [SerializeField] private Vector3 positionOffest;
    [SerializeField] private DiffusionManager diffusionManager;
    [SerializeField] private TextMeshProUGUI timeText;

    enum DiffusionType
    {
        Increase,
        Decrease,
        Spread
    }

    public void UpdatePosition(RoomController room)
    {
        transform.position = room.transform.position + positionOffest;
        timeText.text = $"{room.name}, {diffusionManager.time + 1}PM";
    }

    public void SetToIncreased()
    {
        diffusionManager.UpdateDiffusionType((int)DiffusionType.Increase);
    }

    public void SetToDecreased()
    {
        diffusionManager.UpdateDiffusionType((int)DiffusionType.Decrease);
    }

    public void SetToSpread()
    {
        diffusionManager.UpdateDiffusionType((int)DiffusionType.Spread);
    }
}
