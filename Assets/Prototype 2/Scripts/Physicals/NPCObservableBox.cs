using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObservableBox : MonoBehaviour
{
    public InvestigationNPCObject NPCData;
    private EquipmentType _lastToolType;
    public GameObject FlyerPrefab;
    public Sprite DialogueSprite;

    public void Start()
    {
        ToolManager.OnToolUpdated += HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered += HandleHourEntered;
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        ToolManager.OnToolUpdated -= HandleToolUpdated;
        InvestigationTimelineSystem.OnHourEntered -= HandleHourEntered;
    }

    private void OnMouseDown()
    {
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;

        NPCTimeSlot slot = NPCData.TimeSlots[index];
        RoomType room = slot.CurrentRoom;

        PlayerKnowledgeState.Discover(NPCData.Character, hour, KnowledgeType.NPCDialogue);
        PlayerKnowledgeState.Discover(NPCData.Character, hour, KnowledgeType.NPCSymptom);

        PlayerKnowledgeState.Discover(slot.Symptom);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        RectTransform canvasRect = CaseFileManager.Instance.AnimatedItemLocation.root as RectTransform;
        Canvas canvas = canvasRect.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.worldCamera;

        GameObject flyer = Instantiate(FlyerPrefab, canvasRect);
        RectTransform flyerRect = flyer.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPoint,
            uiCamera,
            out localPoint
        );

        flyerRect.anchoredPosition = localPoint;

        FlyingIcon flyerIcon = flyer.GetComponent<FlyingIcon>();


        Symptom symptomType = slot.Symptom;
        string dialogue = slot.CharacterDialogue;

        Sprite flySprite;
        if (symptomType != Symptom.None)
        {
            flySprite = InvestigationLookup.Instance.SymptomMap.GetSprite(symptomType);
        } else
        {
            flySprite = DialogueSprite;
        }

        flyerIcon.Setup(flySprite, CaseFileManager.Instance.AnimatedItemLocation); 

        VisibilityCheck();
    }

    private void HandleToolUpdated(EquipmentType type)
    {
        _lastToolType = type;
        if (type == EquipmentType.Observe)
        {
            VisibilityCheck();
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleHourEntered(int h)
    {
        if (_lastToolType == EquipmentType.Observe) VisibilityCheck();
    }

    private void VisibilityCheck()
    {
        // only show box as observable when info not known
        
        int hour = InvestigationTimelineSystem.Instance.CurrentHour;
        int index = hour - InvestigationTimelineSystem.Instance.BaseHour;

        NPCTimeSlot slot = NPCData.TimeSlots[index];
        RoomType room = slot.CurrentRoom;

        bool knowsDialogue = PlayerKnowledgeState.IsKnownCharacterly(NPCData.Character, hour, KnowledgeType.NPCDialogue);
        bool knowsSymptom = PlayerKnowledgeState.IsKnownCharacterly(NPCData.Character, hour, KnowledgeType.NPCSymptom);
        
        bool somethingToDisplay = false;
        if (slot.CharacterDialogue != "" || slot.Symptom != Symptom.None) somethingToDisplay = true; 

        if (somethingToDisplay && (!knowsDialogue || !knowsSymptom))
        {
            gameObject.SetActive(true);
            return;
        }

        gameObject.SetActive(false);
    }   
}
