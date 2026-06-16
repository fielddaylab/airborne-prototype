using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Investigation/NPC")]
public class InvestigationNPCObject : ScriptableObject 
{
    public CharacterType Character;
    public NPCTimeSlot[] TimeSlots;
}

[System.Serializable]
public class NPCTimeSlot
{
    public int Time;
    public RoomType CurrentRoom;
    public string CharacterDialogue;
    public Symptom Symptom;
}