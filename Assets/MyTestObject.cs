using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Create new food")]
public class NewBehaviourScript : ScriptableObject
{
    public string foodName;
    public int totalCalories;
    public bool needsCooking;
}
