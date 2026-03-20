using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusionManager : MonoBehaviour
{
    [SerializeField] private string pollutant = "CO";
    
    private DiffusionPhase diffusionPhase = DiffusionPhase.SelectingSource;

    [SerializeField] private MovementDialogue movementDialogue;
    [SerializeField] private SpreadingDialogue spreadingDialogue;
    [SerializeField] private EvidenceBucket evidenceBucket;
    [SerializeField] private TimeBlockButton timeBlockButton;

    private RoomController source;

    [SerializeField] private int time = 0;

    public void SelectedRoom(RoomController room)
    {
        if (diffusionPhase == DiffusionPhase.SelectingSource)
        {
            movementDialogue.UpdatePosition(room);
            movementDialogue.gameObject.SetActive(true);
            source = room;
        }

        if (diffusionPhase == DiffusionPhase.SelectingSpread)
        {

        }
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (diffusionPhase == 0)
            {
                // TODO: Fix this and make it so the dialogue can disappear if a player clicks an empty region of space
                //       right now it overrides selecting increase/decrease which is issue
                //movementDialogue.gameObject.SetActive(false);
            }
        }
    }

    public void AdvanceToNextTimeBlock()
    {
        evidenceBucket.ClearEvidence();
    }

    public void UpdateDiffusionType(int t)
    {
        diffusionPhase = DiffusionPhase.SelectingMovement;

        switch (t)
        {
            case 0:
                evidenceBucket.AddEvidence($"{pollutant} <b>increased</b> in the <b>{source.name}</b>.");
                timeBlockButton.AllowAdvance(true);
                break;
            case 1:
                evidenceBucket.AddEvidence($"{pollutant} <b>decreased</b> in the <b>{source.name}</b>.");
                timeBlockButton.AllowAdvance(true);
                break;
            case 2:
                Debug.Log("Spreading?");
                diffusionPhase = DiffusionPhase.SelectingSpread;

                movementDialogue.gameObject.SetActive(false);

                spreadingDialogue.gameObject.SetActive(true);
                spreadingDialogue.UpdatePosition(source);
                break;
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