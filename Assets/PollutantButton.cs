using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutantButton : MonoBehaviour
{
    public Pollutant pollutantType;

    public void Selection()
    {
        Game.Instance.CheckPollutant(pollutantType);
    }
}
