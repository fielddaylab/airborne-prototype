using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    [SerializeField] private Slider timeIndicator;
    [SerializeField] private DiffusionManager diffusionManager;

    public int time = 1;

    public void ChangeTime(int i)
    {
        diffusionManager.AddTime(i);
        time = diffusionManager.time;

        timeIndicator.value = time;
    }
}
