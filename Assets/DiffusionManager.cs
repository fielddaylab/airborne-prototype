using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionManager : MonoBehaviour
{
    private DiffusionPhase diffusionPhase = DiffusionPhase.SelectingSource;

    [SerializeField] private GameObject SpreadingDialogue;
    [SerializeField] private GameObject Bucket;
    [SerializeField] private GameObject EvidenceLogger;

    public void SelectedRoom(RoomController room)
    {
        if (diffusionPhase == 0)
        {

        }
    }
}
enum DiffusionPhase
{
    SelectingSource,
    SelectingMovement,
    SelectingSpread,
    SubmitToBucket
}