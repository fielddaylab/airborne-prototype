using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutantButton : MonoBehaviour
{
    public PollutantType pollutantType;

    public void Selection()
    {
        GameManager.Instance.CheckPollutant(pollutantType);
    }
}
