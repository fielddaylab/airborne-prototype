using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDialogue : MonoBehaviour
{
    [SerializeField] private Vector3 positionOffest;
    [SerializeField] private DiffusionManager diffusionManager;

    enum DiffusionType
    {
        Increase,
        Decrease,
        Spread
    }

    public void UpdatePosition(RoomController room)
    {
        transform.position = room.transform.position + positionOffest;
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
