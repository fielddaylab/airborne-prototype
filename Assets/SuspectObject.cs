using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pollution Level")]
public class SuspectObject : ScriptableObject
{
    public string levelName;
    public RoomType sourceRoom;
    public PollutionSource pollutionSource;
    public Pollutant pollutant;

}

public enum RoomType
{
    Kitchen,
    Dining,
    Basement,
    Bedroom
}

public enum PollutionSource
{
    Furnace,
    Stove
}

public enum Pollutant
{
    CO2,
    NO,
    O3
}